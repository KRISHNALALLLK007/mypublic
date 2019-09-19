using Sample.Demo.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Demo.Contracts.BusinessService
{
    public interface IUserService
    {
        string Validate(ILoginCrdentials loginCrdentials);
        List<IUserType> GetAllUserTypes();
        bool SaveUserTypes(IUserType userType);
    }
}
