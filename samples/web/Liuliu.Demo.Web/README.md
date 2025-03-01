# OSharp多租户访问实现

本项目实现了OSharp框架的多种多租户访问方式，包括：

## 支持的租户识别方式

1. **域名识别**：根据请求的域名识别租户，例如 `tenant1.example.com`
2. **请求头识别**：通过HTTP请求头识别租户，默认使用 `X-Tenant` 请求头
3. **查询参数识别**：通过URL查询参数识别租户，默认使用 `tenant` 参数，例如 `?tenant=tenant1`
4. **Cookie识别**：通过Cookie识别租户，默认使用 `tenant` Cookie
5. **Claims识别**：通过用户Claims识别租户，默认使用 `tenant_id` Claim
6. **路由参数识别**：通过路由参数识别租户，默认使用 `tenant` 路由参数

## 配置说明

在 `appsettings.json` 中配置多租户识别方式：

```json
"MultiTenancy": {
  "TenantResolve": {
    "EnableDomain": true,
    "EnableHeader": true,
    "HeaderName": "X-Tenant",
    "EnableQueryString": true,
    "QueryStringName": "tenant",
    "EnableCookie": true,
    "CookieName": "tenant",
    "EnableClaim": true,
    "ClaimType": "tenant_id",
    "EnableRoute": true,
    "RouteParamName": "tenant"
  }
}
```

配置租户信息：

```json
"Tenants": {
  "tenant1": {
    "Name": "租户1",
    "Host": "tenant1.example.com",
    "ConnectionString": "",
    "IsEnabled": true
  },
  "tenant2": {
    "Name": "租户2",
    "Host": "tenant2.example.com",
    "ConnectionString": "",
    "IsEnabled": true
  }
}
```

配置租户数据库连接字符串：

```json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=OSharpDemo;User Id=sa;Password=Password123;MultipleActiveResultSets=true",
  "tenant1": {
    "Default": "Server=localhost;Database=OSharpDemo_Tenant1;User Id=sa;Password=Password123;MultipleActiveResultSets=true"
  },
  "tenant2": {
    "Default": "Server=localhost;Database=OSharpDemo_Tenant2;User Id=sa;Password=Password123;MultipleActiveResultSets=true"
  }
}
```

## 使用方法

### 1. 通过域名访问

直接通过配置的租户域名访问应用，例如：`http://tenant1.example.com/api/tenant/current`

### 2. 通过请求头访问

在请求中添加 `X-Tenant` 请求头：

```
GET /api/tenant/current
X-Tenant: tenant1
```

### 3. 通过查询参数访问

在URL中添加 `tenant` 查询参数：

```
GET /api/tenant/current?tenant=tenant1
```

### 4. 通过Cookie访问

设置Cookie后访问：

```
GET /api/tenant/set-cookie/tenant1
```

然后访问：

```
GET /api/tenant/current
```

### 5. 通过Claims访问

用户登录后，如果用户的Claims中包含 `tenant_id` Claim，则会自动识别租户。

### 6. 通过路由参数访问

在路由中包含租户参数：

```
GET /tenant1/api/tenant/current
```

## API接口

- `GET /api/tenant/current`：获取当前租户信息
- `GET /api/tenant`：获取所有租户信息
- `GET /api/tenant/set-cookie/{tenantId}`：设置租户Cookie
- `GET /api/tenant/clear-cookie`：清除租户Cookie

## 多租户数据库访问

系统会根据当前租户自动选择对应的数据库连接字符串，优先级如下：

1. 租户自己的连接字符串（TenantInfo.ConnectionString）
2. 配置中特定租户的特定DbContext连接字符串（ConnectionStrings:tenant1:DbContextName）
3. 配置中特定租户的默认连接字符串（ConnectionStrings:tenant1:Default）
4. 应用程序的特定DbContext连接字符串（ConnectionStrings:DbContextName）
5. 应用程序的默认连接字符串（ConnectionStrings:Default） 