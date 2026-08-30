# Unity 商业 FPS 完整整合框架

> 一套模块化、开箱即用的 Unity 第一人称射击（FPS）核心框架。
> 源于《Unity 商业 FPS 完整整合全套源码》合并脚本，已重构为可维护的
> 多文件工程结构，并修复了原脚本中的命名不一致、硬编码与内存泄漏隐患。

![version](https://img.shields.io/badge/version-1.0.0-60a5fa)
![license](https://img.shields.io/badge/license-MIT-green)
![engine](https://img.shields.io/badge/engine-Unity%202021.3%2B-222222)
![language](https://img.shields.io/badge/language-C%23-9b59b6)

## ✨ 为什么是它

原文档将全部脚本合并成一个「复制即用」的超长 C# 文件，虽然方便粘贴，却存在
命名不一致（`ins` / `Ins` 混用）、魔术数字、硬编码层掩码、类间硬引用导致的内存泄漏
等问题，且文档末尾被截断（`AddExp` 未写完）。

本仓库在此基础上做了 **工程化重构**：

- 拆分为职责单一的脚本文件，按 `Core` / `Player` 分层；
- 统一单例模式，提供 `Singleton<T>` 基类；
- 难度倍率抽离为可序列化配置，策划可在 Inspector 直接调参；
- 事件中心提供安全的静态广播入口，模块间彻底解耦；
- 补全被截断的 `AddExp` / `TakeDamage` / `Heal` 逻辑。

## 🚀 功能特性

| 模块 | 说明 |
| --- | --- |
| 🎚️ GameModeManager | 五档难度（简单~地狱）+ 四种玩法模式，难度倍率数据驱动 |
| 📡 GlobalEventCenter | 全局事件总线，16 种游戏事件，解耦模块、修复内存泄漏 |
| 🧍 PlayerFull | 玩家属性、生存消耗、移动跳跃、受伤护甲、等级经验 |
| 🧩 GameEnums | 统一枚举定义（难度 / 玩法 / 事件 / 敌人 / 任务 / 输入缓冲） |
| 🚀 GameGlobal | 场景引导 + 运行时自动装配管理器，杜绝「忘记挂脚本」 |

## 📁 目录结构

```
Unity-Commercial-FPS/
├── Assets/
│   └── Scripts/
│       ├── Core/          # 核心系统（枚举、单例、难度、事件、引导）
│       └── Player/        # 玩家逻辑
├── docs/                  # 前置配置与架构说明
├── .github/               # Issue 模板等
├── LICENSE                # MIT 许可证
├── README.md
├── CONTRIBUTING.md
├── CHANGELOG.md
├── CODE_OF_CONDUCT.md
└── SECURITY.md
```

## 🛠️ 快速开始

1. 使用 **Unity 2021.3 或更高版本** 新建工程，将 `Assets` 目录复制进去
   （或直接以本仓库作为 `Assets` 根目录打开）。
2. 按 [docs/SETUP.md](docs/SETUP.md) 完成 Layer / Tag / 对象池等前置配置。
3. 新建空场景，创建空物体并命名为 `GameGlobal`，挂载 `GameGlobal` 脚本；
   或直接运行 —— 管理器会在加载时自动创建。
4. 运行场景即可体验基础移动、跳跃、生存消耗与难度切换逻辑。

## 📖 文档

- [docs/SETUP.md](docs/SETUP.md) —— 前置配置（Layer / Tag / 对象池 Key / Canvas）
- [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) —— 架构设计与重构说明

## 🤝 贡献

欢迎提交 Issue 与 Pull Request，详见 [CONTRIBUTING.md](CONTRIBUTING.md)。

## 📄 许可证

[MIT License](LICENSE) © 2026 Unity-Commercial-FPS contributors
