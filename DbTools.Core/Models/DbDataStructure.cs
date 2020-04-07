using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public abstract class DbDataStructure : DbSchemaObject
    {
        /// <summary>
        /// Get or set columns
        /// </summary>
        public DbObjectCollection<DbColumn> Columns { get; set; }
    }
}
