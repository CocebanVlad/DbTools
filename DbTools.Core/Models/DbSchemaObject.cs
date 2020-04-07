using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public abstract class DbSchemaObject : DbObject
    {
        /// <summary>
        /// Get or set object name
        /// </summary>
        public string SchemaName { get; set; }
    }
}
