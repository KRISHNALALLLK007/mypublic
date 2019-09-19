using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Shared.Utilities.ApplicationContext
{
    public class ApplicationContext : IApplicationContext
    {
        private readonly Dictionary<string, object> _sharedObjects = new Dictionary<string, object>();

        public void SetSharedObjectByKey(string key, object obj)
        {
            if (!string.IsNullOrEmpty(key) && obj != null)
            {
                if (!_sharedObjects.ContainsKey(key))
                {
                    _sharedObjects[key] = obj;
                }
                else
                {
                    _sharedObjects.Add(key, obj);
                }
            }
        }

        public object GetSharedObjectByKey(string key)
        {
            return _sharedObjects.Any(p=>p.Key.Equals(key)) ? _sharedObjects.First(p=> p.Key.Equals(key)).Value : null;
        }
    }
}
