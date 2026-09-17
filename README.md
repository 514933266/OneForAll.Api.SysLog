# OneForAll.Api.SysLog - 系统日志服务

蜂窝云平台的集中式系统日志微服务，负责采集和管理 API 日志、异常日志、登录日志、操作日志及全局异常日志。

## 技术栈

| 技术 | 版本 | 说明 |
|------|------|------|
| .NET | 8.0 | 运行时框架 |
| ASP.NET Core | 8.0 | Web API 框架 |
| Entity Framework Core | 8.0.22 | ORM |
| SQL Server | - | 数据库 |
| Autofac | 9.0.0 | 依赖注入容器 |
| AutoMapper | 16.0.0 | 对象映射 |
| Quartz.NET | 3.15.1 | 定时任务调度 |
| StackExchange.Redis | 8.0.22 | 分布式缓存（可选） |
| JWT Bearer | 8.0.22 | 身份认证 |

## 项目结构

```
OneForAll.Api.SysLog/
├── SysLog.Host/                  # 宿主层 - Web API 入口
│   ├── Controllers/              # API 控制器
│   ├── Filters/                  # 中间件过滤器（API日志、异常处理）
│   ├── Providers/                # 自定义提供者（租户、任务工厂）
│   ├── QuartzJobs/               # Quartz 定时任务
│   ├── Profiles/                 # AutoMapper 映射配置
│   ├── Models/                   # 配置模型
│   ├── Program.cs                # 应用入口
│   ├── Startup.cs                # 服务配置
│   └── SysLogDbContext.cs        # 数据库上下文
│
├── SysLog.Application/           # 应用层 - 业务编排
│   ├── Interfaces/               # 服务接口
│   ├── Dtos/                     # 数据传输对象
│   └── *Service.cs               # 应用服务实现
│
├── SysLog.Domain/                # 领域层 - 核心业务逻辑
│   ├── Entities/                 # 领域实体
│   ├── Interfaces/               # 领域接口
│   ├── Models/                   # 请求/表单模型
│   ├── Aggregates/               # 聚合根
│   ├── Enums/                    # 枚举
│   ├── Repositorys/              # 仓储接口
│   ├── ValueObjects/             # 值对象
│   └── *Manager.cs               # 领域管理器
│
├── SysLog.Repository/            # 仓储层 - 数据访问
│   └── *Repository.cs            # 仓储实现
│
├── SysLog.HttpService/           # HTTP服务层 - 外部服务调用
│   ├── Interfaces/               # 服务接口
│   ├── Models/                   # 配置与DTO
│   └── *Service.cs               # HTTP服务实现
│
├── SysLog.Public/                # 公共层 - 共享模型
│   └── Models/                   # 共享DTO
│
└── SysLog.sln                    # 解决方案文件
```

**架构模式**: 整洁架构（Clean Architecture）/ 领域驱动设计（DDD），各层职责明确，依赖关系由外向内。

## 核心功能

### 日志采集

| 日志类型 | 实体 | 数据表 | 说明 |
|----------|------|--------|------|
| API 日志 | `SysApiLog` | `sys_api_log` | HTTP 请求/响应记录，含方法、URL、状态码、耗时等 |
| 异常日志 | `SysExceptionLog` | `sys_exception_log` | 应用异常，含堆栈信息 |
| 登录日志 | `SysLoginLog` | `sys_login_log` | 用户登录事件，含登录方式、来源、IP |
| 操作日志 | `SysOperationLog` | `sys_operation_log` | 用户操作审计，含操作类型、内容 |
| 全局异常 | `SysGlobalExceptionLog` | `sys_global_exception_log` | 未处理的全局异常 |

### 日志清理策略

通过 Quartz 定时任务 `DeleteSysLogJob` 自动清理过期日志（默认每日凌晨 2:00 执行）：

