using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSharp.Demo.WebApi.Areas.Admin.ViewModels
{
    public class TreeNode
    {
        public TreeNode()
        {
            Items = new List<TreeNode>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; }

        public double OrderCode { get; set; }

        public string Remark { get; set; }

        public string IconCls { get; set; }

        public string Url { get; set; }

        public bool HasChildren { get; set; }

        public object Data { get; set; }

        public List<TreeNode> Items { get; }
    }
}
