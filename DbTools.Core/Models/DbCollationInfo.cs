using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbCollationInfo
    {
        /// <summary>
        /// Get or set name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set catalog name
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        /// Get or set schema name
        /// </summary>
        public string SchemaName { get; set; }
    }
}
