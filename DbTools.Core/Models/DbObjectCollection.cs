using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DbTools.Core.Models
{
    [Serializable]
    public class DbObjectCollection<T> : Dictionary<string, T> where T : DbObject
    {
        public DbObjectCollection() : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        protected DbObjectCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Execute an action for each item in collection
        /// </summary>
        /// <param name="actn">Action to execute</param>
        public void ForEach(Action<string, T> actn)
        {
            foreach (var pair in this)
                actn.Invoke(pair.Key, pair.Value);
        }
    }
}
