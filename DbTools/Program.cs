using DbTools.Core;

namespace DbTools
{
    public partial class Program : CliProgram
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
            exec.Register("init", (p) => exec__cmd_Init(p));
            exec.Register("genfakestruct", (p) => exec__cmd_GenFakeStruct(p));
        }
    }
}
