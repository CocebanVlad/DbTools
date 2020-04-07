using DbTools.Core;
using System;

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
            exec.Register("init", (p) =>
            {
                if (!p.ContainsKey("c"))
                    throw new ArgumentNullException("-c: Connection string");
                var c = p["c"];

                if (!p.ContainsKey("n"))
                    throw new ArgumentNullException("-n: Schema name");
                var d = p["n"];

                if (!p.ContainsKey("o"))
                    throw new ArgumentNullException("-o: Output file");
                var o = p["o"];

                var file = DbSchemaFile.Factory.Init(p["c"], p["n"], p.ContainsKey("v"));
                file.Save(o);
            });
        }
    }
}
