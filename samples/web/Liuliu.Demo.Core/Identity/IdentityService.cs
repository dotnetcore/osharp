// -----------------------------------------------------------------------
//  <copyright file="IdentityService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Identity.Dtos;
using Liuliu.Demo.Identity.Entities;
using Liuliu.Demo.Identity.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OSharp.Data;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Extensions;
using OSharp.Identity;
using OSharp.Identity.OAuth2;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

using OSharp.Caching;


namespace Liuliu.Demo.Identity
{
    /// <summary>
    /// 业务实现：身份认证模块
    /// </summary>
    public partial class IdentityService : IIdentityContract
    {
        private readonly IEventBus _eventBus;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<UserDetail, int> _userDetailRepository;
        private readonly IRepository<UserLogin, Guid> _userLoginRepository;
        private readonly IDistributedCache _cache;
        private readonly IPrincipal _currentUser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<UserRole, Guid> _userRoleRepository;
        private readonly ILogger<IdentityService> _logger;

        /// <summary>
        /// 初始化一个<see cref="IdentityService"/>类型的新实例
        /// </summary>
        public IdentityService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IEventBus eventBus,
            ILoggerFactory loggerFactory,
            IRepository<UserRole, Guid> userRoleRepository,
            IRepository<UserDetail, int> userDetailRepository,
            IRepository<UserLogin, Guid> userLoginRepository,
            IDistributedCache cache,
            IPrincipal currentUser,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _eventBus = eventBus;
            _logger = loggerFactory.CreateLogger<IdentityService>();
            _userRoleRepository = userRoleRepository;
            _userDetailRepository = userDetailRepository;
            _userLoginRepository = userLoginRepository;
            _cache = cache;
            _currentUser = currentUser;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取 角色信息查询数据集
        /// </summary>
        public IQueryable<Role> Roles
        {
            get { return _roleManager.Roles; }
        }

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<User> Users
        {
            get { return _userManager.Users; }
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="dto">注册信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult<User>> Register(RegisterDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            User user = new User() { UserName = dto.UserName, NickName = dto.NickName ?? dto.UserName, Email = dto.Email };
            IdentityResult result = dto.Password == null ? await _userManager.CreateAsync(user) : await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return result.ToOperationResult(user);
            }
            UserDetail detail = new UserDetail() { RegisterIp = dto.RegisterIp, UserId = user.Id };
            int count = await _userDetailRepository.InsertAsync(detail);

            //触发注册成功事件
            if (count > 0)
            {
                RegisterEventData eventData = new RegisterEventData() { RegisterDto = dto, User = user };
                await _eventBus.PublishAsync(eventData);
                return new OperationResult<User>(OperationResultType.Success, "用户注册成功", user);
            }
            return new OperationResult<User>(OperationResultType.NoChanged);
        }
        
        /// <summary>
        /// 使用账号登录
        /// </summary>
        /// <param name="dto">登录信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult<User>> Login(LoginDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            User user = await FindUserByAccount(dto.Account);
            if (user == null)
            {
                return new OperationResult<User>(OperationResultType.Error, "用户不存在");
            }
            if (user.IsLocked)
            {
                return new OperationResult<User>(OperationResultType.Error, "用户已被冻结，无法登录");
            }
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
            OperationResult<User> result = ToOperationResult(signInResult, user);
            if (!result.Succeeded)
            {
                return result;
            }
            _logger.LogInformation(1, $"用户 {user.Id} 使用账号登录系统成功");

            //触发登录成功事件
            LoginEventData loginEventData = new LoginEventData() { LoginDto = dto, User = user };
            await _eventBus.PublishAsync(loginEventData);

            return result;
        }

        /// <summary>
        /// 使用第三方用户信息进行OAuth2登录
        /// </summary>
        /// <param name="loginInfo">第三方用户信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> LoginOAuth2(UserLoginInfoEx loginInfo)
        {
            SignInResult result = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, true);
            if (!result.Succeeded)
            {
                string cacheId = await SetLoginInfoEx(loginInfo);
                return new OperationResult(OperationResultType.Error, "登录失败", cacheId);
            }

