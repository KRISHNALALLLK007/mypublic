using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Shared.Utilities.ApplicationContext;

namespace Sample.Demo.Web.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IApplicationContext _applicationContext;
        public BaseController(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            SetupApplicationContext();
        }


        private void SetupApplicationContext()
        {
            SetApplicationSettings();
            SetRequestDetails();
        }

        private void SetApplicationSettings()
        {
            if (_applicationContext.GetSharedObjectByKey("ApplicationSettings") == null)
            {
                // Call service to collect application settings value from table.
                var c = new Dictionary<string, object>();
                c.Add("DateFormat", "DD/MM/YYYY");
                c.Add("Pagination", 1);
                _applicationContext.SetSharedObjectByKey("ApplicationSettings", c);
            }
        }

        private void SetRequestDetails()
        {

            //if (_applicationContext.GetSharedObjectByKey("RequestDetails") == null)
            //{
            //    // Call service to collect application settings value from table.
            //    var c = new Dictionary<string, object>();
            //    c.Add("RequestToken", "DD/MM/YYYY");
            //    c.Add("Pagination", 1);
            //    _applicationContext.SetSharedObjectByKey("ApplicationSettings", c);
            //}
        }
    }
}