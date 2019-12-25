namespace DbTools.Core.Models
{
    public class DbTableIndexColumn : DbColumn
    {
        /// <summary>
        /// Get or set table index column order type
        /// </summary>
        public DbTableIndexColumnOrderType OrderType { get; internal set; }
    }
}
