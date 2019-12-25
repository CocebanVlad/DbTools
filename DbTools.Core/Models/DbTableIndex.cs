namespace DbTools.Core.Models
{
    public class DbTableIndex : DbSchemaObject
    {
        /// <summary>
        /// Get or set table index type
        /// </summary>
        public DbTableIndexType Type { get; internal set; }

        /// <summary>
        /// Get or set table index flag specifying whether it is unique or not
        /// </summary>
        public bool IsUnique { get; internal set; }

        /// <summary>
        /// Get or set table index columns collection
        /// </summary>
        public DbObjectCollection<DbTableIndexColumn> IndexColumns { get; internal set; }
    }
}
