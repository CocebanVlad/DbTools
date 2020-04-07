using DbTools.Core.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace DbTools.Core
{
    /*
     * https://docs.microsoft.com/en-us/sql/relational-databases/system-compatibility-views/sys-sysobjects-transact-sql?redirectedfrom=MSDN&view=sql-server-ver15
     * Object type. Can be one of the following values:
     * ---------------
     *  AF   ggregate function (CLR)
     *  C    CHECK constraint
     *  D    Default or DEFAULT constraint
     *  F    FOREIGN KEY constraint
     *  FN   Scalar function
     *  FS   Assembly (CLR) scalar-function
     *  FT   Assembly (CLR) table-valued functionIF = In-lined table-function
     *  IT   Internal table
     *  K    PRIMARY KEY or UNIQUE constraint
     *  L    Log
     *  P    Stored procedure
     *  PC   Assembly (CLR) stored-procedure
     *  R    Rule
     *  RF   Replication filter stored procedure
     *  S    System table
     *  SN   Synonym
     *  SQ   Service queue
     *  TA   Assembly (CLR) DML trigger
     *  TF   Table function
     *  TR   SQL DML Trigger
     *  TT   Table type
     *  U    User table
     *  V    View
     *  X    Extended stored procedure
     */
    public static class DbHelpers
    {
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
        /// Decode DB object name into parts ([schema_name], [object_name])
        /// </summary>
        /// <param name="name">Name to decode</param>
        /// <returns>Name parts</returns>
        public static string[] DecodeDbObjectName(string name)
        {
            return name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimStart(new char[] { ' ', '[' }).TrimEnd(new char[] { ']', ' ' })).ToArray();
        }

        #region Methods for generating SQL commands
        /// <summary>
        /// Generate command for getting all schemas
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetSchemasCommand(SqlConnection conn)
        {
            var cmd = new SqlCommand(@"
                SELECT _schema.[name] AS [Name] FROM [sys].[schemas] AS _schema
                ", conn);

            return cmd;
        }

        /// <summary>
        /// Generate command for getting concrete objects
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <param name="type">Object type</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetConcreteObjectsCommand(SqlConnection conn, string schemaName, params string[] type)
        {
            var cmd = new SqlCommand();

            var strBldr = new StringBuilder();
            strBldr.Append(@"
                SELECT _obj.[name] AS [Name], _module.[definition] AS [Definition] FROM [sys].[objects] AS _obj
                LEFT JOIN [sys].[schemas] AS _schema ON _schema.[schema_id] = _obj.[schema_id]
                LEFT JOIN [sys].[sql_modules] AS _module ON _module.[object_id] = _obj.[object_id]
                WHERE _schema.[name] LIKE @schemaName
                ");
            #region Append type constraint
            if (type.Length > 0)
            {
                strBldr.Append(" AND _obj.[type] IN (");

                for (var i = 0; i < type.Length; i++)
                {
                    if (i > 0)
                    {
                        strBldr.Append(", ");
                    }

                    strBldr.Append($"@{cmd.Parameters.AddWithValue($"__param{i}", type[i]).ParameterName}");
                }

                strBldr.Append(")");
            }
            #endregion

            cmd.Connection = conn;
            cmd.CommandText = strBldr.ToString();
            cmd.Parameters.AddWithValue("schemaName", schemaName);

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
            return GenerateGetConcreteObjectsCommand(conn, schemaName, "U");
        }

        /// <summary>
        /// Generate command for getting all views
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetViewsCommand(SqlConnection conn, string schemaName)
        {
            return GenerateGetConcreteObjectsCommand(conn, schemaName, "V");
        }

        /// <summary>
        /// Generate command for getting all functions
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetFunctionsCommand(SqlConnection conn, string schemaName)
        {
            return GenerateGetConcreteObjectsCommand(conn, schemaName, "FN", "FS", "FT", "IF", "TF");
        }

        /// <summary>
        /// Generate command for getting all procedures
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetProceduresCommand(SqlConnection conn, string schemaName)
        {
            return GenerateGetConcreteObjectsCommand(conn, schemaName, "P", "PC", "RF", "X");
        }

        /// <summary>
        /// Generate command for getting all columns of a specific view or table
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <param name="objectName">Object name (VIEW or TABLE)</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetColumnsCommand(SqlConnection conn, string schemaName, string objectName)
        {
            var cmd = new SqlCommand(@"
                SELECT _column.[table_schema] AS [SchemaName],
                       _column.[table_name] AS [TableName],
                       _column.[column_name] AS [Name],
                       _column.[ordinal_position] AS [OrdinalPosition],
                       _column.[column_default] AS [Default],
                       _column.[is_nullable] AS [IsNullable],
                       _column.[data_type] AS [DataType],
                       _column.[character_maximum_length] AS [CharacterMaximumLength],
                       _column.[character_octet_length] AS [CharacterOctetLength],
                       _column.[character_set_name] AS [CharacterSetName],
                       _column.[character_set_catalog] AS [CharacterSetCatalogName],
                       _column.[character_set_schema] AS [CharacterSetSchemaName],
                       _column.[numeric_precision] AS [NumericPrecision],
                       _column.[numeric_precision_radix] AS [NumericPrecisionRadix],
                       _column.[numeric_scale] AS [NumericScale],
                       _column.[datetime_precision] AS [DatetimePrecision],
                       _column.[collation_name] AS [CollationName],
                       _column.[collation_catalog] AS [CollationCatalogName],
                       _column.[collation_schema] AS [CollationSchemaName]
                FROM [INFORMATION_SCHEMA].[COLUMNS] AS _column
                WHERE _column.[table_schema] = @schemaName AND _column.[table_name] = @tableName
                ", conn);
            cmd.Parameters.AddWithValue("schemaName", schemaName);
            cmd.Parameters.AddWithValue("tableName", objectName);

            return cmd;
        }

        /// <summary>
        /// Generate command for getting all synonyms
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="schemaName">Schema name</param>
        /// <returns>SQL command</returns>
        private static SqlCommand GenerateGetSynonymsCommand(SqlConnection conn, string schemaName)
        {
            var cmd = new SqlCommand(@"
                SELECT _synonym.[name] AS [Name], _synonym.[base_object_name] AS [Target] FROM [sys].[synonyms] AS _synonym
                LEFT JOIN [sys].[schemas] AS _schema ON _schema.[schema_id] = _synonym.[schema_id]
                WHERE _schema.[name] LIKE @schemaName
                ", conn);
            cmd.Parameters.AddWithValue("schemaName", schemaName);

            return cmd;
        }
        #endregion

        #region Methods for filling DB object
        /// <summary>
        /// Fill data structure with data
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="structure">Structure to fill</param>
        /// <param name="schema">Schema</param>
        /// <param name="row">Row</param>
        /// <param name="verbose">Flag specifying whether to provide additional details regarding the execution or not</param>
        private static void FillDbDataStructure(SqlConnection conn, DbDataStructure structure, DbSchema schema, DataRow row, bool verbose = false)
        {
            #region STEP 1. Assign general info
            structure.Name = row.GetStringValue("Name");
            structure.CatalogName = schema.CatalogName;
            structure.SchemaName = schema.Name;
            #endregion

            AssertLog(verbose, $"Fetching [{structure.SchemaName}].[{structure.Name}]...");

            #region STEP 2. Fetch columns
            structure.Columns = new DbObjectCollection<DbColumn>();
            var columns = new DataTable("Columns");
            columns.Fill(GenerateGetColumnsCommand(conn, schema.Name, structure.Name));
            foreach (DataRow columnData in columns.Rows)
            {
                var column = new DbColumn();
                FillDbColumn(conn, column, schema, structure, columnData, verbose);
                structure.Columns.Add(column.Name, column);
            }
            #endregion
        }

        /// <summary>
        /// Fill column object with data
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="column">Column to fill</param>
        /// <param name="schema">Schema</param>
        /// <param name="structure">Structure</param>
        /// <param name="row">Row</param>
        /// <param name="verbose">Flag specifying whether to provide additional details regarding the execution or not</param>
        private static void FillDbColumn(SqlConnection conn, DbColumn column, DbSchema schema, DbDataStructure structure, DataRow row, bool verbose = false)
        {
            #region STEP 1. Assign general info
            column.Name = row.GetStringValue("Name");
            column.CatalogName = schema.CatalogName;
            column.SchemaName = schema.Name;
            column.OwnerName = structure.Name;
            column.Position = row.GetIntValue("OrdinalPosition");
            column.Default = row.GetStringValue("Default");
            column.IsNullable = row.GetStringValue("IsNullable") != "NO";
            #endregion

            AssertLog(verbose, $"Discovering column [{column.Name}] from [{column.SchemaName}].[{column.OwnerName}]...");

            #region STEP 2. Identify data type
            var dataTypeStr = row.GetStringValue("DataType");
            if (!Enum.TryParse(dataTypeStr, true, out DbColumnDataType dataType))
                throw new Exception("Unsupported data type \"" + dataTypeStr + "\"");

            column.DataType = dataType;
            #endregion

            #region STEP 3. Collect data info
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
                    var charDataInfo = new DbColumnDataCharacterInfo
                    {
                        MaxLength = row.GetIntValue("CharacterMaximumLength"),
                        OctetLength = row.GetIntValue("CharacterOctetLength"),
                        CharacterSetName = row.GetStringValue("CharacterSetName"),
                        CharacterSetCatalogName = row.GetStringValue("CharacterSetCatalogName"),
                        CharacterSetSchemaName = row.GetStringValue("CharacterSetSchemaName")
                    };
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
                    var numDataInfo = new DbColumnDataNumericInfo
                    {
                        Precision = row.GetNullableValue<byte?>("NumericPrecision", null),
                        PrecisionRadix = row.GetNullableValue<short?>("NumericPrecisionRadix", null),
                        Scale = row.GetNullableValue<int?>("NumericScale", null)
                    };
                    dataInfo = numDataInfo;
                    break;
                case DbColumnDataType.DATE:
                case DbColumnDataType.DATETIMEOFFSET:
                case DbColumnDataType.DATETIME2:
                case DbColumnDataType.SMALLDATETIME:
                case DbColumnDataType.DATETIME:
                case DbColumnDataType.TIME:
                    var dateTimeDataInfo = new DbColumnDataDateTimeInfo
                    {
                        Precision = row.GetValue<short>("DatetimePrecision")
                    };
                    dataInfo = dateTimeDataInfo;
                    break;
            }
            #endregion
            column.DataInfo = dataInfo;
            #endregion

            #region STEP 4. Collect collation info
            var collationInfo = new DbCollationInfo
            {
                Name = row.GetStringValue("CollationName"),
                CatalogName = row.GetStringValue("CollationCatalogName"),
                SchemaName = row.GetStringValue("CollationSchemaName")
            };
            column.Collation = collationInfo;
            #endregion
        }

        /// <summary>
        /// Fill synonym with data
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="synonym">Synonym to fill</param>
        /// <param name="schema">Schema</param>
        /// <param name="row">Row</param>
        /// <param name="verbose">Flag specifying whether to provide additional details regarding the execution or not</param>
        private static void FillDbSynonym(SqlConnection conn, DbSynonym synonym, DbSchema schema, DataRow row, bool verbose = false)
        {
            synonym.Name = row.GetStringValue("Name");
            synonym.CatalogName = schema.CatalogName;
            synonym.SchemaName = schema.Name;

            string targetName;
            string targetSchemaName = schema.Name;

            var targetNameParts = DecodeDbObjectName(row.GetStringValue("Target"));
            if (targetNameParts.Length == 1)
            {
                targetName = targetNameParts[0];
            }
            else
            {
                targetSchemaName = targetNameParts[0];
                targetName = targetNameParts[1];
            }

            if (targetSchemaName.Equals(schema.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                synonym.TargetObject = schema.GetObject(targetName);
            }
            else
            {
                synonym.TargetObject = new DbUnknownSchemaObject()
                {
                    SchemaName = targetSchemaName,
                    Name = targetName
                };
            }
        }

        /// <summary>
        /// Fill routine with data
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="routine">Routine to fill</param>
        /// <param name="schema">Schema</param>
        /// <param name="row">Row</param>
        /// <param name="verbose">Flag specifying whether to provide additional details regarding the execution or not</param>
        private static void FillDbRoutine(SqlConnection conn, DbRoutine routine, DbSchema schema, DataRow row, bool verbose = false)
        {
            routine.Name = row.GetStringValue("Name");
            routine.CatalogName = schema.CatalogName;
            routine.SchemaName = schema.Name;
            routine.DDLScript = row.GetStringValue("Definition");
        }
        #endregion

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
            schema.CatalogName = catalogName;

            #region STEP 2. Fetch tables
            schema.Tables = new DbObjectCollection<DbTable>();
            var t1 = new Thread(() =>
            {
                var tables = new DataTable("Tables");
                using (var c = new SqlConnection(conn.ConnectionString))
                {
                    c.Open();
                    tables.Fill(GenerateGetTablesCommand(c, schemaName));
                    foreach (DataRow r in tables.Rows)
                    {
                        var table = new DbTable();
                        FillDbDataStructure(c, table, schema, r, verbose);
                        schema.Tables.Add(table.Name, table);
                    }
                }
            });
            t1.Start();
            #endregion

            #region STEP 3. Fetch views
            schema.Views = new DbObjectCollection<DbView>();
            var t2 = new Thread(() =>
            {
                var views = new DataTable("Views");
                using (var c = new SqlConnection(conn.ConnectionString))
                {
                    c.Open();
                    views.Fill(GenerateGetViewsCommand(c, schemaName));
                    foreach (DataRow r in views.Rows)
                    {
                        var view = new DbView();
                        FillDbDataStructure(c, view, schema, r, verbose);
                        view.DDLScript = r.GetStringValue("Definition");
                        schema.Views.Add(view.Name, view);
                    }
                }
            });
            t2.Start();
            #endregion

            #region STEP 4. Fetch function
            schema.Functions = new DbObjectCollection<DbFunction>();
            var t3 = new Thread(() =>
            {
                var functions = new DataTable("Functions");
                using (var c = new SqlConnection(conn.ConnectionString))
                {
                    c.Open();
                    functions.Fill(GenerateGetFunctionsCommand(c, schemaName));
                    foreach (DataRow r in functions.Rows)
                    {
                        var function = new DbFunction();
                        FillDbRoutine(c, function, schema, r, verbose);
                        schema.Functions.Add(function.Name, function);
                    }
                }
            });
            t3.Start();
            #endregion

            #region STEP 5. Fetch procedures
            schema.Procedures = new DbObjectCollection<DbProcedure>();
            var t4 = new Thread(() =>
            {
                var procedures = new DataTable("Procedures");
                using (var c = new SqlConnection(conn.ConnectionString))
                {
                    c.Open();
                    procedures.Fill(GenerateGetProceduresCommand(c, schemaName));
                    foreach (DataRow r in procedures.Rows)
                    {
                        var procedure = new DbProcedure();
                        FillDbRoutine(c, procedure, schema, r, verbose);
                        schema.Procedures.Add(procedure.Name, procedure);
                    }
                }
            });
            t4.Start();
            #endregion

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            #region STEP 6. Fetch synonyms
            schema.Synonyms = new DbObjectCollection<DbSynonym>();
            var synonyms = new DataTable("Synonyms");
            synonyms.Fill(GenerateGetSynonymsCommand(conn, schemaName));
            foreach (DataRow r in synonyms.Rows)
            {
                var synonym = new DbSynonym();
                FillDbSynonym(conn, synonym, schema, r, verbose);
                schema.Synonyms.Add(synonym.Name, synonym);
            }
            #endregion

            return schema;
        }
    }
}
