# 贡献指南

感谢你对 Unity-Commercial-FPS 的关注！欢迎通过以下方式参与贡献。

## 提交 Issue

- 提交 Bug 前，请先搜索是否已有相同 Issue。
- 尽量包含：Unity 版本、复现步骤、期望与实际行为、相关报错日志。

## 提交 Pull Request

1. Fork 本仓库并创建特性分支：`git checkout -b feature/你的功能`
2. 遵循现有代码风格：
   - 使用 `CommercialFPS` 命名空间；
   - 管理器类继承 `Singleton<T>`，通过 `Instance` 访问；
   - 跨模块通信使用 `GlobalEventCenter.Raise(...)`，避免硬引用。
3. 保持中文注释风格与现有文件一致。
4. 提交前确认脚本在 Unity 中无编译错误。
5. 提交清晰、简洁的 commit message。

## 代码规范速查

- 命名：类 `PascalCase`，私有字段 `_camelCase`，公开属性 `PascalCase`。
- 配置项优先使用 `[SerializeField] private`，仅对外暴露只读属性。
- 避免魔术数字，关键参数抽出为可序列化字段。

## 行为准则

请遵守 [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md)。
