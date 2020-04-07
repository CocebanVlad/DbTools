using DbTools.Core.Prototype;
using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public abstract class DbRoutine : DbSchemaObject, IDbDefinedObject
    {
        /// <summary>
        /// Get or set DDL script
        /// </summary>
        public string DDLScript { get; set; }
    }
}
