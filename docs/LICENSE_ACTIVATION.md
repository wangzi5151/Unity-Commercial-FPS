# Unity 许可证激活（云端出包前置）

> 云端（GameCI）出 APK 需要一个有效的 Unity 许可证 `.ulf` 文件。
> 这是 Unity 的强制要求，**无法用账号密码自动激活**——Unity 在签发许可证时
> 需要经过带验证码的账号登录，任何自动化都无法绕过。

## 背景

- GameCI 对**个人版（免费）**许可证的激活方式只有一种：读取 `UNITY_LICENSE` 密钥中的 `.ulf` 文件内容。
- 仅提供 `UNITY_EMAIL` + `UNITY_PASSWORD` **无法**激活个人版（已在 GameCI 源码中确认，
  个人版必须走 `.ulf` 文件；账号密码只对 Pro/Plus 的 `UNITY_SERIAL` 生效）。
- 因此，出包前需要一次性生成 `.ulf` 文件，之后即可全自动复用。

## 获取 `.ulf` 文件（任选其一）

### 方式 A：电脑上装 Unity Hub（推荐，生成可移植许可证）

1. 在 Windows / Mac 电脑上安装 [Unity Hub](https://unity.com/download)。
2. 登录 Unity 账号，进入 `Preferences → Licenses`，点击 `Add`，
   选择 **Get a free personal license**（获取免费个人版许可证）。
3. 许可证文件位置：
   - Windows：`C:\ProgramData\Unity\Unity_lic.ulf`
   - Mac：`/Library/Application Support/Unity/Unity_lic.ulf`
   - Linux：`~/.local/share/unity3d/Unity/Unity_lic.ulf`
4. 用文本编辑器打开该 `.ulf` 文件，复制全部内容。

> 该许可证不绑定具体机器，可直接用于云端 CI 构建。

### 方式 B：手机浏览器手动激活

1. 打开 https://license.unity3d.com/manual 并登录 Unity 账号。
2. 按页面提示完成手动激活，下载 `.ulf` 文件。

## 配置密钥

把 `.ulf` 文件内容写入仓库 Secret（或交给维护者代写）：

- 密钥名：`UNITY_LICENSE`
- 值：`.ulf` 文件的完整内容

同时需已配置 `UNITY_EMAIL` 与 `UNITY_PASSWORD`（用于激活校验）。

## 触发出包

配置完成后，进入 `Actions → Build Android APK → Run workflow`，
APK 产物会出现在该次运行的 Artifacts 中（`Unity-Commercial-FPS-APK`）。
