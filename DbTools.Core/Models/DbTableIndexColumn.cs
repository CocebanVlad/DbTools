using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbTableIndexColumn : DbColumn
    {
        /// <summary>
        /// Get or set table index column order type
        /// </summary>
        public DbTableIndexColumnOrderType OrderType { get; set; }
    }
}
