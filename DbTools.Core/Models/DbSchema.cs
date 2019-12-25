namespace DbTools.Core.Models
{
    public class DbSchema : DbObject
    {
        /// <summary>
        /// Get or set schema tables collection
        /// </summary>
        public DbObjectCollection<DbTable> Tables { get; internal set; }
    }
}
