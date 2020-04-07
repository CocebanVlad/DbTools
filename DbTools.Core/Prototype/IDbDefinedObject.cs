namespace DbTools.Core.Prototype
{
    public interface IDbDefinedObject
    {
        /// <summary>
        /// Get DDL script
        /// </summary>
        string DDLScript { get; set; }
    }
}
