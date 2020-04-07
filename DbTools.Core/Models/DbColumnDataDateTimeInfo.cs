using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbColumnDataDateTimeInfo : DbColumnDataInfo
    {
        /// <summary>
        /// Get or set precision
        /// </summary>
        public short? Precision { get; set; }
    }
}
