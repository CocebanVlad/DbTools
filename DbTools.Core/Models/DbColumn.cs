using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbColumn : DbSchemaObject
    {
        /// <summary>
        /// Get or set column owner name
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Get or set column ordinal position
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Get or set column data type
        /// </summary>
        public DbColumnDataType DataType { get; set; }

        /// <summary>
        /// Get or set column data info
        /// </summary>
        public DbColumnDataInfo DataInfo { get; set; }

        /// <summary>
        /// Get or set column default value
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Get or set column flag specifying whether its value can or cannot be null
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Get or set column collation
        /// </summary>
        public DbCollationInfo Collation { get; set; }
    }
}