            User user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            return new OperationResult(OperationResultType.Success, "登录成功", user);
        }

        /// <summary>
        /// 登录并绑定现有账号
        /// </summary>
        /// <param name="loginInfoEx">第三方登录信息</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult<User>> LoginBind(UserLoginInfoEx loginInfoEx)
        {
            UserLoginInfoEx existLoginInfoEx = await GetLoginInfoEx(loginInfoEx.ProviderKey);
            if (existLoginInfoEx == null)
            {
                return new OperationResult<User>(OperationResultType.Error, "无法找到相应的第三方登录信息");
            }

            LoginDto loginDto = new LoginDto() { Account = loginInfoEx.Account, Password = loginInfoEx.Password };
            OperationResult<User> loginResult = await Login(loginDto);
            if (!loginResult.Succeeded)
            {
                return loginResult;
            }

            User user = loginResult.Data;
            IdentityResult result = await CreateOrUpdateUserLogin(user, existLoginInfoEx);
            if (!result.Succeeded)
            {
                return result.ToOperationResult(user);
            }
            return new OperationResult<User>(OperationResultType.Success, "登录并绑定账号成功", user);
        }

        /// <summary>
        /// 一键创建新用户并登录
        /// </summary>
        /// <param name="cacheId">第三方登录信息缓存编号</param>
        /// <returns>业务操作结果</returns>
        public virtual async Task<OperationResult<User>> LoginOneKey(string cacheId)
        {
            UserLoginInfoEx loginInfoEx = await GetLoginInfoEx(cacheId);
            if (loginInfoEx == null)
            {
                return new OperationResult<User>(OperationResultType.Error, "无法找到相应的第三方登录信息");
            }
            IdentityResult result;
            User user = await _userManager.FindByLoginAsync(loginInfoEx.LoginProvider, loginInfoEx.ProviderKey);
            if (user == null)
            {
                user = new User()
                {
                    UserName = $"{loginInfoEx.LoginProvider}_{loginInfoEx.ProviderKey}",
                    NickName = loginInfoEx.ProviderDisplayName,
                    HeadImg = loginInfoEx.AvatarUrl
                };
                result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult(user);
                }
                UserDetail detail = new UserDetail() { RegisterIp = loginInfoEx.RegisterIp, UserId = user.Id };
                int count = await _userDetailRepository.InsertAsync(detail);
                if (count == 0)
                {
                    return new OperationResult<User>(OperationResultType.NoChanged);
                }
            }

            result = await CreateOrUpdateUserLogin(user, loginInfoEx);
            if (!result.Succeeded)
            {
                return result.ToOperationResult(user);
            }
            return new OperationResult<User>(OperationResultType.Success, "第三方用户一键登录成功", user);
        }

        private async Task<IdentityResult> CreateOrUpdateUserLogin(User user, UserLoginInfoEx loginInfoEx)
        {
            if (string.IsNullOrEmpty(user.HeadImg) && !string.IsNullOrEmpty(loginInfoEx.AvatarUrl))
            {
                user.HeadImg = loginInfoEx.AvatarUrl;
            }
            UserLogin userLogin = _userLoginRepository.GetFirst(m =>
                m.LoginProvider == loginInfoEx.LoginProvider && m.ProviderKey == loginInfoEx.ProviderKey);
            if (userLogin == null)
            {
                userLogin = new UserLogin()
                {
                    LoginProvider = loginInfoEx.LoginProvider, ProviderKey = loginInfoEx.ProviderKey,
                    ProviderDisplayName = loginInfoEx.ProviderDisplayName, Avatar = loginInfoEx.AvatarUrl, UserId = user.Id
                };
                await _userLoginRepository.InsertAsync(userLogin);
            }
            else
            {
                userLogin.UserId = user.Id;
                await _userLoginRepository.UpdateAsync(userLogin);
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// 账号退出
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> Logout(int userId)
        {
            //await _signInManager.SignOutAsync();
            //todo: Site和API的授权与退出，分别处理
            _logger.LogInformation(4, $"用户 {userId} 登出系统");

            //触发登出成功事件
            LogoutEventData logoutEventData = new LogoutEventData() { UserId = userId };
            await _eventBus.PublishAsync(logoutEventData);

            return OperationResult.Success;
        }

        /// <summary>
        /// 依次按用户名，Email，手机查找用户
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        private async Task<User> FindUserByAccount(string account)
        {
            User user = await _userManager.FindByNameAsync(account);
            if (user != null)
            {
                return user;
            }
            if (account.IsEmail())
            {
                user = await _userManager.FindByEmailAsync(account);
                if (user != null)
                {
                    return user;
                }
            }
            if (account.IsMobileNumber())
            {
                user = _userManager.Users.FirstOrDefault(m => m.PhoneNumber == account);
            }
            return user;
        }

        private OperationResult<User> ToOperationResult(SignInResult result, User user)
        {
            if (result.IsNotAllowed)
            {
                if (_userManager.Options.SignIn.RequireConfirmedEmail && !user.EmailConfirmed)
                {
                    _logger.LogWarning(2, $"用户 {user.Id} 因邮箱未验证而不允许登录");
                    return new OperationResult<User>(OperationResultType.Error, "用户邮箱未验证，请到邮箱收邮件进行确认后再登录");
                }
                if (_userManager.Options.SignIn.RequireConfirmedPhoneNumber && !user.PhoneNumberConfirmed)
                {
                    _logger.LogWarning(2, $"用户 {user.Id} 因手机号未验证而不允许登录");
                    return new OperationResult<User>(OperationResultType.Error, "用户手机号未验证，请接收手机验证码验证之后再登录");
                }
                return new OperationResult<User>(OperationResultType.Error, "用户未满足登录条件，不允许登录");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, $"用户 {user.Id} 因密码错误次数过多被锁定，解锁时间：{user.LockoutEnd}");
                return new OperationResult<User>(OperationResultType.Error,
                    $"用户因密码错误次数过多而被锁定 {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} 分钟，请稍后重试");
            }
            if (result.RequiresTwoFactor)
            {
                return new OperationResult<User>(OperationResultType.Error, "用户登录需要二元验证");
            }
            if (result.Succeeded)
            {
                return new OperationResult<User>(OperationResultType.Success, "用户登录成功", user);
            }
            return new OperationResult<User>(OperationResultType.Error,
                $"用户名或密码错误，剩余 {_userManager.Options.Lockout.MaxFailedAccessAttempts - user.AccessFailedCount} 次机会");
        }

        private async Task<UserLoginInfoEx> GetLoginInfoEx(string cacheId)
        {
            string key = $"Identity_UserLoginInfoEx_{cacheId}";
            return await _cache.GetAsync<UserLoginInfoEx>(key);
        }

        private async Task<string> SetLoginInfoEx(UserLoginInfoEx loginInfo)
        {
            string cacheId = Guid.NewGuid().ToString("N");
            string key = $"Identity_UserLoginInfoEx_{cacheId}";
            await _cache.SetAsync(key, loginInfo, 60 * 5);
            return cacheId;
        }
    }
}