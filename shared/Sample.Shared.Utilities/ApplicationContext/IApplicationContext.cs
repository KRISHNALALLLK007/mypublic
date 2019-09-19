using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Shared.Utilities.ApplicationContext
{
    public interface IApplicationContext
    {
        void SetSharedObjectByKey(string key, Object obj);
        object GetSharedObjectByKey(string key);
    }
}
