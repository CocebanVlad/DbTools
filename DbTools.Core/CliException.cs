using System;

namespace DbTools.Core
{
    public sealed class CliException : Exception
    {
        public CliException(string msg) : base(msg)
        {
        }

        public CliException(string msg, Exception innerEx) : base(msg, innerEx)
        {
        }
    }
}