| 日志类型 | 保留天数 |
|----------|----------|
| API 日志 | 3 天 |
| 异常日志 | 7 天 |
| 全局异常 | 7 天 |
| 登录日志 | 15 天 |
| 操作日志 | 15 天 |

### 其他功能

- **用户活跃度统计** - 基于 API 日志聚合计算用户活跃指标
- **日志过滤配置** - 通过 `SysFilterLogConfig` 配置需排除的日志规则
- **多租户隔离** - 基于 JWT Claims 的租户数据隔离
- **Webhook 通知** - 支持钉钉/企业微信机器人异常告警

## API 接口

### 日志写入（匿名访问）

| 方法 | 路径 | 说明 |
|------|------|------|
| POST | `/api/SysApiLogs` | 写入 API 日志 |
| POST | `/api/SysExceptionLogs` | 写入异常日志 |
| POST | `/api/SysLoginLogs` | 写入登录日志 |
| POST | `/api/SysOperationLogs` | 写入操作日志 |
| POST | `/api/SysGlobalExceptionLogs` | 写入全局异常日志 |

### 日志查询（需认证）

| 方法 | 路径 | 权限 | 说明 |
|------|------|------|------|
| GET | `/api/SysApiLogs/{pageIndex}/{pageSize}` | Ruler | 分页查询 API 日志 |
| GET | `/api/SysExceptionLogs/{pageIndex}/{pageSize}` | Ruler | 分页查询异常日志 |
| GET | `/api/SysLoginLogs/{pageIndex}/{pageSize}` | - | 分页查询登录日志 |
| GET | `/api/SysOperationLogs/{pageIndex}/{pageSize}` | - | 分页查询操作日志 |
| GET | `/api/SysGlobalExceptionLogs/{pageIndex}/{pageSize}` | - | 分页查询全局异常 |
| GET | `/api/Liveness` | Admin | 用户活跃度统计 |
| GET | `/api/Activitys` | - | 用户活动记录 |

**通用查询参数**: `startTime`, `endTime`, `userName`, `controller`, `action`, `key`

## 快速开始

### 环境要求

- .NET 8.0 SDK
- SQL Server
- Redis（可选）

### 配置

编辑 `SysLog.Host/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=<服务器>; Initial Catalog=OneForAll.SysLog; User Id=<用户名>; Password=<密码>; Trust Server Certificate=true"
  },
  "Auth": {
    "JwtKey": "<32位以上密钥>",
    "Issuer": "<Token签发地址>"
  },
  "HttpService": {
    "SysBase": "http://localhost:5082",
    "SysUms": "http://localhost:5085",
    "SysJob": "http://localhost:5087"
  }
}
```

### 运行

```bash
cd SysLog.Host
dotnet restore
dotnet run
```

服务默认运行在 `http://localhost:5084`。

### 数据库迁移

```bash
cd SysLog.Host
dotnet ef database update
```

## 服务依赖

本服务运行时依赖以下蜂窝云微服务：

| 服务 | 默认地址 | 说明 |
|------|----------|------|
| SysBase | `http://localhost:5082` | 基础系统服务 |
| SysUms | `http://localhost:5085` | 用户管理服务 |
| SysJob | `http://localhost:5087` | 定时任务服务 |

## 认证与授权

- **认证方式**: JWT Bearer Token（HS256 对称签名）
- **授权模型**: 基于角色的访问控制（RBAC）
- **角色**: `Ruler`（系统管理员）、`Admin`（管理员）
- **多租户**: 通过 JWT Claims 中的 `TenantId` 实现租户隔离

## 部署

支持以下部署方式：

- **IIS** - 通过 `Web.config` 配置
- **Kestrel 自托管** - ASP.NET Core 默认方式
- **Docker / Kubernetes** - 容器化部署

生产环境注意事项：
1. 更换 JWT 密钥为高强度随机字符串
2. 配置 HTTPS
3. 启用 Redis 缓存
4. 配置 Application Insights 监控
5. 按需调整日志保留策略
