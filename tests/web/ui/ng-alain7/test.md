### 您的功能请求与现有问题有关吗？请描述
A clear and concise description of what the problem is. Ex. I'm always frustrated when [...]
现在的JWT没法管理已发出的Token，在安全上得不到保障，需要一个Token刷新机制

### 描述您想要的解决方案
A clear and concise description of what you want to happen.

具体流程如下图所示：
![jwt-refresh](https://user-images.githubusercontent.com/3930317/59324179-5b2a4200-8d10-11e9-8335-8fc101dd4bc1.png)

### 一些要点
#### 客户端
- 登录成功之后，存储AccessToken和RefreshToken
- 前端用户信息以AccessToken为准，例如用户信息显示，用户角色判断
- 刷新页面时，拉取用户的所有功能点，用于前端功能点权限判断
- 使用 HttpInterceptor 刷新AccessToken
  - 每次发起Request时，检测AccessToken是否快过期，快过期前，使用RefreshToken刷新AccessToken
  - 接收Response之后，检测是否存在包含Token的Header，存在则更新本地Token

#### 应用服务端
- 验证AccessToken有效性，有效则执行业务，否则返回401给前端，要求刷新AccessToken

#### 认证服务端
- 用户登录成功之后，创建一个与RefreshToken对应的ClientId，当ClientId失效之后，相应的RefreshToken亦失效
- 刷新AccessToken时，如相应的ClientId过期，则RefreshToken过期，返回401要求前端登录，否则返回新的AccessToken和RefreshToken
- 用户登录成功之后，缓存用户的功能点信息
- OnlineUser 新增一个与RefreshToken配套的RefreshOnNext标记，用于决定下次请求时是否刷新AccessToken。当用户信息或用户权限更新之后，即时刷新AccessToken中的用户信息
- 用户信息或权限信息更新之后，更新RefreshOnNext标记，以即时刷新AccessToken
