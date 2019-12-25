namespace DbTools.Core.Models
{
    public class DbTable : DbSchemaObject
    {
        /// <summary>
        /// Get or set table columns collection
        /// </summary>
        public DbObjectCollection<DbColumn> Columns { get; internal set; }
    }
}
