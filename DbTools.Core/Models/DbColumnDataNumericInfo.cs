namespace DbTools.Core.Models
{
    public class DbColumnDataNumericInfo : DbColumnDataInfo
    {
        /// <summary>
        /// Get or set precision
        /// </summary>
        public byte? Precision { get; internal set; }

        /// <summary>
        /// Get or set precision radix
        /// </summary>
        public short? PrecisionRadix { get; internal set; }

        /// <summary>
        /// Get or set scale
        /// </summary>
        public int? Scale { get; internal set; }
    }
}
