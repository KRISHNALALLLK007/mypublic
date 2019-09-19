using Sample.Demo.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Demo.Data
{
    public class LoginCrdentials : ILoginCrdentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
