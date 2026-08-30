#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommercialFPS.Editor
{
    /// <summary>
    /// 云端 CI（GameCI）调用入口：在批处理模式下自动装配场景并打包 Android APK。
    /// 由 .github/workflows/build-apk.yml 通过 buildMethod 参数调用。
    /// </summary>
    public static class BuildScript
    {
        private const string ScenePath = "Assets/Scenes/Main.unity";
        private const string OutputDir = "build/Android";
        private const string ApkName = "Unity-Commercial-FPS.apk";

        public static void BuildAndroid()
        {
            SetupPlayerSettings();
            string scenePath = EnsureScene();

            Directory.CreateDirectory(OutputDir);
            string apkPath = Path.Combine(OutputDir, ApkName);

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { scenePath },
                locationPathName = apkPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new System.Exception("APK 构建失败: " + report.summary.result);
            }

            Debug.Log("APK 构建成功: " + apkPath);
        }

        private static void SetupPlayerSettings()
        {
            PlayerSettings.productName = "Unity-Commercial-FPS";
            PlayerSettings.companyName = "CommercialFPS";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.commercialfps.game");
            PlayerSettings.bundleVersion = "1.0.0";
            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            EditorUserBuildSettings.buildAppBundle = false;
            EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        }

        private static string EnsureScene()
        {
            string scenesDir = "Assets/Scenes";
            if (!Directory.Exists(scenesDir))
            {
                Directory.CreateDirectory(scenesDir);
            }

            if (!File.Exists(ScenePath))
            {
                Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                var global = new GameObject("GameGlobal");
                global.AddComponent<GameModeManager>();
                global.AddComponent<GlobalEventCenter>();
                EditorSceneManager.SaveScene(scene, ScenePath);
            }

            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            return ScenePath;
        }
    }
}
#endif
