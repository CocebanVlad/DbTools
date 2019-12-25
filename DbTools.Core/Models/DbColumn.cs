namespace DbTools.Core.Models
{
    public class DbColumn : DbSchemaObject
    {
        /// <summary>
        /// Get or set column table name
        /// </summary>
        public string TableName { get; internal set; }

        /// <summary>
        /// Get or set column ordinal position
        /// </summary>
        public int Position { get; internal set; }

        /// <summary>
        /// Get or set column data type
        /// </summary>
        public DbColumnDataType DataType { get; internal set; }

        /// <summary>
        /// Get or set column data info
        /// </summary>
        public DbColumnDataInfo DataInfo { get; internal set; }

        /// <summary>
        /// Get or set column default value
        /// </summary>
        public string Default { get; internal set; }

        /// <summary>
        /// Get or set column flag specifying whether its value can or cannot be null
        /// </summary>
        public bool IsNullable { get; internal set; }

        /// <summary>
        /// Get or set column collation
        /// </summary>
        public DbCollationInfo Collation { get; internal set; }
    }
}
