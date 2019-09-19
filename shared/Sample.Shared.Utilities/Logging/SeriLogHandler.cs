using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System;
using Sample.Shared.Utilities.Constants;

namespace Sample.Shared.Utilities.Logging
{
    public class SeriLogHandler : ILogHandler
    {
        private IConfiguration _configuration;
        private ILoggerFactory _loggerFactory;
        public SeriLogHandler(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }
        public void Initialize()
        {
            var serilogSection = _configuration.GetSection(ConfigurationConstants.Serilog);
            Log.Logger = new LoggerConfiguration()
                               .ReadFrom.Configuration(_configuration)
                               .Enrich.WithProperty(ConfigurationConstants.ApplicationName, serilogSection.GetValue<string>(ConfigurationConstants.ApplicationName))
                               .WriteTo.Elasticsearch(
                                    new ElasticsearchSinkOptions(new Uri(serilogSection.GetValue<string>(ConfigurationConstants.ElasticSearchUrl)))
                                    {
                                        AutoRegisterTemplate = true,
                                        IndexFormat = serilogSection.GetValue<string>(ConfigurationConstants.IndexFormat)
                                    })
                                .CreateLogger();
            _loggerFactory.AddSerilog();
        }
    }
}
