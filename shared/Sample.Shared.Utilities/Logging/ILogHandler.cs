using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Shared.Utilities.Logging
{
    public interface ILogHandler
    {
        void Initialize();
    }
}
