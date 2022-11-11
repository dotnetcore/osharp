// -----------------------------------------------------------------------
//  <copyright file="IdentityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-12 16:36</last-date>
// -----------------------------------------------------------------------

using Lazy.Captcha.Core;

using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Apis.Controllers;

[Description("网站-认证")]
[ModuleInfo(Order = 1)]
public class IdentityController : SiteApiControllerBase
{
    private readonly IServiceProvider _provider;

    public IdentityController(IServiceProvider provider)
        : base(provider)
    {
        _provider = provider;
    }

    private IIdentityContract IdentityContract => _provider.GetRequiredService<IIdentityContract>();

    private UserManager<User> UserManager => _provider.GetRequiredService<UserManager<User>>();

    private SignInManager<User> SignInManager => _provider.GetRequiredService<SignInManager<User>>();

    //private IVerifyCodeService VerifyCodeService => _provider.GetRequiredService<IVerifyCodeService>();

    /// <summary>
    /// 用户名是否存在
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns>是否存在</returns>
    [HttpGet]
    [Description("用户名是否存在")]
    public bool CheckUserNameExists(string userName)
    {
        bool exists = UserManager.Users.Any(m => m.NormalizedUserName == UserManager.NormalizeName(userName));
        return exists;
    }

    /// <summary>
    /// 用户Email是否存在
    /// </summary>
    /// <param name="email">电子邮箱</param>
    /// <returns>是否存在</returns>
    [HttpGet]
    [Description("用户Email是否存在")]
    public bool CheckEmailExists(string email)
    {
        bool exists = UserManager.Users.Any(m => m.NormalizeEmail == UserManager.NormalizeEmail(email));
        return exists;
    }

    /// <summary>
    /// 用户昵称是否存在
    /// </summary>
    /// <param name="nickName">用户昵称</param>
    /// <returns>是否存在</returns>
    [HttpGet]
    [Description("用户昵称是否存在")]
    public async Task<bool> CheckNickNameExists(string nickName)
    {
        IUserValidator<User> nickNameValidator =
            UserManager.UserValidators.FirstOrDefault(m => m.GetType() == typeof(UserNickNameValidator<User, long>));
        if (nickNameValidator == null)
        {
            return false;
        }

        IdentityResult result = await nickNameValidator.ValidateAsync(UserManager, new User() { NickName = nickName });
        return !result.Succeeded;
    }

    /// <summary>
    /// 新用户注册
    /// </summary>
    /// <param name="dto">注册信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [UnitOfWork]
    [ModuleInfo]
    [DependOnFunction("CheckUserNameExists")]
    [DependOnFunction("CheckEmailExists")]
    [DependOnFunction("CheckNickNameExists")]
    [Description("用户注册")]
    public async Task<AjaxResult> Register(RegisterDto dto)
    {
        Check.NotNull(dto, nameof(dto));

        if (!ModelState.IsValid)
        {
            return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
        }
        ICaptcha captcha = _provider.GetRequiredService<ICaptcha>();
        if (!captcha.Validate(dto.CaptchaId, dto.Captcha))
        {
            return new AjaxResult("验证码错误，请刷新重试", AjaxResultType.Error);
        }

        dto.UserName ??= dto.Email;
        dto.NickName ??= $"User_{Random.NextLetterAndNumberString(8)}"; //随机用户昵称
        dto.RegisterIp = HttpContext.GetClientIp();

        OperationResult<User> result = await IdentityContract.Register(dto);

        if (result.Succeeded)
        {
            User user = result.Data;
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = UrlBase64ReplaceChar(code);
            string url = $"{Request.Scheme}://{Request.Host}/#/passport/confirm-email?userId={user.Id}&code={code}";
            string body =
                $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，您好！<br>"
                + $"欢迎注册，激活邮箱请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                + $"{url}<br>"
                + $"祝您使用愉快！";
            await SendMailAsync(user.Email, "柳柳软件 注册邮箱激活邮件", body);

            return result.ToAjaxResult(m => new { m.UserName, m.NickName, m.Email });
        }
        return result.ToAjaxResult();
    }

