using DbTools.Core.Models;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DbTools.Core
{
    [Serializable]
    public class DbSchemaFile
    {
        /// <summary>
        /// Get connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Get DB schema
        /// </summary>
        public DbSchema Schema { get; set; }

        /// <summary>
        /// Get DB schema file factory
        /// </summary>
        public static DbSchemaFileFactory Factory { get; } = new DbSchemaFileFactory();

        /// <summary>
        /// Save schema file
        /// </summary>
        /// <param name="path">File path</param>
        public void Save(string path)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var ser = new BinaryFormatter();
                ser.Serialize(stream, this);
            }
        }
    }
}
