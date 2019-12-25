using DbTools.Core;
using System;
using System.Data.SqlClient;

namespace DbTools
{
    public class Program : CliProgram
    {
        public static void Main(string[] args)
        {
            var cli = new Program();
            cli.Run(args);
        }

        /// <summary>
        /// Register commands
        /// </summary>
        /// <param name="exec">Executor</param>
        protected override void RegisterCommands(CliExec exec)
        {
            exec.Register("clone", (p) =>
            {
                if (!p.ContainsKey("c"))
                    throw new ArgumentNullException("-c: Connection string");
                var c = p["c"];

                if (!p.ContainsKey("d"))
                    throw new ArgumentNullException("-d: Directory");
                var d = p["d"];

                using (var conn = new SqlConnection(c))
                {
                    var schema = DbHelpers.GetSchema(conn, "dbo");
                }

                // Incomplete implementation
            });
        }
    }
}
