# CLAUDE.md

此文件为 Claude Code (claude.ai/code) 在本项目中工作时提供指导。全程使用中文沟通。

## 项目概述

这是一个 Unity (Tuanjie 2022.3.62t3) 项目，使用 Universal Render Pipeline (URP) 14.1.0，目标是 netstandard2.1。

## 开发命令

- **打开编辑器**: 启动 `D:\Soft\Tuanjie\2022.3.62t3\Editor\Tuanjie.exe`
- **运行测试**: 使用 Unity Test Runner (Window > General > Test Runner)
- **构建**: File > Build Settings > Standalone Windows64

## 架构

- **Assets/Tests/**: 单元测试代码 (基于 NUnit 的性能测试)
- **Assets/Settings/**: URP 管线设置
- **Packages 依赖**: 通过 Packages/manifest.json 管理，使用 scoped registry

## 注意事项

- Unity 项目文件 (.csproj) 是自动生成的 - 请勿直接编辑
- 本项目包含 Unity MCP 包 (com.ivanmurzak.unity.mcp) 用于 Model Context Protocol
- 代码使用 C# 9.0 编译，目标框架是 netstandard2.1