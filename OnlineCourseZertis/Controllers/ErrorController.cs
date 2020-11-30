using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourseZertis.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Noaccess(string language)
        {
            if (language == null) language = "ru";

            ViewBag.language = language;
            return View();
        }
    }
}