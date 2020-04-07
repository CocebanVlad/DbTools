using System;
using System.Reflection;

namespace DbTools.Core
{
    public abstract class CliProgram
    {
        /// <summary>
        /// Get CLI executor
        /// </summary>
        protected CliExec Exec { get; } = new CliExec();

        /// <summary>
        /// Get the flag specifying whether the program is running or not
        /// </summary>
        protected bool Running { get; set; }

        protected CliProgram()
        {
        }

        /// <summary>
        /// Run (start execution)
        /// </summary>
        /// <param name="args">Arguments</param>
        public void Run(string[] args)
        {
            Running = true;
            Exec.Register("help", (p) => PrintHelp());
            RegisterCommands(Exec);

            if (args == null || args.Length == 0)
            {
                ExecuteInLoop();
            }
            else
            {
                Execute(string.Join(" ", args));
                Stop();
            }
        }

        /// <summary>
        /// Stop execution
        /// </summary>
        public void Stop()
        {
            Running = false;
        }

        #region Command execution
        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="cmd">Command</param>
        private void Execute(string cmd)
        {
            try
            {
                Exec.Exec(cmd);
            }
            catch (Exception ex)
            {
                CliHelpers.WriteLine(ex, ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Execute commands in loop
        /// </summary>
        private void ExecuteInLoop()
        {
            Exec.Register("quit", (p) => Stop());
            Exec.Register("cls", (p) => Console.Clear());

            Console.WriteLine();

            while (Running)
            {
                CliHelpers.Write("> ", ConsoleColor.White);
                Execute(CliHelpers.ReadLine());
            }
        }
        #endregion

        /// <summary>
        /// Print help (documentation)
        /// </summary>
        protected virtual void PrintHelp()
        {
            #region Print header
            CliHelpers.WriteLine();
            CliHelpers.WriteLine(Assembly.GetExecutingAssembly().FullName, ConsoleColor.White);
            CliHelpers.WriteLine(Environment.OSVersion, ConsoleColor.Gray);
            CliHelpers.WriteLine();
            #endregion

            foreach (var cmd in Exec.Cmds)
            {
                #region Print command
                CliHelpers.WriteLine(cmd, ConsoleColor.Cyan);
                CliHelpers.WriteLine(Exec.GetDoc(cmd), ConsoleColor.Gray);
                CliHelpers.WriteLine();
                #endregion
            }

            CliHelpers.WriteLine(new string('-', 5));
            CliHelpers.WriteLine();
        }

        /// <summary>
        /// Register commands
        /// </summary>
        /// <param name="exec">Executor</param>
        protected abstract void RegisterCommands(CliExec exec);
    }
}
