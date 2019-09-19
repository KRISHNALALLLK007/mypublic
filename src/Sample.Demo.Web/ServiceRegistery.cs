using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Sample.Demo.Contracts.BusinessService;
using Sample.Demo.BusinessService;
using Sample.Demo.DataService;
using Sample.Demo.Contracts.DataService;
using Sample.Demo.Web.Dtos;
using Sample.Demo.Contracts;
using Sample.Shared.Utilities.Logging;
using Sample.Shared.Utilities.ApplicationContext;
using Sample.Demo.Contracts.Data;
using Sample.Demo.Data;

namespace Sample.Demo.Web
{
    public static class ServiceRegistery
    {
        public static void RegisterAllServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterBusinessServices();
            serviceCollection.RegisterDataServices();
            serviceCollection.RegisterOtherServices();
            serviceCollection.RegisterDomainModels();
            InitializeAutoMapper();
        }
        private static void RegisterBusinessServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService,UserService>();
        }
        private static void RegisterDataServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserDataService, UserDataService>();
        }
        private static void RegisterOtherServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ILogHandler, SeriLogHandler>();
            serviceCollection.AddSingleton<IApplicationContext, ApplicationContext>();
            serviceCollection.AddScoped<IRequestEventLogger, RequestEventLogger>();
        }
        private static void RegisterDomainModels(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserType, UserType>();
        }
        private static void InitializeAutoMapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<LoginCrdentialsDto, ILoginCrdentials>();
                config.CreateMap<UserTypeDto,IUserType>().As<UserType>(); 
            });
        }
    }
}
