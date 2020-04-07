using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DbTools.Core
{
    public class DbSchemaFileFactory
    {
        /// <summary>
        /// Initialize a new DB schema file
        /// </summary>
        /// <param name="conn">SQL connection string</param>
        /// <param name="schemaName">Schema name</param>
        /// <param name="verbose">Flag specifying whether to provide additional details regarding the execution or not</param>
        /// <returns></returns>
        public DbSchemaFile Init(string connStr, string schemaName, bool verbose = false)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                return new DbSchemaFile()
                {
                    ConnectionString = connStr,
                    Schema = DbHelpers.GetSchema(conn, schemaName, verbose)
                };
            }
        }

        /// <summary>
        /// Load a schema file
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>DB schema file</returns>
        public DbSchemaFile Load(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var ser = new BinaryFormatter();
                return (DbSchemaFile)ser.Deserialize(stream);
            }
        }
    }
}
