using System;
using System.Collections.Generic;

namespace DbTools.Core.Models
{
    public class DbObjectCollection<T> : Dictionary<string, T> where T : DbObject
    {
        public DbObjectCollection()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }
    }
}
