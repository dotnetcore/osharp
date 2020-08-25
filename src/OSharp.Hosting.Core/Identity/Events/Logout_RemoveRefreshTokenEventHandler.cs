using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Hosting.Identity.Entities;

using Microsoft.AspNetCore.Identity;

using OSharp.EventBuses;
using OSharp.Identity;


namespace OSharp.Hosting.Identity.Events
{
    public class Logout_RemoveRefreshTokenEventHandler : EventHandlerBase<LogoutEventData>
    {
        private readonly UserManager<User> _userManager;
        private readonly IPrincipal _principal;

        /// <summary>
        /// 初始化一个<see cref="Logout_RemoveRefreshTokenEventHandler"/>类型的新实例
        /// </summary>
        public Logout_RemoveRefreshTokenEventHandler(UserManager<User>userManager, IPrincipal principal)
        {
            _userManager = userManager;
            _principal = principal;
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(LogoutEventData eventData)
        {
            HandleAsync(eventData).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns>是否成功</returns>
        public override async Task HandleAsync(LogoutEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            ClaimsIdentity identity = _principal.Identity as ClaimsIdentity;
            if (identity?.IsAuthenticated != true)
            {
                return;
            }

            string clientId = identity.GetClaimValueFirstOrDefault("clientId");
            if (clientId == null)
            {
                return;
            }

            await _userManager.RemoveRefreshToken(eventData.UserId.ToString(), clientId);
        }
    }
}
