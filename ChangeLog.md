# NewLife.NetPing 版本更新记录

## v1.1.2026.1024 (2026-10-24)

### 依赖升级
- **NewLife.IoT 3.2.2026.1024**：同步最新 IoT 标准库，适配异步优先驱动接口、结构化读取结果与多语言/AOT 能力。

### 工程与协作
- **解决方案迁移**：`NewLife.NetPing.sln` 升级为 `.slnx`，统一解决方案条目与仓库工程视图。
- **CI 发布流水线**：打 tag 时同时发布 NuGet.org、GitHub Packages，并创建同名 GitHub Release；测试流水线增加 slnx 构建与打包校验。
- **文档同步**：Readme 生态版图与团队介绍对齐 NewLife.Core。

---
