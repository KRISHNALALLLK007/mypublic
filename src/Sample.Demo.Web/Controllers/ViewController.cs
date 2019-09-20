using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Demo.Web.Controllers
{
    public class ViewController : Controller
    {
        [Route("view/home")]
        [HttpGet]
        public IActionResult Home()
        {
            return View("./Views/Home.cshtml");
        }

        [Route("view/user")]
        [HttpGet]
        public IActionResult GetUser()
        {
            return View("./Views/User.cshtml");
        }
    }
}