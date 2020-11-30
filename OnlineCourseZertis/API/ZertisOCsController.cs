using OnlineCourseZertis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineCourseZertis.API
{
    public class ZertisOCsController : ApiController
    {

        private Entities db = new Entities();

        [AllowAnonymous]
        [HttpGet]
        [Route("api/data/forall")]
        public IHttpActionResult Get()
        {
            return Ok("Now server time is: " + DateTime.Now.ToString());
        }



        [Authorize]
        [HttpGet]
        public IHttpActionResult UserInfo()
        {
            UserInfo UserInfo = db.UserInfoes.Where(e => e.UserName == User.Identity.Name).FirstOrDefault();

            return Json(UserInfo);
        }



    }
}
