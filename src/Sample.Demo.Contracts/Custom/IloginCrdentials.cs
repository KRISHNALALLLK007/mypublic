using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Demo.Contracts
{
    public interface ILoginCrdentials
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}
