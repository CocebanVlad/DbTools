using DbTools.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DbTools
{
    public partial class Program
    {
        private void exec__cmd_GenFakeStruct(IReadOnlyDictionary<string, string> p)
        {
            #region f
            if (!p.ContainsKey("f"))
                throw new ArgumentNullException("-f: DB schema file");
            var f = p["f"];
            #endregion

            #region o
            if (!p.ContainsKey("o"))
                throw new ArgumentNullException("-o: Output file");
            var o = p["o"];
            #endregion

            #region v
            var v = p.ContainsKey("p");
            #endregion

            var file = DbSchemaFile.Factory.Load(f);
            var bldr = new StringBuilder();

            #region Create tables
            file.Schema.Tables.ForEach((_, obj) =>
            {
                bldr.AppendLine("CREATE TABLE " + obj.GetFullName() + "(A INT NULL);");
            });
            #endregion

            #region Create views
            file.Schema.Views.ForEach((_, obj) =>
            {
                bldr.AppendLine("CREATE VIEW " + obj.GetFullName() + " AS SELECT 1 AS [A];");
            });
            #endregion

            #region Create functions
            file.Schema.Functions.ForEach((_, obj) =>
            {
                bldr.AppendLine("CREATE FUNCTION " + obj.GetFullName() + "() RETURNS INT AS BEGIN RETURN 1 END;");
            });
            #endregion

            #region Create procedures
            file.Schema.Procedures.ForEach((_, obj) =>
            {
                bldr.AppendLine("CREATE PROCEDURE " + obj.GetFullName() + " AS BEGIN PRINT '1' END;");
            });
            #endregion

            File.WriteAllText(o, bldr.ToString());
        }
    }
}
