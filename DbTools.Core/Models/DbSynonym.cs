using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbSynonym : DbSchemaObject
    {
        /// <summary>
        /// Get or set synonym target object
        /// </summary>
        public DbSchemaObject TargetObject { get; set; }
    }
}
