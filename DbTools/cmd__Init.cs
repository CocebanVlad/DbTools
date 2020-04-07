using DbTools.Core;
using System;
using System.Collections.Generic;

namespace DbTools
{
    public partial class Program
    {
        private void exec__cmd_Init(IReadOnlyDictionary<string, string> p)
        {
            #region c
            if (!p.ContainsKey("c"))
                throw new ArgumentNullException("-c: Connection string");
            var c = p["c"];
            #endregion

            #region n
            if (!p.ContainsKey("n"))
                throw new ArgumentNullException("-n: Schema name");
            var n = p["n"];
            #endregion

            #region o
            if (!p.ContainsKey("o"))
                throw new ArgumentNullException("-o: Output file");
            var o = p["o"];
            #endregion

            #region v
            var v = p.ContainsKey("v");
            #endregion

            var file = DbSchemaFile.Factory.Init(c, n, v);
            file.Save(o);
        }
    }
}
