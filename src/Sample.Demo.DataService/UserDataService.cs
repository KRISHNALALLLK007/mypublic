using Sample.Demo.Contracts;
using Sample.Demo.Contracts.Data;
using Sample.Demo.Contracts.DataService;
using Sample.Demo.Data;
using Sample.Shared.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Demo.DataService
{
    public class UserDataService : IUserDataService
    {
        private readonly DemoDbContext _demoDbContext;
        public UserDataService(DemoDbContext demoDbContext)
        {
            _demoDbContext = demoDbContext;
        }
        public string Validate(ILoginCrdentials loginCrdentials)
        {
            if (loginCrdentials != null && loginCrdentials.Username == "Kri" && loginCrdentials.Password == "123")
            {
                return "authtoken";
            }
            return null;
        }

        public List<IUserType> GetAllUserTypes()
        {
            var list = _demoDbContext.UserTypes.ToList<IUserType>();
            return list;
        }

        public bool SaveUserTypes(IUserType userType)
        {
            //var userSession = new UserSession
            //{ 
            //    UserId = "12345"
            //};
            //// First add into the db
            //_demoDbContext.UserTypes.Add(userType as UserType);

            //_demoDbContext.UserTypes.Add(userType as UserType);

            //// Second update operation 
            //var type = _demoDbContext.UserTypes.FirstOrDefault(x => x.Id == 1);
            //type.Name = "Updated Name";

            //var deleteType = _demoDbContext.UserTypes.FirstOrDefault(x => x.Id == 2);
            //_demoDbContext.UserTypes.Remove(deleteType);

            //_demoDbContext.SaveChanges(userSession);

            var user = _demoDbContext.Users.FirstOrDefault(x => x.UserType.Id==1);
            return true;
        }
    }
}
