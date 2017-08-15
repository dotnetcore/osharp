namespace OSharp.Filter
{
    /// <summary>
    /// 指定可用于表数据存在性检查类型的值
    /// </summary>
    public enum CheckExistsType
    {
        /// <summary>
        ///   插入数据时重复性检查
        /// </summary>
        Insert,

        /// <summary>
        ///   编辑数据时重复性检查
        /// </summary>
        Update
    }
}
