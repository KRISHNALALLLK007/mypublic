using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Demo.Contracts.BusinessService;
using Sample.Demo.Web.Dtos;
using Sample.Shared.Utilities.ApplicationContext;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Sample.Demo.Contracts.Data;

namespace Sample.Demo.Web.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly IApplicationContext _applicationContext;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        public UserController(IUserService userService, ILoggerFactory loggerFactory, IApplicationContext applicationContext, IConfiguration configuration,
             IHostingEnvironment hostingEnvironment)
            : base(applicationContext)
        {
            _logger = loggerFactory.CreateLogger(typeof(UserController));
            _userService = userService;
            _applicationContext = applicationContext;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("api/usertypes")]
        [ProducesResponseType(typeof(List<UserTypeDto>), 200)]
        public IActionResult GetAllUserTypes()
        {
            var userinfo = (User?.FindFirst(ClaimTypes.Email)?.Value);
            _logger.LogInformation("Entering UserController.GetAllUserTypes()");
            return new ObjectResult(Mapper.Map<List<UserTypeDto>>(_userService.GetAllUserTypes()));
        }

        [HttpPost]
        [Route("api/usertypes")]
        public IActionResult SaveUserTypes(UserTypeDto userTypeDto)
        {
            return new ObjectResult(_userService.SaveUserTypes(Mapper.Map<UserTypeDto, IUserType>(userTypeDto)));
        }

        [HttpGet]
        [Route("api/status")]
        public string Status()
        {
            _logger.LogInformation("Entering UserController.status()");
            return "Service is running.";
        }
    }
}