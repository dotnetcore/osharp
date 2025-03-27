// -----------------------------------------------------------------------
//  <copyright file="UserSeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-06 21:56</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Identity;

public class UserSeedDataInitializer : SeedDataInitializerBase<User, long>
{
    private readonly IServiceProvider _rootProvider;

    /// <summary>
    /// 初始化一个<see cref="SeedDataInitializerBase{TEntity, TKey}"/>类型的新实例
    /// </summary>
    public UserSeedDataInitializer(IServiceProvider rootProvider)
        : base(rootProvider)
    {
        _rootProvider = rootProvider;
    }

    private static string PassWord = "osharp123456";

    /// <summary>
    /// 重写以提供要初始化的种子数据
    /// </summary>
    /// <returns></returns>
    protected override User[] SeedData(IServiceProvider provider)
    {
        return new[]
        {
            new User() { Id = 10000, AccessFailedCount=0,Email = "i66soft@qq.com", EmailConfirmed=true, IsSystem=true, NickName="大站长", NormalizedUserName="ADMIN",UserName="admin"}
        };
    }

    /// <summary>
    /// 重写以提供判断某个实体是否存在的表达式
    /// </summary>
    /// <param name="entity">要判断的实体</param>
    /// <returns></returns>
    protected override Expression<Func<User, bool>> ExistingExpression(User entity)
    {
        return m => m.NormalizedUserName == entity.NormalizedUserName;
    }

    /// <summary>
    /// 将种子数据初始化到数据库
    /// </summary>
    protected override void SyncToDatabase(User[] entities, IServiceProvider provider)
    {
        if (entities.Length == 0)
        {
            return;
        }

        IUnitOfWork unitOfWork = provider.GetUnitOfWork(true);
        UserManager<User> userManager = provider.GetRequiredService<UserManager<User>>();
        IRepository<UserDetail,long> userDetailRepository = provider.GetRequiredService<IRepository<UserDetail,long>>();
        foreach (User user in entities)
        {
            if (userManager.Users.Any(ExistingExpression(user)))
            {
                continue;
            }
            IdentityResult result = userManager.CreateAsync(user, PassWord).Result;
            if (!result.Succeeded)
            {
                throw new OsharpException($"进行角色种子数据“{user.NickName}”同步时出错：{result.ErrorMessage()}");
            }
            var userDetail = new UserDetail() {  Id=10000, RegisterIp="127.0.0.1", UserId=10000};
            var count = userDetailRepository.Insert(userDetail);
            if (count == 0)
            {
                throw new OsharpException($"进行角色种子数据“{user.NickName}”同步时出错：用户扩展信息写入失败");
            }
        }
        unitOfWork.Commit();
    }
}
