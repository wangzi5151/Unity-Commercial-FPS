using System;
using UnityEngine;

namespace CommercialFPS
{
    /// <summary>
    /// 玩家完整逻辑：属性、生存、移动、受伤、等级经验。
    /// 相对原合并脚本的优化：
    ///  - 属性改用 [SerializeField] + 只读属性，内部状态私有化；
    ///  - CharacterController 自动获取，无需手动拖拽；
    ///  - 接地检测使用可配置的 LayerMask 与距离，替换硬编码的 ~0 与 0.9f 偏移；
    ///  - HUD 状态通过事件解耦，不再直接引用 HUDUIManager；
    ///  - 补全原文档中被截断的 AddExp / TakeDamage / Heal 逻辑。
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerFull : Singleton<PlayerFull>
    {
        [Header("生存属性")]
        [SerializeField] private float maxHp = 100f;
        [SerializeField] private float maxArmor = 50f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float maxFood = 100f;
        [SerializeField] private float maxWater = 100f;

        [Header("角色养成")]
        [SerializeField] private int playerLevel = 1;
        [SerializeField] private long playerExp;
        [SerializeField] private long expToNextLevel = 100;
        [SerializeField] private long playerGold;

        [Header("移动参数")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float staminaDrainPerSecond = 10f;

        [Header("接地检测")]
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private float groundCheckDistance = 0.2f;

        [Header("生存消耗")]
        [SerializeField] private float foodDrainPerTick = 1.2f;
        [SerializeField] private float waterDrainPerTick = 1.8f;
        [SerializeField] private float tickInterval = 2f;

        // 运行时状态（只读对外）
        public float Hp { get; private set; }
        public float Armor { get; private set; }
        public float Stamina { get; private set; }
        public float Food { get; private set; }
        public float Water { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsHurtStun { get; private set; }
        public int PlayerLevel => playerLevel;
        public long PlayerExp => playerExp;
        public long ExpToNextLevel => expToNextLevel;
        public long PlayerGold => playerGold;

        /// <summary>HUD 状态更新事件，参数依次为 hp/armor/stamina/food/water。</summary>
        public event Action<float, float, float, float, float> OnStatusChanged;

        private CharacterController _controller;
        private Vector3 _velocity;
        private float _hurtStunTimer;

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<CharacterController>();

            Hp = maxHp;
            Armor = maxArmor;
            Stamina = maxStamina;
            Food = maxFood;
            Water = maxWater;
        }

        private void Start()
        {
            InvokeRepeating(nameof(SurvivalTick), tickInterval, tickInterval);
        }

        private void Update()
        {
            UpdateGroundCheck();

            if (_hurtStunTimer > 0f)
            {
                _hurtStunTimer -= Time.deltaTime;
                IsHurtStun = true;
                return;
            }

            IsHurtStun = false;
            ApplyMovement();
        }

        private void UpdateGroundCheck()
        {
            Vector3 origin = transform.position + Vector3.up * 0.05f;
            IsGrounded = Physics.CheckSphere(origin, groundCheckDistance, groundMask,
                QueryTriggerInteraction.Ignore);
        }

        private void ApplyMovement()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = (transform.right * h + transform.forward * v).normalized;
            bool wantsRun = Input.GetKey(KeyCode.LeftShift)
                            && moveDir.sqrMagnitude > 0.01f
                            && Stamina > 0f;

            float speed = wantsRun ? runSpeed : moveSpeed;
            _controller.Move(moveDir * speed * Time.deltaTime);

            if (wantsRun)
            {
                Stamina = Mathf.Max(0f, Stamina - staminaDrainPerSecond * Time.deltaTime);
            }
            else if (IsGrounded)
            {
                Stamina = Mathf.Min(maxStamina, Stamina + staminaDrainPerSecond * 0.5f * Time.deltaTime);
            }

            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -Physics.gravity.y);
            }

            _velocity.y += Physics.gravity.y * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void SurvivalTick()
        {
            Food = Mathf.Clamp(Food - foodDrainPerTick, 0f, maxFood);
            Water = Mathf.Clamp(Water - waterDrainPerTick, 0f, maxWater);

            if (Food <= 0f)
            {
                TakeDamage(3f, ignoreArmor: true);
            }

            if (Water <= 0f)
            {
                TakeDamage(5f, ignoreArmor: true);
            }

            OnStatusChanged?.Invoke(Hp, Armor, Stamina, Food, Water);
        }

        public void TakeDamage(float damage, bool ignoreArmor)
        {
            if (Hp <= 0f)
            {
                return;
            }

            if (!ignoreArmor && Armor > 0f)
            {
                float absorbed = Mathf.Min(Armor, damage);
                Armor -= absorbed;
                damage -= absorbed;
            }

            if (damage <= 0f)
            {
                return;
            }

            Hp = Mathf.Max(0f, Hp - damage);
            GlobalEventCenter.Raise(GameEvent.OnPlayerHurt, damage);

            if (Hp <= 0f)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            Hp = Mathf.Min(maxHp, Hp + amount);
        }

        public void AddArmor(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            Armor = Mathf.Min(maxArmor, Armor + amount);
        }

        public void ApplyHurtStun(float duration)
        {
            _hurtStunTimer = Mathf.Max(_hurtStunTimer, duration);
            IsHurtStun = true;
        }

        public void AddExp(long amount)
        {
            playerExp += amount;

            while (playerExp >= expToNextLevel)
            {
                playerExp -= expToNextLevel;
                playerLevel++;
                expToNextLevel = (long)(expToNextLevel * 1.3f);

                GlobalEventCenter.Raise(GameEvent.OnPlayerLevelUp, playerLevel);
            }
        }

        public void AddGold(long amount)
        {
            playerGold += amount;
        }

        private void Die()
        {
            GlobalEventCenter.Raise(GameEvent.OnPlayerDeath, null);
        }
    }
}
