using System;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbSchema : DbObject
    {
        /// <summary>
        /// Get or set schema tables
        /// </summary>
        public DbObjectCollection<DbTable> Tables { get; set; }

        /// <summary>
        /// Get or set schema views
        /// </summary>
        public DbObjectCollection<DbView> Views { get; set; }

        /// <summary>
        /// Get or set schema synonyms
        /// </summary>
        public DbObjectCollection<DbSynonym> Synonyms { get; set; }

        /// <summary>
        /// Get or set schema functions
        /// </summary>
        public DbObjectCollection<DbFunction> Functions { get; set; }

        /// <summary>
        /// Get or set schema procedures
        /// </summary>
        public DbObjectCollection<DbProcedure> Procedures { get; set; }

        /// <summary>
        /// Get schema object by name
        /// </summary>
        /// <param name="name">Object name</param>
        /// <returns>Schema object</returns>
        public DbSchemaObject GetObject(string name)
        {
            if (this.Tables != null && this.Tables.ContainsKey(name))
            {
                return this.Tables[name];
            }

            if (this.Views != null && this.Views.ContainsKey(name))
            {
                return this.Views[name];
            }

            if (this.Synonyms != null && this.Synonyms.ContainsKey(name))
            {
                return this.Synonyms[name];
            }

            if (this.Functions != null && this.Functions.ContainsKey(name))
            {
                return this.Functions[name];
            }

            if (this.Procedures != null && this.Procedures.ContainsKey(name))
            {
                return this.Procedures[name];
            }

            return null;
        }
    }
}
