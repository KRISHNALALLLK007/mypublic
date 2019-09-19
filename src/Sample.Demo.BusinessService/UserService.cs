using Sample.Demo.Contracts;
using Sample.Demo.Contracts.BusinessService;
using Sample.Demo.Contracts.Data;
using Sample.Demo.Contracts.DataService;
using System;
using System.Collections.Generic;

namespace Sample.Demo.BusinessService
{
    public class UserService : IUserService
    {
        private readonly IUserDataService _userDataService;
        public UserService(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }
        public string Validate(ILoginCrdentials loginCrdentials)
        {
            return _userDataService.Validate(loginCrdentials);
        }

        public List<IUserType> GetAllUserTypes()
        {
            return _userDataService.GetAllUserTypes();
        }

        public bool SaveUserTypes(IUserType userType)
        {
            return _userDataService.SaveUserTypes(userType);
        }
    }
}
