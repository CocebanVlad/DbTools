namespace DbTools.Core.Models
{
    public class DbCollationInfo
    {
        /// <summary>
        /// Get or set name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Get or set catalog name
        /// </summary>
        public string CatalogName { get; internal set; }

        /// <summary>
        /// Get or set schema name
        /// </summary>
        public string SchemaName { get; internal set; }
    }
}
