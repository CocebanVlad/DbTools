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

        /// <summary>
        /// Get object full name
        /// </summary>
        /// <returns>A string formatted as "[{schema_name}].[{object_name}]"</returns>
        public string GetFullName()
        {
            return "[" + this.SchemaName + "].[" + this.Name + "]";
        }
    }
}
