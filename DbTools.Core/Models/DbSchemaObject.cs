namespace DbTools.Core.Models
{
    public abstract class DbSchemaObject : DbObject
    {
        /// <summary>
        /// Get or set object name
        /// </summary>
        public string SchemaName { get; internal set; }
    }
}
