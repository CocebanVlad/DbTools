using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbColumnDataNumericInfo : DbColumnDataInfo
    {
        /// <summary>
        /// Get or set precision
        /// </summary>
        public byte? Precision { get; set; }

        /// <summary>
        /// Get or set precision radix
        /// </summary>
        public short? PrecisionRadix { get; set; }

        /// <summary>
        /// Get or set scale
        /// </summary>
        public int? Scale { get; set; }
    }
}
