using System;
using UnityEngine;

namespace CommercialFPS
{
    /// <summary>
    /// 全局事件中心，用于模块间解耦通信，修复硬引用带来的内存泄漏问题。
    /// 通过 <see cref="Raise"/> 静态方法可在任意位置安全广播事件。
    /// </summary>
    public class GlobalEventCenter : Singleton<GlobalEventCenter>
    {
        /// <summary>全局游戏事件回调。</summary>
        public event Action<GameEvent, object> OnGameEvent;

        public void TriggerEvent(GameEvent evt, object param = null)
        {
            OnGameEvent?.Invoke(evt, param);
        }

        /// <summary>
        /// 静态广播入口：即使实例尚未初始化也不会抛空引用异常，
        /// 便于在任意 MonoBehaviour 中直接调用。
        /// </summary>
        public static void Raise(GameEvent evt, object param = null)
        {
            if (Instance != null)
            {
                Instance.TriggerEvent(evt, param);
            }
        }

        protected override void OnDestroy()
        {
            OnGameEvent = null;
            base.OnDestroy();
        }
    }
}
