# 前置配置（SETUP）

> 原文档要求「前置配置必须设置，否则报错」。以下为完整清单，已尽量自动化。

## 1. 场景挂载

- 新建空物体并命名为 `GameGlobal`，挂载 `GameGlobal` 脚本；
  - 该脚本通过 `RuntimeInitializeOnLoadMethod` 会在进入游戏时**自动创建**
    `GameModeManager` 与 `GlobalEventCenter`，即使忘记手动挂载也不会报错。
- 若需手动控制，也可直接创建空物体并分别挂载 `GameModeManager`、
  `GlobalEventCenter` 两个脚本。

## 2. Layer 添加

在 `Edit → Project Settings → Tags and Layers` 中添加以下 Layer：

```
Player、Enemy、Bullet、Interactable、Destructible、Vehicle、Pickup
```

## 3. Tag 添加

在 `Edit → Project Settings → Tags and Layers` 中添加以下 Tag：

```
Player、Enemy、Head、Cleanable、Vehicle、Pickup、Boss
```

## 4. ObjectPoolManager 对象池 Key

若接入对象池模块，请注册以下 Key（本仓库当前为精简版，对象池为后续里程碑）：

```
bullet、shell、ui_notice、ui_killfeed、ui_damagefloat、ui_damage_dir、ui_pickuptip、explosion
```

## 5. Canvas 渲染模式

- 伤害飘字使用 **World Space** UI 相机；
- HUD 使用 **Screen Space - Overlay**；
- 建议两者叠加使用，飘字挂载世界 UI 相机以贴合受击点。

## 6. 输入设置（Input Manager）

默认使用旧版 Input Manager 轴与按键：

| 输入 | 默认映射 |
| --- | --- |
| Horizontal / Vertical | WASD 或方向键 |
| Jump | Space |
| 奔跑 | 左 Shift |

如需接入新输入系统（Input System），可在 `PlayerFull.ApplyMovement` 中替换。
