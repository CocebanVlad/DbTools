namespace DbTools.Core.Models
{
    public enum DbColumnDataType
    {
        #region Exact numerics
        BIGINT,
        NUMERIC,
        BIT,
        SMALLINT,
        DECIMAL,
        SMALLMONEY,
        INT,
        TINYINT,
        MONEY,
        #endregion
        #region Approximate numerics
        FLOAT,
        REAL,
        #endregion
        #region Date and time
        DATE,
        DATETIMEOFFSET,
        DATETIME2,
        SMALLDATETIME,
        DATETIME,
        TIME,
        #endregion
        #region Character strings
        CHAR,
        VARCHAR,
        TEXT,
        #endregion
        #region Unicode character strings
        NCHAR,
        NVARCHAR,
        NTEXT,
        #endregion
        #region Binary strings
        BINARY,
        VARBINARY,
        IMAGE,
        #endregion
        #region Other data types
        HIERARCHYID,
        UNIQUEIDENTIFIER,
        SQL_VARIANT,
        XML
        #endregion
    }
}
