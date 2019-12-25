namespace DbTools.Core.Models
{
    public class DbColumnDataCharacterInfo : DbColumnDataInfo
    {
        /// <summary>
        /// Get or set max length
        /// </summary>
        public int MaxLength { get; internal set; }

        /// <summary>
        /// Get or set octet length
        /// </summary>
        public int OctetLength { get; internal set; }

        /// <summary>
        /// Get or set name of the character set
        /// </summary>
        public string CharacterSetName { get; internal set; }

        /// <summary>
        /// Get or set catalog name of the character set
        /// </summary>
        public string CharacterSetCatalogName { get; internal set; }

        /// <summary>
        /// Get or set schema name of the character set
        /// </summary>
        public string CharacterSetSchemaName { get; internal set; }
    }
}