    /// <summary>
    /// 获取身份认证Token
    /// </summary>
    /// <param name="dto">TokenDto</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [Description("Token")]
    public async Task<AjaxResult> Token(TokenDto dto)
    {
        string grantType = dto.GrantType?.UpperToLowerAndSplit("_");
        if (grantType == GrantType.Password)
        {
            Check.NotNull(dto.Account, nameof(dto.Account));
            Check.NotNull(dto.Password, nameof(dto.Password));

            LoginDto loginDto = new LoginDto()
            {
                Account = dto.Account,
                Password = dto.Password,
                ClientType = dto.ClientType,
                IsToken = true,
                Ip = HttpContext.GetClientIp(),
                UserAgent = Request.Headers["User-Agent"].FirstOrDefault()
            };

            IUnitOfWork unitOfWork = HttpContext.RequestServices.GetUnitOfWork(true);
            OperationResult<User> result = await IdentityContract.Login(loginDto);
            if (!result.Succeeded)
            {
                await unitOfWork.CommitAsync();
                return result.ToAjaxResult();
            }

            User user = result.Data;
            JsonWebToken token = await CreateJwtToken(user, dto.ClientType);
            await unitOfWork.CommitAsync();
            return new AjaxResult("登录成功", AjaxResultType.Success, token);
        }

        if (grantType == GrantType.RefreshToken)
        {
            Check.NotNull(dto.RefreshToken, nameof(dto.RefreshToken));
            JsonWebToken token = await CreateJwtToken(dto.RefreshToken);
            return new AjaxResult("刷新成功", AjaxResultType.Success, token);
        }

        return new AjaxResult("GrantType错误", AjaxResultType.Error);
    }

    [HttpPost]
    [ModuleInfo]
    [Description("用户登录")]
    public async Task<AjaxResult> Login(LoginDto dto)
    {
        Check.NotNull(dto, nameof(dto));

        if (!ModelState.IsValid)
        {
            return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
        }

        //todo: 校验验证码
        dto.Ip = HttpContext.GetClientIp();
        dto.UserAgent = Request.Headers["User-Agent"].FirstOrDefault();

        IUnitOfWork unitOfWork = HttpContext.RequestServices.GetUnitOfWork(true);
        OperationResult<User> result = await IdentityContract.Login(dto);

        if (!result.Succeeded)
        {
            return result.ToAjaxResult();
        }

        User user = result.Data;
        await SignInManager.SignInAsync(user, dto.Remember);
        await unitOfWork.CommitAsync();
        return new AjaxResult("登录成功");
    }

