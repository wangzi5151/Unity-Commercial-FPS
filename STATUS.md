# 项目现状与配置难点（归档）

> 本文件记录了截至 2026-08-30 的完整进度、已完成的配置、以及云端出包
> 卡住的原因和后续恢复步骤。以后再继续时，只看本文件即可。

## 一、项目是什么

`Unity-Commercial-FPS` —— 由 `A手机备用U盘/GitHub项目/3D.docx` 中
《Unity 商业 FPS 完整整合全套源码》重构而来的 Unity C# 框架工程。

- 源码文档本身在 `PlayerFull.AddExp` 处被截断，已补全。
- 原合并脚本的工程化问题（命名不一致、硬编码、硬引用内存泄漏）已修复。

## 二、目录结构

```
Unity-Commercial-FPS/
├── Assets/
│   ├── Editor/BuildScript.cs      # 云端构建入口（自动装配场景 + 打 APK）
│   └── Scripts/
│       ├── Core/                   # Singleton/GameEnums/GameModeManager/
│       │                           # GlobalEventCenter/GameGlobal
│       └── Player/PlayerFull.cs
├── Packages/manifest.json          # 内建模块清单
├── ProjectSettings/ProjectVersion.txt  # Unity 2022.3.22f1
├── docs/                           # SETUP / ARCHITECTURE / LICENSE_ACTIVATION
├── .github/workflows/build-apk.yml # GameCI 云端出包（手动触发）
├── LICENSE / README / CONTRIBUTING / CHANGELOG / CODE_OF_CONDUCT / SECURITY
└── .gitignore
```

## 三、已完成事项

1. **本地环境无法出包** —— Android Termux 环境无 Unity Editor（Unity 出 APK
   必须桌面版 Unity，无法在本机完成）。
2. **GitHub 仓库已建并推送** —— https://github.com/wangzi5151/Unity-Commercial-FPS
   （默认分支 `master`，公开仓库，账号 `wangzi5151`）。
3. **Unity 工程已补全** —— `ProjectVersion.txt`、`Packages/manifest.json`、
   `BuildScript.cs`（批处理下自动建场景并打 Android APK）。
4. **GameCI 工作流已配好** —— `build-apk.yml` 使用 `game-ci/unity-builder@v4`，
   `unityVersion: 2022.3.22f1`，`buildMethod: CommercialFPS.Editor.BuildScript.BuildAndroid`，
   产物上传为 `Unity-Commercial-FPS-APK`。已改为**手动触发**（`workflow_dispatch`），
   避免每次 push 都跑失败的构建。
5. **密钥已写入**（仓库 Settings → Secrets）：
   - `UNITY_EMAIL` = Unity 账号邮箱
   - `UNITY_PASSWORD` = Unity 账号密码
   - ⚠️ 还缺 `UNITY_LICENSE`（见下文"配置难点"）。

## 四、配置难点（踩坑记录）

### 1. 本地无法打包（根本性）
Android 手机 + Termux 没有 Unity Editor，Unity 项目无法在本机编译成 APK。
结论：只能走云端（GameCI）。

### 2. GameCI 流水线本身已验证可跑通
实际运行 `build-apk.yml`，以下步骤全部成功：
`checkout → free-disk-space → cache → unity-builder 启动 → 生成版本号 0.0.1、versionCode 1`
唯一报错是许可证（下一条）。即：**排除许可证，流水线是通的**。

### 3. Unity 许可证（当前唯一卡点）
- 报错原文：`Missing Unity License File and no Serial was found.`
- 阅读 GameCI 源码 `activate.sh` 确认：**个人免费版只有 `.ulf` 文件一种激活方式**，
  仅 `UNITY_EMAIL` + `UNITY_PASSWORD` 无法激活个人版；账号密码只对
  Pro/Plus 的 `UNITY_SERIAL` 生效。
- `.ulf` 文件必须由用户本人在 Unity 官网/Hub 登录（带验证码）后签发，
  **无法自动化绕过**。

### 4. GameCI 相关 action 版本坑
- `game-ci/unity-request-activation-file@v4` **不存在**（正确是 `@v2`）。
- `game-ci/unity-request-activation-file@v2` 已**弃用**，提示改用新的激活流程
  （即 Unity Hub 本地生成 `.ulf`）。所以已删除该激活工作流，
  改为文档 [docs/LICENSE_ACTIVATION.md](docs/LICENSE_ACTIVATION.md)。

### 5. GitHub 平台小坑
- 工作流文件 push 后有时不会立即注册（`gh workflow list` 看不到），
  重新 push 一次即可。
- Termux 下 `git push` 偶发 SSL `unexpected eof` / 超时，多试几次或加大超时即可。

## 五、后续恢复步骤（一次性，约 2 分钟）

1. 在任意电脑装 [Unity Hub](https://unity.com/download)，登录 Unity 账号
   （邮箱已存于 `UNITY_EMAIL` 密钥）。
2. `Preferences → Licenses → Add → Get a free personal license`。
3. 找到 `.ulf` 文件：
   - Windows：`C:\ProgramData\Unity\Unity_lic.ulf`
   - Mac：`/Library/Application Support/Unity/Unity_lic.ulf`
4. 用文本编辑器打开，复制全部内容，写入仓库 Secret `UNITY_LICENSE`
   （或发给维护者代写）。
5. 回到 `Actions → Build Android APK → Run workflow`，APK 出现在本次运行的
   Artifacts（`Unity-Commercial-FPS-APK`）。

详见 [docs/LICENSE_ACTIVATION.md](docs/LICENSE_ACTIVATION.md)。

## 六、备注

- 当前框架是"核心逻辑"，无美术资源、无完整可玩场景；云端出包会得到一个
  能启动、但内容为空的 APK（管理器自动装配）。真正的游戏内容（敌人/武器/任务等）
  见 [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) 的"后续里程碑"。
- 未提交密钥到仓库（`UNITY_EMAIL`/`UNITY_PASSWORD` 仅存于 GitHub Secrets，
  `.gitignore` 已排除 `*.apk`、`*.keystore`、`*.jks` 等敏感文件）。
