namespace DbTools.Core.Models
{
    public abstract class DbObject
    {
        /// <summary>
        /// Get or set object catalog name (database name)
        /// </summary>
        public string CatalogName { get; internal set; }

        /// <summary>
        /// Get or set object name
        /// </summary>
        public string Name { get; internal set; }
    }
}
