using System.Collections.Generic;


namespace OSharp.Demo.Common.Dtos
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
