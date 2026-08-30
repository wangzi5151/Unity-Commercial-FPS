using UnityEngine;

namespace CommercialFPS
{
    /// <summary>
    /// 场景引导组件。挂载到空物体 <c>GameGlobal</c> 上即可自动装配全部核心管理器。
    /// 同时提供 <see cref="RuntimeInitializeOnLoadMethod"/> 自动引导，
    /// 即使忘记手动放置也能在进入游戏时自动创建，避免「忘记挂载脚本」的报错。
    /// </summary>
    public class GameGlobal : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoBootstrap()
        {
            if (FindObjectOfType<GameModeManager>() != null)
            {
                return;
            }

            var root = new GameObject(nameof(GameGlobal));
            DontDestroyOnLoad(root);
            root.AddComponent<GameModeManager>();
            root.AddComponent<GlobalEventCenter>();
        }
    }
}
