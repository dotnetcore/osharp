using System.Collections.Generic;


namespace OSharp.Hosting.Infos.Dtos
{
    public partial class MessageInputDto
    {
        /// <summary>
        /// 获取或设置 公共消息的接收角色编号集合
        /// </summary>
        public ICollection<int> PublicRoleIds { get; set; }

        /// <summary>
        /// 获取或设置 私人消息的接收者编号集合
        /// </summary>
        public ICollection<int> RecipientIds { get; set; }
    }
}
