using UnityEngine;

namespace CommercialFPS
{
    /// <summary>
    /// 通用 MonoBehaviour 单例基类。
    /// 统一了原合并脚本中 `ins` / `Ins` 命名不一致的问题，
    /// 并处理重复实例销毁与跨场景常驻（DontDestroyOnLoad）。
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
