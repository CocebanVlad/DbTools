using System;
using System.Collections.Generic;

namespace DbTools.Core
{
    public sealed class CliExec
    {
        private Dictionary<string, CliAction> _registry =
            new Dictionary<string, CliAction>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Encapsulates a method intended for execution of a command
        /// </summary>
        /// <param name="parameters">Extra parameters</param>
        public delegate void CliAction(IReadOnlyDictionary<string, string> parameters);

        /// <summary>
        /// Get registered commands
        /// </summary>
        public IEnumerable<string> Cmds
        {
            get
            {
                foreach (var key in _registry.Keys)
                    yield return key;
            }
        }

        /// <summary>
        /// Register command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="actn">Action</param>
        public void Register(string cmd, CliAction actn)
        {
            _registry[cmd] = actn;
        }

        /// <summary>
        /// Execute action for command
        /// </summary>
        /// <param name="str">Command</param>
        public void Exec(string str)
        {
            var cmd =
                new CliCmd(str);
            if (!_registry.ContainsKey(cmd.Cmd))
                throw new CliException(
                    string.Format(
                        resources.__cli_error__0__is_not_recognized_as_command,
                        cmd.Cmd
                        ));

            _registry[cmd.Cmd].Invoke(cmd.Params);
        }

        /// <summary>
        /// Get documentation for a command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <returns>A string</returns>
        public string GetDoc(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
                return null;
            var doc = resources.ResourceManager.GetString(
                string.Format(
                    "__cli_cmd__{0}",
                    cmd.ToLower()
                    ));

            return string.IsNullOrEmpty(doc)
                ? resources.__cli_undocumented_command
                : doc;
        }
    }
}
