using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Shared.Utilities.Constants
{
    public static class ApplicationConstants
    {
        public static string TemplateRootPath = "/Templates/Email/";
        public const string Slash = "/";
        public const string Dot = ".";
    }

    public enum Language
    {
        English=1,
        Arabic=2
    }


    public static class EmailTemplates
    {
        public static string RegistrationVerificationEmail = "RegistrationVerificationEmail.html";
    }


    public static class EmailTemplateConstants
    {
        public const string RegisrationMailVerificationLink = "$$RegisrationMailVerificationLink$$";
    }

    public static class ConfigurationConstants
    {
        public const string Serilog = "Serilog";
        public const string Environment = "Environment";
        public const string IndexFormat = "IndexFormat";
        public const string ElasticSearchUrl = "ElasticSearchUrl";
        public const string RequestEventLogName = "RequestEventLogName";
        public const string ApplicationName = "ApplicationName";
    }
}
