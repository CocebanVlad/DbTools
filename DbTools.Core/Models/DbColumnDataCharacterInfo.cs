using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbColumnDataCharacterInfo : DbColumnDataInfo
    {
        /// <summary>
        /// Get or set max length
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Get or set octet length
        /// </summary>
        public int OctetLength { get; set; }

        /// <summary>
        /// Get or set name of the character set
        /// </summary>
        public string CharacterSetName { get; set; }

        /// <summary>
        /// Get or set catalog name of the character set
        /// </summary>
        public string CharacterSetCatalogName { get; set; }

        /// <summary>
        /// Get or set schema name of the character set
        /// </summary>
        public string CharacterSetSchemaName { get; set; }
    }
}
