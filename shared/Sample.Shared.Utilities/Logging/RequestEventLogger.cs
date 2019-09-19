using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Sample.Shared.Utilities.Constants;
using System;

namespace Sample.Shared.Utilities.Logging
{
    public class RequestEventLogger : IRequestEventLogger
    {
        private readonly IConfiguration _configuration;
        private readonly ElasticClient _elasticClient;
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestEventLogger"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        public RequestEventLogger(IConfiguration configuration)
        {
            _configuration = configuration;
            _elasticClient = new ElasticClient(new ConnectionSettings(new Uri(_configuration.GetSection(ConfigurationConstants.Serilog).GetValue<string>(ConfigurationConstants.ElasticSearchUrl))));
        }

        /// <summary>
        /// Logs the course request event.
        /// </summary>
        /// <param name="requestEventLog">The request event log.</param>
        public void LogCourseRequestEvent(CourseRequestEventLog requestEventLog)
        {
            var serilLogSection = _configuration.GetSection(ConfigurationConstants.Serilog);
            //requestEventLog.Environment = serilLogSection.GetValue<string>(ConfigurationConstants.Environment);
            _elasticClient.IndexAsync(requestEventLog, idx => idx.Index(serilLogSection.GetValue<string>(ConfigurationConstants.RequestEventLogName) + DateTime.UtcNow.Year + ApplicationConstants.Dot + DateTime.UtcNow.Month.ToString("00") + ApplicationConstants.Dot + DateTime.UtcNow.Day.ToString("00")));
        }
    }
}
