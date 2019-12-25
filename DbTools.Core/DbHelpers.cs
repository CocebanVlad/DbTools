using DbTools.Core.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DbTools.Core
{
    public static class DbHelpers
    {
        /// <summary>
        /// Fill table with data returned from command execution
        /// </summary>
        /// <param name="table">Table to fill</param>
        /// <param name="cmd">SQL command</param>
        /// <returns>Data table</returns>
        public static void FillTable(DataTable table, SqlCommand cmd)
        {
            using (var adapter = new SqlDataAdapter(cmd))
                adapter.Fill(table);
        }

        /// <summary>
        /// Assert log message
        /// </summary>
        /// <param name="assertion">Flag specifying weather to log or not the message</param>
        /// <param name="message">Message</param>
        public static void AssertLog(bool assertion, string message)
        {
            if (assertion)
                Console.WriteLine(message);
        }

        /// <summary>
        /// Generate command for getting all schemas
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetSchemasCommand(SqlConnection conn)
        {
            var cmd = new SqlCommand(@"
                SELECT sch.[name] AS [Name] FROM [sys].[schemas] AS sch
                ", conn);

            return cmd;
        }

        /// <summary>
        /// Generate command for getting all tables
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetTablesCommand(SqlConnection conn, string schemaName)
        {
            var cmd = new SqlCommand(@"
                SELECT tbl.[name] AS [Name] FROM [sys].[tables] AS tbl
                LEFT JOIN [sys].[schemas] AS sch ON sch.[schema_id] = tbl.[schema_id]
                WHERE sch.[name] LIKE @schemaName
                ", conn);
            cmd.Parameters.AddWithValue("schemaName", schemaName);

            return cmd;
        }

        /// <summary>
        /// Generate command for getting all columns of a specific table
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <param name="tableName">Table name</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetColumnsCommand(SqlConnection conn, string schemaName, string tableName)
        {
            var cmd = new SqlCommand(@"
                SELECT col.[table_schema] AS [SchemaName],
                       col.[table_name] AS [TableName],
                       col.[column_name] AS [Name],
                       col.[ordinal_position] AS [OrdinalPosition],
                       col.[column_default] AS [Default],
                       col.[is_nullable] AS [IsNullable],
                       col.[data_type] AS [DataType],
                       col.[character_maximum_length] AS [CharacterMaximumLength],
                       col.[character_octet_length] AS [CharacterOctetLength],
                       col.[character_set_name] AS [CharacterSetName],
                       col.[character_set_catalog] AS [CharacterSetCatalogName],
                       col.[character_set_schema] AS [CharacterSetSchemaName],
                       col.[numeric_precision] AS [NumericPrecision],
                       col.[numeric_precision_radix] AS [NumericPrecisionRadix],
                       col.[numeric_scale] AS [NumericScale],
                       col.[datetime_precision] AS [DatetimePrecision],
                       col.[collation_name] AS [CollationName],
                       col.[collation_catalog] AS [CollationCatalogName],
                       col.[collation_schema] AS [CollationSchemaName]
                FROM INFORMATION_SCHEMA.COLUMNS AS col
                WHERE col.[table_schema] = @schemaName AND col.[table_name] = @tableName
                ", conn);
            cmd.Parameters.AddWithValue("schemaName", schemaName);
            cmd.Parameters.AddWithValue("tableName", tableName);

            return cmd;
        }

        /// <summary>
        /// Get schema
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <param name="verbose">Flag specifying whether to provide additional details regarding the execution or not</param>
        /// <returns>Schema object</returns>
        public static DbSchema GetSchema(SqlConnection conn, string schemaName, bool verbose = false)
        {
            if (conn == null)
                throw new ArgumentNullException(nameof(conn));
            if (string.IsNullOrEmpty(schemaName))
                throw new ArgumentNullException(nameof(schemaName));

            var catalogName = conn.Database;
            var schema = new DbSchema();

            // STEP 1. Assign general info
            schema.Name = schemaName;
            schema.CatalogName = conn.Database;
            schema.Tables = new DbObjectCollection<DbTable>();

            // STEP 2. Fetch tables
            var schemaTables = new DataTable("SchemaTables");
            FillTable(schemaTables,
                GenerateGetTablesCommand(conn, schemaName));
            foreach (DataRow tableData in schemaTables.Rows)
            {
                var table = new DbTable();
                FillDbTable(conn, table, catalogName, schemaName, tableData, verbose);
                schema.Tables.Add(table.Name, table);
            }

            return schema;
        }

        /// <summary>
        /// Fill table object with data
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="table">Table to fill</param>
        /// <param name="catalogName">Catalog name</param>
        /// <param name="schemaName">Schema name</param>
        /// <param name="row">Row</param>
        /// <param name="verbose">Flag specifying whether to provide additional details regarding the execution or not</param>
        private static void FillDbTable(
            SqlConnection conn,
            DbTable table,
            string catalogName,
            string schemaName,
            DataRow row,
            bool verbose = false
            )
        {
            // STEP 1. Assign general info
            table.Name = row.GetStringValue("Name");
            table.CatalogName = catalogName;
            table.SchemaName = schemaName;
            table.Columns = new DbObjectCollection<DbColumn>();

            // STEP 2. Fetch columns
            var tableColumns = new DataTable("TableColumns");
            FillTable(tableColumns,
                GenerateGetColumnsCommand(conn, schemaName, table.Name));
            foreach (DataRow columnData in tableColumns.Rows)
            {
                var column = new DbColumn();
                FillDbColumn(conn, column, catalogName, schemaName, table.Name, columnData, verbose);
                table.Columns.Add(column.Name, column);
            }
        }

        /// <summary>
        /// Fill column object with data
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="column">Column to fill</param>
        /// <param name="catalogName">Catalog name</param>
        /// <param name="schemaName">Schema name</param>
        /// <param name="tableName">Table name</param>
        /// <param name="row">Row</param>
        /// <param name="verbose">Flag specifying whether to provide additional details regarding the execution or not</param>
        private static void FillDbColumn(
            SqlConnection conn,
            DbColumn column,
            string catalogName,
            string schemaName,
            string tableName,
            DataRow row,
            bool verbose = false
            )
        {
            // STEP 1. Assign general info
            column.Name = row.GetStringValue("Name");
            column.CatalogName = catalogName;
            column.SchemaName = schemaName;
            column.TableName = tableName;
            column.Position = row.GetIntValue("OrdinalPosition");
            column.Default = row.GetStringValue("Default");
            column.IsNullable = row.GetStringValue("IsNullable") != "NO";

            // STEP 2. Identify data type
            var dataTypeStr = row.GetStringValue("DataType");
            DbColumnDataType dataType;
            if (!Enum.TryParse(dataTypeStr, true, out dataType))
                throw new Exception("Unsupported data type \"" + dataTypeStr + "\"");

            column.DataType = dataType;

            // STEP 3. Collect data info
            DbColumnDataInfo dataInfo = null;
            #region Collect data info
            switch (dataType)
            {
                case DbColumnDataType.CHAR:
                case DbColumnDataType.VARCHAR:
                case DbColumnDataType.TEXT:
                case DbColumnDataType.NCHAR:
                case DbColumnDataType.NVARCHAR:
                case DbColumnDataType.NTEXT:
                    var charDataInfo =
                        new DbColumnDataCharacterInfo();
                    charDataInfo.MaxLength =
                            row.GetIntValue("CharacterMaximumLength");
                    charDataInfo.OctetLength =
                        row.GetIntValue("CharacterOctetLength");
                    charDataInfo.CharacterSetName =
                        row.GetStringValue("CharacterSetName");
                    charDataInfo.CharacterSetCatalogName =
                        row.GetStringValue("CharacterSetCatalogName");
                    charDataInfo.CharacterSetSchemaName =
                        row.GetStringValue("CharacterSetSchemaName");

                    dataInfo = charDataInfo;
                    break;
                case DbColumnDataType.BIGINT:
                case DbColumnDataType.NUMERIC:
                case DbColumnDataType.BIT:
                case DbColumnDataType.SMALLINT:
                case DbColumnDataType.DECIMAL:
                case DbColumnDataType.SMALLMONEY:
                case DbColumnDataType.INT:
                case DbColumnDataType.TINYINT:
                case DbColumnDataType.MONEY:
                case DbColumnDataType.FLOAT:
                case DbColumnDataType.REAL:
                    var numDataInfo =
                        new DbColumnDataNumericInfo();
                    numDataInfo.Precision =
                        row.GetNullableValue<byte?>("NumericPrecision", null);
                    numDataInfo.PrecisionRadix =
                        row.GetNullableValue<short?>("NumericPrecisionRadix", null);
                    numDataInfo.Scale =
                        row.GetNullableValue<int?>("NumericScale", null);

                    dataInfo = numDataInfo;
                    break;
                case DbColumnDataType.DATE:
                case DbColumnDataType.DATETIMEOFFSET:
                case DbColumnDataType.DATETIME2:
                case DbColumnDataType.SMALLDATETIME:
                case DbColumnDataType.DATETIME:
                case DbColumnDataType.TIME:
                    var dateTimeDataInfo =
                        new DbColumnDataDateTimeInfo();
                    dateTimeDataInfo.Precision =
                        row.GetValue<short>("DatetimePrecision");

                    dataInfo = dateTimeDataInfo;
                    break;
            }
            #endregion

            column.DataInfo = dataInfo;

            // STEP 4. Collect collation info
            var collationInfo = new DbCollationInfo();
            collationInfo.Name =
                row.GetStringValue("CollationName");
            collationInfo.CatalogName =
                row.GetStringValue("CollationCatalogName");
            collationInfo.SchemaName =
                row.GetStringValue("CollationSchemaName");

            column.Collation = collationInfo;
        }
    }
}
