using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DbTools.Core
{
    public sealed class CliCmd : IEquatable<string>
    {
        /// <summary>
        /// Get requested command
        /// </summary>
        public string Cmd { get; }

        /// <summary>
        /// Get parameters
        /// </summary>
        public IReadOnlyDictionary<string, string> Params { get; }

        public CliCmd(string str)
        {
            if (str == null)
                return;
            var lastIdx = str.Length - 1;
            var idx = str.IndexOf(' ');
            if (idx == -1)
                idx = lastIdx + 1;
            Cmd = str.Substring(0, idx);

            var parameters =
                new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (idx != lastIdx)
            {
                #region Collect parameters
                var keyStrBldr = new StringBuilder();
                var valStrBldr = new StringBuilder();
                var strBldr = keyStrBldr;
                var strBldrLock = false;
                var collect =
                    new Action(() =>
                    {
                        parameters[keyStrBldr.ToString()] = valStrBldr.ToString();
                        keyStrBldr.Clear();
                        valStrBldr.Clear();
                        strBldr = keyStrBldr;
                        strBldrLock = false;
                    });

                while (++idx < str.Length)
                {
                    switch (str[idx])
                    {
                        case ' ':
                            // @if current string builder is locked then append char
                            // @else if current string builder is not locked and
                            //    the previous character is diffrent from the current one (space) then
                            //    swap string builders;
                            if (strBldrLock)
                                strBldr.Append(str[idx]);
                            else if (str[idx] != str[idx - 1])
                                strBldr = strBldr == keyStrBldr ? valStrBldr : keyStrBldr;

                            // @if current string builder is targeting the key and the key is already formed
                            //    then collect parameter;
                            if (strBldr == keyStrBldr && keyStrBldr.Length > 0)
                                collect();
                            break;
                        case '-':
                            // @if current string builder is not locked,
                            //    it points to the value and
                            //    the value is empty (since the value cannot start with a '-' character)
                            //    then collect parameter and swap string builders;
                            if (!strBldrLock && strBldr == valStrBldr && valStrBldr.Length == 0)
                            {
                                collect();
                                strBldr = keyStrBldr;
                            }

                            // @if current string builder is locked and it point to value
                            //    or it points to an non-empty key
                            //    then append char;
                            if (strBldrLock || strBldr == valStrBldr || (strBldr == keyStrBldr && keyStrBldr.Length > 0))
                                strBldr.Append(str[idx]);
                            break;
                        case '"':
                            // @if current string builder points value then toggle string builder lock;
                            if (strBldr == valStrBldr)
                                strBldrLock = !strBldrLock;
                            break;
                        default:
                            strBldr.Append(str[idx]);
                            break;
                    }

                    // @if we are over the last character from the string
                    //    then collect parameter;
                    if (idx == lastIdx)
                        collect();
                }
                #endregion
            }

            Params = new ReadOnlyDictionary<string, string>(
                parameters
                );
        }

        /// <summary>
        /// Check equality between CliCmd object and a string-represented command
        /// </summary>
        /// <param name="other">Command</param>
        /// <returns>true if equal, otherwise false</returns>
        public bool Equals(string other)
        {
            return Cmd.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
