using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public abstract class DbObject
    {
        /// <summary>
        /// Get or set object catalog name (database name)
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        /// Get or set object name
        /// </summary>
        public string Name { get; set; }
    }
}
