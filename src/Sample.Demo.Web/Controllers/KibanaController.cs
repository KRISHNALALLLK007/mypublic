using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Shared.Utilities.ApplicationContext;
using Sample.Shared.Utilities.Logging;
using System;

namespace Sample.Demo.Web.Controllers
{
    [ApiController]
    public class KibanaController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IRequestEventLogger _requestEventLogger;
        public KibanaController(ILoggerFactory loggerFactory, IApplicationContext applicationContext, IRequestEventLogger requestEventLogger)
            : base(applicationContext)
        {
            _logger = loggerFactory.CreateLogger(typeof(KibanaController));
            _requestEventLogger = requestEventLogger;
        }

        [HttpPost]
        [Route("api/v1/courses")]
        public IActionResult CreateCourse()
        {
            _logger.LogInformation("Creating Course");
            var requestEventLog = new CourseRequestEventLog
            {
                CourseName= "Course1",
                TransactionTimestamp = DateTime.UtcNow,
                TransactionName ="Add Course",
                TransactionTriggerPoint="Browser"
            };
            _requestEventLogger.LogCourseRequestEvent(requestEventLog);
            return Ok();
        }

        [HttpDelete]
        [Route("api/v1/courses/{id}")]
        public IActionResult DeleteCourse()
        {
            _logger.LogInformation("Delete Course");
            var requestEventLog = new CourseRequestEventLog
            {
                CourseName = "Course1",
                TransactionTimestamp = DateTime.UtcNow,
                TransactionName = "Delete Course",
                TransactionTriggerPoint = "Browser"
            };
            _requestEventLogger.LogCourseRequestEvent(requestEventLog);
            return Ok();
        }

        [HttpGet]
        [Route("api/v1/courses/{id}")]
        public IActionResult GetCourse(long id)
        {
            _logger.LogInformation("Get Course informations");
            return Ok();
        }

        [HttpGet]
        [Route("api/v1/error")]
        public IActionResult GetException()
        {
            throw new Exception();
        }
    }
}