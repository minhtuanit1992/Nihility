using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nihility.X0.Solution.Areas.Administrator.Controllers
{
    public class BaseUIController : Controller
    {
        public ActionResult Card()
        {
            return View();
        }
    }
}