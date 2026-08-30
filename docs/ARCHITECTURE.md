# 架构设计

## 概述

本项目由《Unity 商业 FPS 完整整合全套源码》合并脚本重构而来。
原脚本把所有类塞进一个文件，本仓库按职责拆分为 `Core` / `Player` 两层，
并为后续扩展（敌人 / 武器 / 任务等）预留目录。

## 目录职责

| 目录 | 职责 |
| --- | --- |
| `Assets/Scripts/Core` | 引擎无关的核心框架：枚举、单例基类、难度、事件、引导 |
| `Assets/Scripts/Player` | 玩家逻辑 |

## 核心设计

### 1. Singleton<T> 单例基类

统一原脚本中 `ins` / `Ins` / `Instance` 混用问题：

```csharp
public class GameModeManager : Singleton<GameModeManager> { ... }
// 访问：GameModeManager.Instance
```

处理了重复实例销毁与跨场景常驻，`OnDestroy` 自动清理静态引用，避免悬空引用。

### 2. 事件驱动的模块解耦

`GlobalEventCenter` 提供 16 种游戏事件，模块间不再直接互相持有引用：

```csharp
GlobalEventCenter.Raise(GameEvent.OnPlayerLevelUp, playerLevel);
```

`Raise` 为静态安全入口，实例未初始化时不会抛空引用异常，从而修复了
原脚本「硬引用 HUD 管理器」可能带来的内存泄漏。

### 3. 数据驱动的难度系统

原脚本用 `switch` 硬编码难度倍率。重构后通过 `DifficultyConfig[]` 序列化数组
承载，策划可直接在 Inspector 中调整倍率，无需改动代码：

```csharp
[SerializeField] private DifficultyConfig[] difficultyTable;
```

顺序需与 `GameDifficulty` 枚举一致，缺失时会在运行时回退到内置默认表。

### 4. 运行时引导

`GameGlobal` 通过 `RuntimeInitializeOnLoadMethod` 在场景加载前自动创建核心管理器，
从根源上杜绝「忘记挂载脚本导致的报错」。

## 数据流

```
玩家受伤 → PlayerFull.TakeDamage → GlobalEventCenter.Raise(OnPlayerHurt)
                                     → HUD / 音效 / 成就 等订阅者
```

## 后续里程碑

- [ ] 武器系统（射击、换弹、后坐力）
- [ ] 敌人 AI（状态机：Idle→Alert→Chase→Attack）
- [ ] 波次 / Boss 战 / 空投 / 天气
- [ ] 任务系统与成就系统
- [ ] 对象池（bullet / shell / 飘字 / 拾取提示 / 爆炸）
