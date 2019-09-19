using Sample.Demo.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Demo.Contracts.DataService
{
    public interface IUserDataService
    {
        string Validate(ILoginCrdentials loginCrdentials);
        List<IUserType> GetAllUserTypes();

        bool SaveUserTypes(IUserType userType);
    }
}
