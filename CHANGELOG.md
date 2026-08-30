# 更新日志

本项目遵循 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.0.0/) 规范，
版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

## [1.0.0] - 2026-08-29

### 新增

- 从《Unity 商业 FPS 完整整合全套源码》合并脚本重构为多文件工程结构。
- `Singleton<T>` 通用单例基类，统一原脚本中 `ins` / `Ins` 命名不一致。
- `GameModeManager` 难度系统，难度倍率数据驱动（可序列化配置）。
- `GlobalEventCenter` 全局事件中心，新增安全的 `Raise` 静态广播入口。
- `PlayerFull` 玩家完整逻辑，补全原文档中被截断的 `AddExp` 及
  `TakeDamage` / `Heal` / `AddArmor` / `ApplyHurtStun`。
- `GameGlobal` 场景引导，运行时自动装配核心管理器。
- 完整仓库文档：README、LICENSE、CONTRIBUTING、CHANGELOG、CODE_OF_CONDUCT、SECURITY。

### 修复

- 修复单例命名不一致（`ins` vs `Ins`）。
- 修复接地检测硬编码 `~0` 层掩码与 0.9f 固定偏移，改为可配置 LayerMask。
- 修复类间硬引用（HUD）导致的内存泄漏隐患，改为事件解耦。
- 修复文档末尾被截断的 `AddExp` 升级循环逻辑。

[1.0.0]: https://github.com/yourname/Unity-Commercial-FPS/releases/tag/v1.0.0
