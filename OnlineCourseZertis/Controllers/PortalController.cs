using OnlineCourseZertis.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineCourseZertis.Controllers
{

    [Authorize(Roles = "admin")]
    public class portalController : Controller
    {
        //
        private Entities db = new Entities();


        //----------------------------------- Users -----------------------------------------
        public ActionResult Users()
        {

            var Users = db.UserInfoes.ToList();
            return View(Users);
        }
        
        //[HttpPost]
        //public ActionResult removeUser(int Id)
        //{

        //    string Msg = "error";

        //    UserInfo userinfo = db.UserInfoes.Find(Id);

        //    try
        //    {

        //        db.EyeVs.RemoveRange(userinfo.EyeVs);

        //        db.Isbrannis.RemoveRange(userinfo.Isbrannis);

        //        db.LessonVideoTimes.RemoveRange(userinfo.LessonVideoTimes);

        //        db.LikeVs.RemoveRange(userinfo.LikeVs);

        //        db.User_Answer_QuizA.RemoveRange(userinfo.User_Answer_QuizA);

        //        db.User_Answer_QuizB.RemoveRange(userinfo.User_Answer_QuizB);

        //        if (userinfo.User_Premium.Count > 0)
        //        {

        //            foreach (var item in userinfo.User_Premium)
        //            {

        //                db.User_Premium_Status.RemoveRange(item.User_Premium_Status);
        //            }
        //        }

               


             

        //        db.User_Premium.RemoveRange(userinfo.User_Premium);
                
        //        db.Users_Certificates.RemoveRange(userinfo.Users_Certificates);

        //        db.ValitOS.RemoveRange(userinfo.ValitOS);

        //        JVLO jv = db.JVLOes.Where(e => e.UserName == userinfo.UserName).FirstOrDefault();

        //        db.JVLOes.Remove(jv);

        //        AspNetUser user = db.AspNetUsers.Where(e => e.UserName == userinfo.UserName).FirstOrDefault();

        //        db.UserInfoes.Remove(userinfo);
               

        //        db.AspNetUsers.Remove(user);




        //        db.SaveChanges();

        //        Msg = "success";

        //    }
        //    catch
        //    {

        //    }

        //    return Json(Msg);

        //}
        



        //----------------------------------- Modules -----------------------------------------
        public ActionResult Modules(string language)
        {
            if (language == null) { language = "ru"; }

            ViewBag.language = language;
            var Modules = db.Modules_Property.Where(e => e.lang == language).ToList();
            return View(Modules);
        }


        // Create
        public ActionResult createModule(string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createModule(ModuleCs newModule, string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;


            string Msg = "error";

            Modul createModule = new Modul();


            try
            {
                HttpPostedFileBase file1 = Request.Files["img1"];
                if (file1.FileName != "" && file1 != null)
                {

                    string ImageName = file1.FileName;
                    createModule.Image = "~/Images/ModulImage/" + ImageName.Replace(" ", "_");
                    file1.SaveAs(HttpContext.Server.MapPath(createModule.Image));

                }



                Modules_Property Modules_PRU = new Modules_Property();
                Modules_PRU.lang = "ru";
                Modules_PRU.Titile = newModule.NameRU;
                createModule.Modules_Property.Add(Modules_PRU);

                if (newModule.NameKZ != null)
                {
                    Modules_Property Modules_PKZ = new Modules_Property();
                    Modules_PKZ.lang = "kz";
                    Modules_PKZ.Titile = newModule.NameKZ;
                    createModule.Modules_Property.Add(Modules_PKZ);
                }




                if (newModule.NameUZ != null)
                {
                    Modules_Property Modules_PUZ = new Modules_Property();
                    Modules_PUZ.lang = "uz";
                    Modules_PUZ.Titile = newModule.NameUZ;
                    createModule.Modules_Property.Add(Modules_PUZ);
                }

                if (newModule.NameKR != null)
                {
                    Modules_Property Modules_PKR = new Modules_Property();
                    Modules_PKR.lang = "kr";
                    Modules_PKR.Titile = newModule.NameKR;
                    createModule.Modules_Property.Add(Modules_PKR);
                }

                if (newModule.NameEN != null)
                {
                    Modules_Property Modules_PEN = new Modules_Property();
                    Modules_PEN.lang = "en";
                    Modules_PEN.Titile = newModule.NameEN;
                    createModule.Modules_Property.Add(Modules_PEN);
                }


                if (newModule.NameTR != null)
                {
                    Modules_Property Modules_PTR = new Modules_Property();
                    Modules_PTR.lang = "tr";
                    Modules_PTR.Titile = newModule.NameTR;
                    createModule.Modules_Property.Add(Modules_PTR);
                }


                Modul_userLevel Modul_level = new Modul_userLevel();
                Modul_level.LevelId = newModule.LevelId;

                createModule.Modul_userLevel.Add(Modul_level);



                db.Moduls.Add(createModule);
                db.SaveChanges();
                Msg = "success";


            }
            catch { }
            ViewBag.AddModuleMsg = Msg;

            return View();
        }



        // Edit
        public ActionResult editModule(int Id, string language)
        {
            if (language == null) { language = "ru"; }

            Modul Module = db.Moduls.Find(Id);
            ViewBag.language = language;

            ModuleCs ModuleCs = new ModuleCs();
            ModuleCs.ModuleId = Id;
            ModuleCs.LevelId = Module.Modul_userLevel.FirstOrDefault().LevelId;

            Modules_Property Modules_PKZ = Module.Modules_Property.Where(e => e.lang == "kz").FirstOrDefault();
            if (Modules_PKZ != null)
            {
                ModuleCs.NameKZ = Modules_PKZ.Titile;
            }


            Modules_Property Modules_PRU = Module.Modules_Property.Where(e => e.lang == "ru").FirstOrDefault();
            if (Modules_PRU != null)
            {
                ModuleCs.NameRU = Modules_PRU.Titile;
            }

            Modules_Property Modules_PUZ = Module.Modules_Property.Where(e => e.lang == "uz").FirstOrDefault();
            if (Modules_PUZ != null)
            {
                ModuleCs.NameUZ = Modules_PUZ.Titile;
            }

            Modules_Property Modules_PKR = Module.Modules_Property.Where(e => e.lang == "kr").FirstOrDefault();
            if (Modules_PKR != null)
            {
                ModuleCs.NameKR = Modules_PKR.Titile;
            }

            Modules_Property Modules_PEN = Module.Modules_Property.Where(e => e.lang == "en").FirstOrDefault();
            if (Modules_PEN != null)
            {
                ModuleCs.NameEN = Modules_PEN.Titile;
            }

            Modules_Property Modules_PTR = Module.Modules_Property.Where(e => e.lang == "tr").FirstOrDefault();
            if (Modules_PTR != null)
            {
                ModuleCs.NameTR = Modules_PTR.Titile;
            }

            ModuleCs.Image = Module.Image;

            return View(ModuleCs);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editModule(ModuleCs newModule, string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            string Msg = "error";

            Modul editModule = db.Moduls.Find(newModule.ModuleId);

            try
            {

                HttpPostedFileBase file1 = Request.Files["img1"];
                if (file1.FileName != "" && file1 != null)
                {
                    string fullPath = Request.MapPath(editModule.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImageName = file1.FileName;
                    editModule.Image = "~/Images/ModulImage/" + ImageName.Replace(" ", "_");
                    newModule.Image = editModule.Image;
                    file1.SaveAs(HttpContext.Server.MapPath(editModule.Image));

                }

                Modules_Property Modules_PKZ = editModule.Modules_Property.Where(e => e.lang == "kz").FirstOrDefault();
                if (Modules_PKZ != null)
                {
                    Modules_PKZ.Titile = newModule.NameKZ;
                }



                Modules_Property Modules_PRU = editModule.Modules_Property.Where(e => e.lang == "ru").FirstOrDefault();
                if (Modules_PRU != null)
                {
                    Modules_PRU.Titile = newModule.NameRU;
                }

                Modules_Property Modules_PUZ = editModule.Modules_Property.Where(e => e.lang == "uz").FirstOrDefault();
                if (Modules_PUZ != null)
                {
                    Modules_PUZ.Titile = newModule.NameUZ;
                }

                Modules_Property Modules_PKR = editModule.Modules_Property.Where(e => e.lang == "kr").FirstOrDefault();
                if (Modules_PKR != null)
                {
                    Modules_PKR.Titile = newModule.NameKR;
                }

                Modules_Property Modules_PEN = editModule.Modules_Property.Where(e => e.lang == "en").FirstOrDefault();
                if (Modules_PEN != null)
                {
                    Modules_PEN.Titile = newModule.NameEN;
                }

                Modules_Property Modules_PTR = editModule.Modules_Property.Where(e => e.lang == "tr").FirstOrDefault();
                if (Modules_PTR != null)
                {
                    Modules_PTR.Titile = newModule.NameTR;
                }


                Modul_userLevel Modul_level = editModule.Modul_userLevel.FirstOrDefault();
                Modul_level.LevelId = newModule.LevelId;



                db.SaveChanges();
                Msg = "success";
                ViewBag.EditModuleMsg = Msg;
            }
            catch { }


            return View(newModule);
        }



        //----------------------------------- Video Lessons -----------------------------------------


        //Index
        public ActionResult VideoLessons(string language)
        {
            if (language == null) { language = "ru"; }

            ViewBag.language = language;
            var VideoLessons = db.VideoLs.Where(e => e.language == language).OrderBy(e => e.XId).ToList();

            return View(VideoLessons);
        }



        // Create
        public ActionResult createvlessons(string language)
        {

            ViewBag.language = language;

            var Modules = db.Modules_Property.Where(e => e.lang == language).ToList();

            return View(Modules);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createvlessons(VideoL videols, int minute, string language, string Id)
        {

            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            string Msg = "error";

            try
            {
                videols.XId = Convert.ToDouble(Id.Replace(",", "."));
            }
            catch
            {
                Msg = "warning02";
            }

            HttpPostedFileBase file1 = Request.Files["img1"];
            if (file1.FileName != "" && file1 != null)
            {

                string ImageName = file1.FileName;
                videols.Iconimg = "~/Images/Limage/" + language + "/" + ImageName.Replace(" ", "_");
                file1.SaveAs(HttpContext.Server.MapPath(videols.Iconimg));

            }


            HttpPostedFileBase file2 = Request.Files["img2"];
            if (file2.FileName != "" && file2 != null)
            {

                string ImageName = file2.FileName;
                videols.Iconimg2 = "~/Images/Limage/" + language + "/" + ImageName.Replace(" ", "_");
                file2.SaveAs(HttpContext.Server.MapPath(videols.Iconimg2));

            }


            VideoL video = db.VideoLs.Where(e => e.language == language && e.XId == videols.XId).FirstOrDefault();

            if (video == null)
            {
                VideoXL videoXL = db.VideoXLs.Where(e => e.XId == videols.XId).FirstOrDefault();

                if (videoXL == null)
                {
                    VideoXL newVideoXL = new VideoXL();
                    newVideoXL.XId = videols.XId;
                    newVideoXL.MId = videols.ModulId;
                    db.VideoXLs.Add(newVideoXL);



                    VideoLEM newVideLem = new VideoLEM();
                    newVideLem.Eye = 0;
                    newVideLem.Likes = 0;
                    newVideLem.minute = minute;
                    newVideLem.VideoXId = videols.XId;
                    db.VideoLEMs.Add(newVideLem);
                }

                db.VideoLs.Add(videols);
                db.SaveChanges();
                Msg = "success";

            }
            else if (video != null)
            {
                Msg = "warning01";
            }

            if (Msg != "success")
            {
                if (file1.FileName != "" && file1 != null)
                {

                    string fullPath = Request.MapPath(videols.Iconimg);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }
                if (file2.FileName != "" && file2 != null)
                {

                    string fullPath = Request.MapPath(videols.Iconimg2);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }
            }

            ViewBag.AddVideoMsg = Msg;
            var Modules = db.Modules_Property.Where(e => e.lang == language).ToList();
            return View(Modules);
        }



        // EDIT
        public ActionResult editvlessons(int Id, string language)
        {

            if (language == null) { language = "ru"; }
            ViewBag.language = language;


            var Modules = db.Modules_Property.Where(e => e.lang == language).ToList();
            ViewBag.Modules = Modules;

            VideoL videols = db.VideoLs.Find(Id);

            return View(videols);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editvlessons(VideoL videols, int minute, string language)
        {

            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            string Msg = "error";

            VideoL oldVideoL = db.VideoLs.Find(videols.Id);

            try
            {

                HttpPostedFileBase file1 = Request.Files["img1"];
                if (file1.FileName != "" && file1 != null)
                {

                    string fullPath = Request.MapPath(oldVideoL.Iconimg);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImageName = file1.FileName;
                    oldVideoL.Iconimg = "~/Images/Limage/" + language + "/" + ImageName.Replace(" ", "_");
                    file1.SaveAs(HttpContext.Server.MapPath(oldVideoL.Iconimg));

                }


                HttpPostedFileBase file2 = Request.Files["img2"];
                if (file2.FileName != "" && file2 != null)
                {
                    string fullPath = Request.MapPath(oldVideoL.Iconimg2);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImageName = file2.FileName;
                    oldVideoL.Iconimg2 = "~/Images/Limage/" + language + "/" + ImageName.Replace(" ", "_");
                    file2.SaveAs(HttpContext.Server.MapPath(oldVideoL.Iconimg2));
                }



                oldVideoL.language = videols.language;
                oldVideoL.Link = videols.Link;
                oldVideoL.Name = videols.Name;
                oldVideoL.ModulId = videols.ModulId;
                oldVideoL.ShortT = videols.ShortT;
                oldVideoL.VideoXL.MId = videols.ModulId;





                VideoLEM videolem = oldVideoL.VideoXL.VideoLEMs.FirstOrDefault();
                videolem.minute = minute;



                db.SaveChanges();
                Msg = "success";
            }

            catch { }

            var Modules = db.Modules_Property.Where(e => e.lang == language).ToList();
            ViewBag.Modules = Modules;


            ViewBag.Message = Msg;
            return View(oldVideoL);


        }



        //Remove VideLesson

        [HttpPost]
        public ActionResult removevlessons(int Id)
        {

            string Msg = "error";

            VideoL videols = db.VideoLs.Find(Id);

            try
            {

                var Tests = videols.Tests.ToList();
                db.Tests.RemoveRange(Tests);

                db.VideoLs.Remove(videols);

                VideoLEM VideoLEMObj = db.VideoLEMs.Where(e => e.VideoXId == videols.XId).First();

                db.VideoLEMs.Remove(VideoLEMObj);

                VideoXL VideoXLObj = db.VideoXLs.Where(e => e.XId == videols.XId).First();

                db.VideoXLs.Remove(VideoXLObj);

                db.SaveChanges();

                Msg = "success";

            }
            catch
            {

            }

            return Json(Msg);

        }

       
        //----------------------------------- Video Tests -----------------------------------------



        //Index
        public ActionResult VideoTests(string language, string Message)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();
            ViewBag.Message = Message;

            return View(VideoLessons);
        }


        //Create
        public ActionResult createvtests(string language)
        {
            if (language == null) { language = "ru"; }

            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();

            ViewBag.language = language;
            return View(VideoLessons);
        }



        [HttpPost]
        public ActionResult createvtests(Test test, string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;

            string Message = "success";

            if (ModelState.IsValid)
            {


                db.Tests.Add(test);
                db.SaveChanges();

            }
            else
            {

                Message = "error";
            }



            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();


            ViewBag.Message = Message;


            VideoL videoTest = VideoLessons.Where(e => e.Id == test.LessonId).FirstOrDefault();

            if (videoTest != null)
            {

                ViewBag.LessonId = test.LessonId;

            }


            return View(VideoLessons);
        }


        //Edit
        public ActionResult editvtests(int Id, string language)
        {
            if (language == null) { language = "ru"; }
            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();

            ViewBag.language = language;


            if (VideoLessons.Count > 0)
            {
                Test sTest = VideoLessons.FirstOrDefault().Tests.FirstOrDefault();

                ViewBag.Test = sTest;
            }


            Test test = db.Tests.Find(Id);

            ViewBag.VideoLessons = VideoLessons;

            return View(test);
        }



        [HttpPost]
        public ActionResult editvtests(Test test, string language)
        {
            if (language == null) { language = "ru"; }
            Test oldTest = db.Tests.Find(test.Id);

            ViewBag.language = language;

            string Msg = "success";

            try
            {
                oldTest.Question = test.Question;
                oldTest.A = test.A;
                oldTest.B = test.B;
                oldTest.C = test.C;
                oldTest.D = test.D;
                oldTest.E = test.E;
                oldTest.Answer = test.Answer;
                oldTest.LessonId = test.LessonId;
                db.SaveChanges();
            }
            catch { Msg = "error"; }




            var VideoLessons = db.VideoLs.Where(e => e.language == language).ToList();


            ViewBag.Message = Msg;
            ViewBag.VideoLessons = VideoLessons;



            return View(test);
        }

        //Delete
        public ActionResult removevtests(int Id, string language)
        {
            if (language == null) { language = "ru"; }
            string Msg = "error";

            Test test = db.Tests.Find(Id);
            VideoL videols = db.VideoLs.Find(test.LessonId);
            try
            {
                db.Tests.Remove(test);
                db.SaveChanges();
                Msg = "success";
            }
            catch { }
            return RedirectToAction("VideoTests", "portal", new { language = videols.language, Message = Msg });
        }

        public ActionResult buyers()
        {


            var buyers = db.User_Premium.Where(e => e.buy == true).OrderByDescending(e => e.Id).ToList();
            return View(buyers);
        }


        public ActionResult Quiz()
        {

          
            return View();
        }

        

        [HttpPost]
        public ActionResult adduserpremium(int Id, int premiumId)
        {


            string Msg = "success";

            try
            {

                UserInfo userinfo = db.UserInfoes.Find(Id);
                if (premiumId == 0)
                {
                    userinfo.LevelId = 0;

                    var UserPremium = userinfo.User_Premium.ToList();

                    foreach (var item in UserPremium)
                    {
                        item.buy = false;
                        
                    }


                    userinfo.ExpiredVLDate = null;
                    db.SaveChanges();
                    return Json(Msg);
                }


                User_Premium User_PremiumOBJ = userinfo.User_Premium.Where(e => e.PremiumId == premiumId).FirstOrDefault();
                Premium premium = db.Premiums.Find(premiumId);

                Premium_OrderId Premium_OrderIdOBJ = db.Premium_OrderId.First();

                Premium_OrderIdOBJ.OrderId += 1;


                if (User_PremiumOBJ == null)
                {

                    db.User_Premium.Add(new User_Premium
                    {

                        UserId = Id,
                        PremiumId = premiumId,
                        OrderId = Premium_OrderIdOBJ.OrderId,
                        Date = DateTime.Now,
                        buy = true
                    });
                    userinfo.LevelId = premium.LevelId;


                    if (userinfo.ExpiredVLDate == null || userinfo.Expired==true)
                    {
                        ExpiredTimeVL CompleteTime = db.ExpiredTimeVLs.Find(1);

                        DateTime CompleteVlDate = DateTime.Now.AddYears(CompleteTime.DefaultVLYear).AddMonths(CompleteTime.DefaultVLMonth).AddDays(CompleteTime.DefaultVLDay);


                        userinfo.ExpiredVLDate = CompleteVlDate;

                        userinfo.Expired = false;
                    }

                }
                else if (!User_PremiumOBJ.buy)
                {
                    User_PremiumOBJ.buy = true;
                    userinfo.LevelId = premium.LevelId;
                    if (userinfo.ExpiredVLDate == null || userinfo.Expired == true)
                    {
                        ExpiredTimeVL CompleteTime = db.ExpiredTimeVLs.Find(1);

                        DateTime CompleteVlDate = DateTime.Now.AddYears(CompleteTime.DefaultVLYear).AddMonths(CompleteTime.DefaultVLMonth).AddDays(CompleteTime.DefaultVLDay);


                        userinfo.ExpiredVLDate = CompleteVlDate;

                        userinfo.Expired = false;
                    }
                }
                db.SaveChanges();

            }
            catch
            {
                Msg = "error";
            }

            return Json(Msg);

        }



        [HttpPost]
        public ActionResult premium_sts(int Id, string sts_premium)
        {



            string Msg = "success";

            try
            {

                User_Premium User_PremiumOBJ = db.User_Premium.Find(Id);


                User_Premium_Status User_Premium_StatusOBJ = User_PremiumOBJ.User_Premium_Status.FirstOrDefault();

                if (User_Premium_StatusOBJ == null)
                {
                    User_Premium_Status newUser_Premium_Status = new User_Premium_Status();
                    newUser_Premium_Status.Status = sts_premium;
                    User_PremiumOBJ.User_Premium_Status.Add(newUser_Premium_Status);

                }
                else
                {

                    User_Premium_StatusOBJ.Status = sts_premium;
                }

                db.SaveChanges();



            }
            catch
            {
                Msg = "error";
            }

            return Json(Msg);

        }



        public JsonResult UsersQuiz(string language)
        {
            

            int oneId = db.Quizs.Find(1).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int twoId = db.Quizs.Find(2).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int threeId = db.Quizs.Find(3).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
          
            int fourId = db.Quizs.Find(4).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int fiveId = db.Quizs.Find(5).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int sixId = db.Quizs.Find(6).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int sevenId = db.Quizs.Find(7).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int eightId = db.Quizs.Find(8).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int nineId = db.Quizs.Find(9).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int tenId = db.Quizs.Find(10).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int elevenId = db.Quizs.Find(11).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int twelveId = db.Quizs.Find(12).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int thirteenId = db.Quizs.Find(13).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;
            int fourteenId = db.Quizs.Find(14).Quiz_Content.Where(e => e.language == language).FirstOrDefault().Id;



            var quiz_a_user_answer = db.Quiz_A_User_Answer.Where(e => e.QuizId == oneId).ToList();

            var userinfo = quiz_a_user_answer.Select(e => e.UserInfo).ToList();

            string[][] userquiz = new string[userinfo.Count][];

            int u = 0;
            foreach (var user in userinfo)
            {

                 string one=null, two=null, three=null, four=null, five=null, six=null,seven = null,eight=null,nine=null,ten=null,eleven=null,twelve=null,thirteen=null,fourteen=null;


                try
                {
                    Quiz_A_User_Answer quiz_a_user_onea = user.Quiz_A_User_Answer.Where(e => e.QuizId == oneId).FirstOrDefault();
                    Quiz_A_User_Answer quiz_a_user_twoa = user.Quiz_A_User_Answer.Where(e => e.QuizId == twoId).FirstOrDefault();
                    Quiz_B_User_Answer quiz_b_user_threea = user.Quiz_B_User_Answer.Where(e => e.QuizId == threeId).FirstOrDefault();
                    Quiz_B_User_Answer quiz_b_user_foura = user.Quiz_B_User_Answer.Where(e => e.QuizId == fourId).FirstOrDefault();
                    Quiz_B_User_Answer quiz_b_user_fivea = user.Quiz_B_User_Answer.Where(e => e.QuizId == fiveId).FirstOrDefault();
                    Quiz_B_User_Answer quiz_b_user_sixa = user.Quiz_B_User_Answer.Where(e => e.QuizId == sixId).FirstOrDefault();
                    Quiz_B_User_Answer quiz_b_user_sevena = user.Quiz_B_User_Answer.Where(e => e.QuizId == sevenId).FirstOrDefault();
                    Quiz_B_User_Answer quiz_b_user_eighta = user.Quiz_B_User_Answer.Where(e => e.QuizId == eightId).FirstOrDefault();
                    Quiz_B_User_Answer quiz_b_user_ninea = user.Quiz_B_User_Answer.Where(e => e.QuizId == nineId).FirstOrDefault();

                    Quiz_C_User_Answer quiz_c_user_tena = user.Quiz_C_User_Answer.Where(e => e.QuizId == tenId).FirstOrDefault();
                    Quiz_C_User_Answer quiz_c_user_elevena = user.Quiz_C_User_Answer.Where(e => e.QuizId == elevenId).FirstOrDefault();
                    Quiz_C_User_Answer quiz_c_user_twelvea = user.Quiz_C_User_Answer.Where(e => e.QuizId == twelveId).FirstOrDefault();
                    Quiz_C_User_Answer quiz_c_user_thirteena = user.Quiz_C_User_Answer.Where(e => e.QuizId == thirteenId).FirstOrDefault();
                    Quiz_C_User_Answer quiz_c_user_fourteena = user.Quiz_C_User_Answer.Where(e => e.QuizId == fourteenId).FirstOrDefault();

                    Quiz_Option quiz_option_f = db.Quiz_Option.Where(e => e.QuizId == oneId).FirstOrDefault();
                    Quiz_Option quiz_option_t = db.Quiz_Option.Where(e => e.QuizId == twoId).FirstOrDefault();

                    switch (quiz_a_user_onea.Answer)
                    {
                        case 1:
                            quiz_a_user_onea.AnswerOther = quiz_option_f.A;
                            break;
                        case 2:
                            quiz_a_user_onea.AnswerOther = quiz_option_f.B;
                            break;
                        case 3:
                            quiz_a_user_onea.AnswerOther = quiz_option_f.C;
                            break;
                    }

                    switch (quiz_a_user_twoa.Answer)
                    {
                        case 1:
                            quiz_a_user_twoa.AnswerOther = quiz_option_t.A;
                            break;
                        case 2:
                            quiz_a_user_twoa.AnswerOther = quiz_option_t.B;
                            break;
                        case 3:
                            quiz_a_user_twoa.AnswerOther = quiz_option_t.C;
                            break;
                        case 4:
                            quiz_a_user_twoa.AnswerOther = quiz_option_t.D;
                            break;
                    }


                    one = quiz_a_user_onea != null ? quiz_a_user_onea.Answer.ToString() + ". " + quiz_a_user_onea.AnswerOther:null;
                    two = quiz_a_user_twoa != null ? quiz_a_user_twoa.Answer.ToString() + ". " + quiz_a_user_twoa.AnswerOther : null;

                  

                    three = quiz_b_user_threea != null ? quiz_b_user_threea.Answer.ToString() : null;
                    four = quiz_b_user_foura != null ? quiz_b_user_foura.Answer.ToString() : null;
                    five = quiz_b_user_fivea != null ? quiz_b_user_fivea.Answer.ToString() : null;
                    six = quiz_b_user_sixa != null ? quiz_b_user_sixa.Answer.ToString() : null;
                    seven = quiz_b_user_sevena != null ? quiz_b_user_sevena.Answer.ToString() : null;
                    eight = quiz_b_user_eighta != null ? quiz_b_user_eighta.Answer.ToString() : null;
                    nine = quiz_b_user_ninea != null ? quiz_b_user_ninea.Answer.ToString() : null;


                    ten = quiz_c_user_tena != null ? quiz_c_user_tena.Answer : null;
                    eleven = quiz_c_user_elevena != null ? quiz_c_user_elevena.Answer : null;
                    twelve = quiz_c_user_twelvea != null ? quiz_c_user_twelvea.Answer : null;
                    thirteen = quiz_c_user_thirteena != null ? quiz_c_user_thirteena.Answer : null;
                    fourteen = quiz_c_user_fourteena != null ? quiz_c_user_fourteena.Answer : null;

                }
                catch { }
                
                userquiz[u] = new string[] {user.UserName,user.Name, one, two, three, four, five, six, seven, eight, nine, ten,eleven, twelve, thirteen, fourteen};
                u++;
                
            }

            userquiz = userquiz.Where(e => e != null).ToArray();

            return Json(userquiz, JsonRequestBehavior.AllowGet);
        }




        //----------------------------------- SetCompleteTimeVL -----------------------------------------
        public ActionResult SetExpiredTimeVL()
        {

            ExpiredTimeVL completeTime = db.ExpiredTimeVLs.Find(1);
            return View(completeTime);
        }



        
        [HttpPost]
        public ActionResult SetExpiredTimeVL(ExpiredTimeVL data)
        {

            string message = "success";

            try
            {
                db.Entry(data).State = EntityState.Modified;

                db.SaveChanges();
            }
            catch {
                message = "error";
            }

            ViewBag.AddVideoMsg = message;


            return View(data);
        }



        [HttpPost]
        public ActionResult SetExpiredTR(int Id,string data,bool set)
        {

            string Msg = "error";

            UserInfo userinfo = db.UserInfoes.Find(Id);

            try
            {

                if (set == true)
                {
                    DateTime newData = Convert.ToDateTime(data);
                    userinfo.ExpiredVLDate = newData;
                }
                else {
                    userinfo.ExpiredVLDate = null;
                }

                db.SaveChanges();
                Msg = "success";


            }
            catch
            {

            }

            return Json(Msg);

        }


    }



}