    /// <summary>
    /// 用户登出
    /// </summary>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [UnitOfWork]
    [Description("用户登出")]
    public async Task<AjaxResult> Logout()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return new AjaxResult("用户登出成功");
        }

        long userId = User.Identity.GetUserId<long>();
        bool isToken = Request.Headers["Authorization"].Any(m => m.StartsWith("Bearer"));

        OperationResult result = await IdentityContract.Logout(userId, isToken);
        return result.ToAjaxResult();
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ModuleInfo]
    [Description("用户信息")]
    public async Task<OnlineUser> Profile()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return new OnlineUser();
        }

        IOnlineUserProvider onlineUserProvider = HttpContext.RequestServices.GetService<IOnlineUserProvider>();
        if (onlineUserProvider == null)
        {
            return new OnlineUser();
        }

        OnlineUser onlineUser = await onlineUserProvider.GetOrCreate(User.Identity.Name);
        onlineUser.RefreshTokens.Clear();
        onlineUser.ExtendData.Clear();
        return onlineUser;
    }

    /// <summary>
    /// 激活邮箱
    /// </summary>
    /// <param name="dto">电子邮箱</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [UnitOfWork]
    [Description("激活邮箱")]
    public async Task<AjaxResult> ConfirmEmail(ConfirmEmailDto dto)
    {
        if (!ModelState.IsValid)
        {
            return new AjaxResult("邮箱激活失败：参数不正确", AjaxResultType.Error);
        }

        User user = await UserManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null)
        {
            return new AjaxResult("注册邮箱激活失败：用户不存在", AjaxResultType.Error);
        }

        if (user.EmailConfirmed)
        {
            return new AjaxResult("注册邮箱已激活，操作取消", AjaxResultType.Info);
        }

        string code = UrlBase64ReplaceChar(dto.Code);
        IdentityResult result = await UserManager.ConfirmEmailAsync(user, code);
        return result.ToOperationResult().ToAjaxResult();
    }

    /// <summary>
    /// 发送激活注册邮件
    /// </summary>
    /// <param name="dto">激活邮箱信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction("CheckEmailExists")]
    [Description("发送激活注册邮件")]
    public async Task<AjaxResult> SendConfirmMail(SendMailDto dto)
    {
        if (!ModelState.IsValid)
        {
            return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
        }

        //if (!VerifyCodeService.CheckCode(dto.VerifyCode, dto.VerifyCodeId))
        //{
        //    return new AjaxResult("验证码错误，请刷新重试", AjaxResultType.Error);
        //}

        User user = await UserManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return new AjaxResult("发送激活邮件失败：用户不存在", AjaxResultType.Error);
        }

        if (user.EmailConfirmed)
        {
            return new AjaxResult("Email已激活，无需再次激活", AjaxResultType.Info);
        }

        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = UrlBase64ReplaceChar(code);
        string url = $"{Request.Scheme}://{Request.Host}/#/passport/confirm-email?userId={user.Id}&code={code}";
        string body =
            $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，你好！<br>"
            + $"欢迎注册，激活邮箱请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
            + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
            + $"{url}<br>"
            + $"祝您使用愉快！";
        await SendMailAsync(user.Email, "柳柳软件 注册邮箱激活邮件", body);
        return new AjaxResult("激活Email邮件发送成功");
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="dto">修改密码信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [LoggedIn]
    [ModuleInfo]
    [UnitOfWork]
    [Description("修改密码")]
    public async Task<AjaxResult> ChangePassword(ChangePasswordDto dto)
    {
        Check.NotNull(dto, nameof(dto));

        long userId = User.Identity.GetUserId<long>();
        User user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return new AjaxResult($"用户不存在", AjaxResultType.Error);
        }

        IdentityResult result = string.IsNullOrEmpty(dto.OldPassword)
            ? await UserManager.AddPasswordAsync(user, dto.NewPassword)
            : await UserManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        return result.ToOperationResult().ToAjaxResult();
    }

    /// <summary>
    /// 发送重置密码邮件
    /// </summary>
    /// <param name="dto">发送邮件信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction("CheckEmailExists")]
    [Description("发送重置密码邮件")]
    public async Task<AjaxResult> SendResetPasswordMail(SendMailDto dto)
    {
        if (!ModelState.IsValid)
        {
            return new AjaxResult("提交数据验证失败", AjaxResultType.Error);
        }

        //if (!VerifyCodeService.CheckCode(dto.VerifyCode, dto.VerifyCodeId))
        //{
        //    return new AjaxResult("验证码错误，请刷新重试", AjaxResultType.Error);
        //}

        User user = await UserManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return new AjaxResult("用户不存在", AjaxResultType.Error);
        }

        string token = await UserManager.GeneratePasswordResetTokenAsync(user);
        token = UrlBase64ReplaceChar(token);
        IEmailSender sender = HttpContext.RequestServices.GetService<IEmailSender>();
        string url = $"{Request.Scheme}://{Request.Host}/#/passport/reset-password?userId={user.Id}&token={token}";
        string body = $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，您好！<br>"
            + $"欢迎使用柳柳软件账户密码重置功能，请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
            + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
            + $"{url}<br>"
            + $"祝您使用愉快！";
        await sender.SendEmailAsync(user.Email, "柳柳软件 重置密码邮件", body);
        return new AjaxResult("密码重置邮件发送成功");
    }

    /// <summary>
    /// 重置登录密码
    /// </summary>
    /// <param name="dto">重置密码信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [UnitOfWork]
    [Description("重置登录密码")]
    public async Task<AjaxResult> ResetPassword(ResetPasswordDto dto)
    {
        Check.NotNull(dto, nameof(dto));

        User user = await UserManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null)
        {
            return new AjaxResult($"用户不存在", AjaxResultType.Error);
        }

        string token = UrlBase64ReplaceChar(dto.Token);
        IdentityResult result = await UserManager.ResetPasswordAsync(user, token, dto.NewPassword);

        return result.ToOperationResult().ToAjaxResult();
    }

    #region 私有方法

    private async Task<JsonWebToken> CreateJwtToken(User user, RequestClientType clientType = RequestClientType.Browser)
    {
        IServiceProvider provider = HttpContext.RequestServices;
        IJwtBearerService jwtBearerService = provider.GetRequiredService<IJwtBearerService>();
        JsonWebToken token = await jwtBearerService.CreateToken(user.Id.ToString(), user.UserName, clientType);

        return token;
    }

    private async Task<JsonWebToken> CreateJwtToken(string refreshToken)
    {
        IServiceProvider provider = HttpContext.RequestServices;
        IJwtBearerService jwtBearerService = provider.GetRequiredService<IJwtBearerService>();
        JsonWebToken token = await jwtBearerService.RefreshToken(refreshToken);
        return token;
    }

    private async Task SendMailAsync(string email, string subject, string body)
    {
        IEmailSender sender = HttpContext.RequestServices.GetRequiredService<IEmailSender>();
        await sender.SendEmailAsync(email, subject, body);
    }

    private static string UrlBase64ReplaceChar(string source)
    {
        if (source.Contains('+') || source.Contains('/'))
        {
            return source.Replace('+', '-').Replace('/', '_');
        }
        if (source.Contains('-') || source.Contains('_'))
        {
            return source.Replace('-', '+').Replace('_', '/');
        }
        return source;
    }

    #endregion
}
