using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbTableIndex : DbSchemaObject
    {
        /// <summary>
        /// Get or set table index type
        /// </summary>
        public DbTableIndexType Type { get; set; }

        /// <summary>
        /// Get or set table index flag specifying whether it is unique or not
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Get or set table index columns
        /// </summary>
        public DbObjectCollection<DbTableIndexColumn> IndexColumns { get; set; }
    }
}
