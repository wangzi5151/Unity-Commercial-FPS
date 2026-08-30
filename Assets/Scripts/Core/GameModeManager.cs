using UnityEngine;

namespace CommercialFPS
{
    /// <summary>
    /// 难度配置数据。将原 switch 硬编码的倍率抽离为可在 Inspector 中
    /// 直接编辑的序列化结构，便于策划调参，无需改动代码。
    /// </summary>
    [System.Serializable]
    public struct DifficultyConfig
    {
        public string displayName;
        public float enemyHpScale;
        public float enemyDmgScale;
        public float enemySpeedScale;
        public float enemySpawnCountScale;
        public float lootDropRateScale;
        public float playerDamageTakenScale;
    }

    /// <summary>
    /// 游戏难度 / 玩法模式管理器。
    /// 原合并脚本中的 `public static GameModeManager ins` 已统一为
    /// <see cref="Singleton{T}.Instance"/> 单例。
    /// </summary>
    public class GameModeManager : Singleton<GameModeManager>
    {
        [Header("当前状态")]
        public GameDifficulty curDifficulty = GameDifficulty.Normal;
        public GamePlayMode playMode = GamePlayMode.EndlessWave;

        [Header("难度倍率表（顺序需与 GameDifficulty 枚举一致）")]
        [SerializeField] private DifficultyConfig[] difficultyTable;

        /// <summary>当前生效的难度配置。</summary>
        public DifficultyConfig CurrentConfig { get; private set; }

        private void Start()
        {
            if (difficultyTable == null || difficultyTable.Length == 0)
            {
                difficultyTable = CreateDefaultTable();
            }

            ApplyDifficulty();
        }

        public void SetDifficulty(GameDifficulty difficulty)
        {
            curDifficulty = difficulty;
            ApplyDifficulty();
        }

        public void ApplyDifficulty()
        {
            if (difficultyTable == null || difficultyTable.Length == 0)
            {
                return;
            }

            int index = Mathf.Clamp((int)curDifficulty, 0, difficultyTable.Length - 1);
            CurrentConfig = difficultyTable[index];
        }

        public void ToggleSandbox()
        {
            playMode = playMode == GamePlayMode.Sandbox
                ? GamePlayMode.EndlessWave
                : GamePlayMode.Sandbox;
        }

        private static DifficultyConfig[] CreateDefaultTable()
        {
            return new[]
            {
                new DifficultyConfig { displayName = "简单",   enemyHpScale = 0.7f,  enemyDmgScale = 0.6f,  enemySpeedScale = 0.8f,  enemySpawnCountScale = 0.7f,  lootDropRateScale = 1.3f, playerDamageTakenScale = 0.7f },
                new DifficultyConfig { displayName = "普通",   enemyHpScale = 1f,    enemyDmgScale = 1f,    enemySpeedScale = 1f,    enemySpawnCountScale = 1f,    lootDropRateScale = 1f,   playerDamageTakenScale = 1f   },
                new DifficultyConfig { displayName = "困难",   enemyHpScale = 1.3f,  enemyDmgScale = 1.3f,  enemySpeedScale = 1.2f,  enemySpawnCountScale = 1.25f, lootDropRateScale = 0.8f, playerDamageTakenScale = 1.2f },
                new DifficultyConfig { displayName = "噩梦",   enemyHpScale = 1.7f,  enemyDmgScale = 1.7f,  enemySpeedScale = 1.4f,  enemySpawnCountScale = 1.6f,  lootDropRateScale = 0.6f, playerDamageTakenScale = 1.4f },
                new DifficultyConfig { displayName = "地狱",   enemyHpScale = 2.2f,  enemyDmgScale = 2.2f,  enemySpeedScale = 1.7f,  enemySpawnCountScale = 2f,    lootDropRateScale = 0.4f, playerDamageTakenScale = 1.8f }
            };
        }
    }
}
