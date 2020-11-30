using OnlineCourseZertis.Models;
using System;
using System.Collections.Generic;
using NReco.ImageGenerator;
using System.Linq;
using System.Web.Mvc;

using OnlineCourseZertis.App_Start;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OnlineCourseZertis.Controllers
{
    [Authorize]
    public class LearningController : Controller
    {
        private Entities db = new Entities();

        public string GetUserName()
        {

            return User.Identity.Name;
        }



        // GET: MODULES--------------------------------------------------------------------
        public async Task<ActionResult> Modules(string language)
        {

            try
            {

                if (language == null) { language = "ru"; }
                string UserName = GetUserName();



                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                //-!!!- User Progress If you create every action then you need take this Progress modul 
                UserProgress UserProgress = UserGet.Progress(UserName, userinfo.LevelId);
                JVLO jv = db.JVLOes.Where(e => e.UserName == UserName).FirstOrDefault();
                Users_Certificates userCert = userinfo.Users_Certificates.FirstOrDefault();


                // Get User Modules
                var Modules = await db.Moduls.ToListAsync();
                var Modules_Property_Enable_List = new List<Modules_Property_Enable>();


                foreach (var Modul in Modules)
                {
                    Modul_userLevel Modul_userLevelOBJ = Modul.Modul_userLevel.FirstOrDefault();
                    Modules_Property_Enable Modules_Property_EnableOBJ = new Modules_Property_Enable();
                    Modules_Property MP = Modul.Modules_Property.Where(e => e.lang == language).FirstOrDefault();
                    if (MP == null)
                    {
                        MP = Modul.Modules_Property.Where(e => e.lang == "ru").FirstOrDefault();
                    }

                    if (Modul_userLevelOBJ.LevelId <= userinfo.LevelId)
                    {

                        Modules_Property_EnableOBJ.Modul_Property_IN = MP;
                        Modules_Property_EnableOBJ.Enable = true;

                    }
                    else
                    {
                        Modules_Property_EnableOBJ.Modul_Property_IN = MP;
                        Modules_Property_EnableOBJ.Enable = false;
                    }


                    Modules_Property_Enable_List.Add(Modules_Property_EnableOBJ);
                }

                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.language = language;
                return View(Modules_Property_Enable_List);
            }
            catch
            {

            }

            return RedirectToAction("ErrorLogOff", "Account", new { language = language });
        }


        // GET: VIDEOS---------------------------------------------------------------------
        public async Task<ActionResult> Videos(int Id, string language)
        {


            try
            {

                if (language == null) { language = "ru"; }
                string UserName = GetUserName();


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                bool ModulNot = false;
                var Modules = await db.Moduls.FindAsync(Id);

                if (Modules == null || Modules.Enable == false)
                {
                    ModulNot = true;
                }
                else
                {

                    int ModulLevel = Modules.Modul_userLevel.FirstOrDefault().LevelId;
                    if (ModulLevel > userinfo.LevelId)
                    {
                        ModulNot = true;
                    }

                }


                if (ModulNot)
                {
                    return RedirectToAction("Noaccess", "Error");
                }


                bool EmptyLessons = false;


                //-!!!- User Progress If you create every action then you need take this Progress modul 
                UserProgress UserProgress = UserGet.Progress(UserName, userinfo.LevelId);
                JVLO jv = UserProgress.JVLO;
                int NextV = UserProgress.NextV;
                var lessons = new List<VideoL>();
                int? DisVID = null;



                if (jv == null)
                {


                    EmptyLessons = true;
                }


                if (!EmptyLessons)
                {
                    VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == jv.X && e.language == language);
                    if (videols == null)
                    {
                        videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == jv.X && e.language == "ru");
                    }

                    DisVID = videols.Id;


                    var VideoLsRu = Modules.VideoLs.Where(e => e.language == "ru" && e.VideoXL.Enable == true).ToList();

                    if (VideoLsRu.Count == 0)
                    {
                        EmptyLessons = true;
                    }



                    if (!EmptyLessons)
                    {

                        foreach (var item in VideoLsRu)
                        {
                            VideoL videol = Modules.VideoLs.Where(e => e.language == language && e.XId == item.XId && e.VideoXL.Enable == true).FirstOrDefault();
                            if (videol == null)
                            {
                                lessons.Add(item);
                            }
                            else
                            {
                                lessons.Add(videol);
                            }

                        }

                        lessons = lessons.OrderBy(e => e.XId).ToList();

                    }

                }







                Modules_Property Modules_Property = Modules.Modules_Property.Where(e => e.lang == language).FirstOrDefault();
                if (Modules_Property == null)
                    Modules_Property = Modules.Modules_Property.Where(e => e.lang == "ru").FirstOrDefault();



                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.ModulName = Modules_Property.Titile;
                ViewBag.JVX = jv.X;
                ViewBag.NextV = NextV;
                ViewBag.DisVID = DisVID;
                ViewBag.language = language;

                return View(lessons);
            }
            catch { }
            return RedirectToAction("ErrorLogOff", "Account", new { language = language });

        }

        // GET: VIDEOLESSON----------------------------------------------------------------
        public async Task<ActionResult> Videolesson(int Id, string language)
        {

            try
            {
                string UserName = GetUserName();
                if (language == null) { language = "ru"; }

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = UserGet.Progress(UserName, userinfo.LevelId);
                JVLO jv = UserProgress.JVLO;



                VideoL videolsForXid = await db.VideoLs.FindAsync(Id);

                VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == videolsForXid.XId && e.language == language);
                if (videols == null)
                {
                    videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == videolsForXid.XId && e.language == "ru");
                }

                if (videols != null && videols.VideoXL.Enable == true)
                {
                    int? LevelId = db.Modul_userLevel.FirstOrDefault(e => e.ModuleId == videols.ModulId).LevelId;

                    if (userinfo.LevelId < LevelId)
                    {
                        return RedirectToAction("Noaccess", "Error");
                    }


                    if (videols.XId <= jv.X)
                    {

                        LikeV lkv = userinfo.LikeVs.FirstOrDefault(e => e.VideoXId == videols.XId);
                        if (lkv != null)
                        { ViewBag.lkv = 1; }


                        Isbranni izb = userinfo.Isbrannis.FirstOrDefault(e => e.VideoLXId == videols.XId);
                        if (izb != null)
                        { ViewBag.izb = 1; }


                        var vxl = UserProgress.EnableVideoXLs.ToList();
                        VideoXL vlXID = vxl.Where(e => e.XId < videols.XId).OrderByDescending(e => e.XId).FirstOrDefault();
                        VideoXL vnXID = vxl.Where(e => e.XId > videols.XId).OrderBy(e => e.XId).FirstOrDefault();


                        double LastVXId = UserProgress.EnableVideoXLs.Max(e => e.XId);


                        int Quiz = 0;

                        VideoL vl = null;
                        VideoL vn = null;


                        if (vlXID != null)
                        {
                            vl = db.VideoLs.FirstOrDefault(e => e.XId == vlXID.XId && e.language == language);
                            if (vl == null)
                                vl = db.VideoLs.FirstOrDefault(e => e.XId == vlXID.XId && e.language == "ru");
                        }
                        if (vnXID != null)
                        {
                            vn = db.VideoLs.FirstOrDefault(e => e.XId == vnXID.XId && e.language == language);
                            if (vn == null)
                                vn = db.VideoLs.FirstOrDefault(e => e.XId == vnXID.XId && e.language == "ru");
                        }





                        //bool VideoTest = false;
                        bool FulllookL = false;
                        //string GetCerURL = null;
                        //int CertStatus = 0;



                        int NextV = 0;


                        var test = videols.Tests.ToList();





                        if (test.Count() > 0)
                        {
                            //VideoTest = true;
                            NextV = 1;
                            ValitO vo = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == videols.XId);
                            if (vo != null)
                            {
                                if (vo.KB >= 75)
                                {
                                    NextV = 0;

                                }
                            }
                        }



                        LessonVideoTime LessonVideoTime = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == videols.XId);
                        if (LessonVideoTime != null)
                        {
                            if (LessonVideoTime.Status == true)
                            {
                                FulllookL = true;
                            }
                        }

                        if (vn != null && NextV == 0)
                        {

                            NextV = 3;
                        }



                        if (jv.X >= LastVXId && NextV == 0)
                        {
                            NextV = 4;

                            if (LastVXId > 2)
                            {
                                NextV = 5;
                            }


                        }



                        if (videols.XId == LastVXId)
                        {
                            //Users_Certificates UserCert = userinfo.Users_Certificates.FirstOrDefault();
                            //if (UserCert == null)
                            //{

                            //    GetCerURL = MakeCertificate(UserName, false, "ru");
                            //}
                            //else
                            //{

                            //    if (!UserCert.PublicCert)
                            //    {
                            //        GetCerURL = UserCert.CertificateURL;
                            //    }

                            //}

                            //if (!VideoTest)
                            //{

                            //    if (FulllookL == false)
                            //    {

                            //        CertStatus = 1;


                            //    }

                            //}

                        }




                        if (vl != null)
                        { ViewBag.LastVId = vl.Id; }

                        if (vn != null)
                        { ViewBag.NextVId = vn.Id; }


                        ViewBag.OUK = UserProgress.OUK;
                        ViewBag.TBB = UserProgress.TBB;
                        ViewBag.NextV = NextV;
                        ViewBag.FullookL = FulllookL;
                        //ViewBag.CertStatus = CertStatus;
                        //ViewBag.CerURL = GetCerURL;
                        ViewBag.language = language;

                        return View(videols);
                    }

                }

                return RedirectToAction("Noaccess", "Error", new { language = language });
            }
            catch { }

            return RedirectToAction("ErrorLogOff", "Account", new { language = language });

        }


        // GET: TEST-----------------------------------------------------------------------
        public async Task<ActionResult> Test(int Id, string language)
        {

            try
            {


                string UserName = GetUserName();
                if (language == null) { language = "ru"; }


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = UserGet.Progress(UserName, userinfo.LevelId);
                JVLO jv = UserProgress.JVLO;


                bool error = false;

                VideoL videols = await db.VideoLs.FindAsync(Id);

                if (videols == null || videols.VideoXL.Enable == false)
                {
                    error = true;
                }
                else
                {
                    int LevelId = db.Modul_userLevel.FirstOrDefault(e => e.ModuleId == videols.ModulId).LevelId;
                    if (userinfo.LevelId < LevelId)
                        error = true;
                }

                if (error)
                    return RedirectToAction("Noaccess", "Error");




                if (videols.XId <= jv.X)
                {
                    bool FulllookL = false;
                    LessonVideoTime LessonVideoTime = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == videols.XId);
                    if (LessonVideoTime != null)
                    {
                        if (LessonVideoTime.Status == true)
                        {
                            FulllookL = true;
                        }
                    }

                    if (FulllookL)
                    {


                        var tst = videols.Tests.ToList();

                        if (tst.Count > 0)
                        {

                            int? NextId = null;

                            VideoXL NextVideoXL = UserProgress.EnableVideoXLs.Where(e => e.XId > videols.XId).FirstOrDefault();
                            if (NextVideoXL != null)
                            {
                                VideoL NextVideo = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == NextVideoXL.XId && e.language == language);
                                if (NextVideo == null)
                                {
                                    NextVideo = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == NextVideoXL.XId && e.language == "ru");
                                }
                                if (NextVideo != null)
                                    NextId = NextVideo.Id;
                            }


                            Test gettest = tst.FirstOrDefault();

                            List<string> testOptions = new List<string>();

                            string answer = "";
                            Random random = new Random();
                            int idAnswer = random.Next(4);


                            if (gettest.Answer == 1)
                            {
                                answer = gettest.A;

                            }
                            else
                            {
                                testOptions.Add(gettest.A);
                            }

                            if (gettest.Answer == 2)
                            {
                                answer = gettest.B;
                            }
                            else
                            {
                                testOptions.Add(gettest.B);

                            }


                            if (gettest.Answer == 3)
                            {
                                answer = gettest.C;
                            }
                            else
                            {
                                testOptions.Add(gettest.C);
                            }


                            if (gettest.Answer == 4)
                            {
                                answer = gettest.D;
                            }
                            else
                            {
                                testOptions.Add(gettest.D);
                            }
                            if (gettest.Answer == 5)
                            {
                                answer = gettest.E;
                            }
                            else
                            {
                                testOptions.Add(gettest.E);
                            }





                            var RanDomTest = testOptions.OrderBy(a => Guid.NewGuid()).ToList();

                            RanDomTest.Insert(idAnswer, answer);

                            Test rntest = new Test();
                            rntest.Id = gettest.Id;
                            rntest.Question = gettest.Question;
                            rntest.LessonId = gettest.LessonId;
                            rntest.A = RanDomTest[0];
                            rntest.B = RanDomTest[1];
                            rntest.C = RanDomTest[2];
                            rntest.D = RanDomTest[3];
                            rntest.E = RanDomTest[4];
                            rntest.Answer = idAnswer + 1;



                            int Quiz = 0;
                            double LastVXId = UserProgress.EnableVideoXLs.Max(e => e.XId);

                            if (jv.X >= LastVXId && LastVXId > 2)
                            {

                                Quiz = 1;
                            }





                            ////int CertStatus = 0;
                            //string GetCerURL = null;


                            //if (videols.XId == LastVXId && LastVXId > 2)
                            //{
                            //    Users_Certificates UserCert = userinfo.Users_Certificates.FirstOrDefault();
                            //    if (UserCert != null)
                            //    {
                            //        if (UserCert.PublicCert != true)
                            //        {
                            //            //CertStatus = 1;
                            //            GetCerURL = UserCert.CertificateURL;

                            //        }

                            //    }

                            //}

                            ViewBag.OUK = UserProgress.OUK;
                            ViewBag.TBB = UserProgress.TBB;
                            ViewBag.VideoName = gettest.VideoL.Name;
                            ViewBag.IdModul = tst.FirstOrDefault().VideoL.ModulId;
                            ViewBag.CountTest = tst.Count;
                            ViewBag.Ids = NextId;
                            ViewBag.FullookL = FulllookL;
                            ViewBag.Quiz = Quiz;
                            //ViewBag.CertStatus = CertStatus;
                            //ViewBag.CerURL = GetCerURL;
                            ViewBag.language = language;


                            return View(rntest);

                        }




                    }
                }

            }
            catch
            {
                return RedirectToAction("ErrorLogOff", "Account", new { language = language });
            }




            return RedirectToAction("Noaccess", "Error", new { language = language });

        }

        public async Task<ActionResult> TestGet(int Id, int LessonId)
        {

            try
            {
                var Tests = await db.Tests.Where(e => e.LessonId == LessonId).ToListAsync();
                Test gettest = Tests.FirstOrDefault(e => e.Id > Id);

                List<string> testOptions = new List<string>();

                string answer = "";
                Random random = new Random();
                int idAnswer = random.Next(4);


                if (gettest.Answer == 1)
                {
                    answer = gettest.A;

                }
                else
                {
                    testOptions.Add(gettest.A);
                }

                if (gettest.Answer == 2)
                {
                    answer = gettest.B;
                }
                else
                {
                    testOptions.Add(gettest.B);

                }


                if (gettest.Answer == 3)
                {
                    answer = gettest.C;
                }
                else
                {
                    testOptions.Add(gettest.C);
                }


                if (gettest.Answer == 4)
                {
                    answer = gettest.D;
                }
                else
                {
                    testOptions.Add(gettest.D);
                }
                if (gettest.Answer == 5)
                {
                    answer = gettest.E;
                }
                else
                {
                    testOptions.Add(gettest.E);
                }





                var RanDomTest = testOptions.OrderBy(a => Guid.NewGuid()).ToList();

                RanDomTest.Insert(idAnswer, answer);

                Test rntest = new Test();
                rntest.Id = gettest.Id;
                rntest.Question = gettest.Question;
                rntest.LessonId = gettest.LessonId;
                rntest.A = RanDomTest[0];
                rntest.B = RanDomTest[1];
                rntest.C = RanDomTest[2];
                rntest.D = RanDomTest[3];
                rntest.E = RanDomTest[4];
                rntest.Answer = idAnswer + 1;

                ViewBag.VideoName = gettest.VideoL.Name;
                return View(rntest);
            }
            catch
            {
            }

            return RedirectToAction("ErrorLogOff", "Account");
        }


        public async Task<ActionResult> Useful_services(string language)
        {

            try
            {
                string UserName = GetUserName();

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                if (language == null) { language = "ru"; }



                //-!!!- User Progress If you create every action then you need take this Progress modul 
                UserProgress UserProgress = UserGet.Progress(UserName, userinfo.LevelId);

                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.language = language;

                var User_Premium = userinfo.User_Premium.Where(e => e.buy == true).ToList();

                return View(User_Premium);
            }
            catch
            {

            }


            return RedirectToAction("ErrorLogOff", "Account", new { language = language });
        }


        public async Task<ActionResult> AllVideos(string language)
        {
            try
            {


                string UserName = GetUserName();
                if (language == null) { language = "ru"; }


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = UserGet.Progress(UserName, userinfo.LevelId);
                JVLO jv = UserProgress.JVLO;


                VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == jv.X && e.language == language);
                if (videols == null)
                {
                    videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == jv.X && e.language == "ru");
                }

                int NextV = UserProgress.NextV;



                var EnableVideoLs = new List<VideoL>();

                foreach (var videoXL in UserProgress.EnableVideoXLs)
                {
                    VideoL addVideoL = videoXL.VideoLs.FirstOrDefault(e => e.language == language);
                    if (language != "ru" && addVideoL == null)
                    {
                        addVideoL = videoXL.VideoLs.FirstOrDefault(e => e.language == "ru");
                    }
                    if (addVideoL != null)
                    {

                        EnableVideoLs.Add(addVideoL);
                    }
                }




                int Quiz = 0;
                double LastVXId = UserProgress.EnableVideoXLs.Max(e => e.XId);



                if (jv.X >= LastVXId && NextV == 0 && LastVXId > 2)
                {

                    Quiz = 1;
                }


                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                ViewBag.language = language;
                ViewBag.JVX = jv.X;
                ViewBag.NextV = NextV;
                ViewBag.DisVId = videols.Id;
                ViewBag.Quiz = Quiz;

                EnableVideoLs = EnableVideoLs.OrderBy(e => e.XId).ToList();


                return View(EnableVideoLs);


            }
            catch
            { }

            return RedirectToAction("ErrorLogOff", "Account", new { language = language });

        }


        public async Task<ActionResult> Quiz(string language)
        {

            try
            {

                string UserName = GetUserName();

                if (language == null) { language = "ru"; }

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == User.Identity.Name);


                UserProgress UserProgress = UserGet.Progress(UserName, userinfo.LevelId);


                double LastVXId = UserProgress.EnableVideoXLs.Max(e => e.XId);


                VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == LastVXId && e.language == language);

                var test = videols.Tests;


                int TestE = 0;

                if (test.Count > 0)
                {
                    TestE = 1;

                    ValitO vo = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == LastVXId);
                    if (vo != null)
                    {
                        if (vo.KB >= 75)
                        {
                            TestE = 0;

                        }
                    }
                }


                if (jv.X >= LastVXId && LastVXId > 2 && TestE == 0)
                {


                    int CertStatus = 0;
                    string GetCerURL = null;

                    Users_Certificates UserCert = userinfo.Users_Certificates.FirstOrDefault();
                    if (UserCert == null)
                    {

                        GetCerURL = await MakeCertificate(UserName, true, "ru");
                        CertStatus = 1;

                    }
                    else
                    {

                        if (!UserCert.PublicCert)
                        {
                            CertStatus = 1;
                            GetCerURL = UserCert.CertificateURL;

                        }

                    }


                    Quiz quiz = db.Quizs.Find(1);




                    Quiz_Content first_quiz_content = quiz.Quiz_Content.FirstOrDefault(e => e.language == language);

                    Quiz_A_User_Answer first_quiz_a_user_answer = userinfo.Quiz_A_User_Answer.FirstOrDefault(e => e.QuizId == first_quiz_content.Id);

                    Quiz_Option first_quiz_option = first_quiz_content.Quiz_Option.FirstOrDefault();


                    Quiz_A quiz_a = new Quiz_A();


                    int answer = first_quiz_a_user_answer == null ? 0 : first_quiz_a_user_answer.Answer;
                    string answerother = first_quiz_a_user_answer == null ? "" : first_quiz_a_user_answer.AnswerOther;



                    quiz_a.QId = 1;
                    quiz_a.Q_content_Id = first_quiz_content.Id;
                    quiz_a.Quiz_Title = first_quiz_content.Title;

                    quiz_a.User_Answer = answer;
                    quiz_a.User_AnswerOther = answerother;

                    quiz_a.A = first_quiz_option.A;
                    quiz_a.B = first_quiz_option.B;
                    quiz_a.C = first_quiz_option.C;
                    quiz_a.D = first_quiz_option.D;
                    quiz_a.E = first_quiz_option.E;




                    ViewBag.CertStatus = CertStatus;
                    ViewBag.CerURL = GetCerURL;
                    ViewBag.OUK = UserProgress.OUK;
                    ViewBag.TBB = UserProgress.TBB;
                    ViewBag.language = language;

                    return View(quiz_a);

                }

                return RedirectToAction("Noaccess", "Error", new { language = language });
            }
            catch { }


            return RedirectToAction("ErrorLogOff", "Account", new { language = language });

        }



        [HttpPost]
        public async Task<ActionResult> quiz_a_user_answer(int Q_content_Id, int answer, string answerOther)
        {

            string UserName = GetUserName();

            string Msg = "success";

            try
            {
                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                Quiz_A_User_Answer Quiz_A_answer = userinfo.Quiz_A_User_Answer.Where(e => e.QuizId == Q_content_Id).FirstOrDefault();

                if (Quiz_A_answer != null)
                {

                    Quiz_A_answer.Answer = answer;

                    Quiz_A_answer.AnswerOther = answerOther;



                }
                else
                {

                    Quiz_A_User_Answer newQuiz_A_answer = new Quiz_A_User_Answer();

                    newQuiz_A_answer.QuizId = Q_content_Id;
                    newQuiz_A_answer.UserId = userinfo.Id;

                    newQuiz_A_answer.Answer = answer;

                    newQuiz_A_answer.AnswerOther = answerOther;



                    db.Quiz_A_User_Answer.Add(newQuiz_A_answer);

                }

                await db.SaveChangesAsync();



            }
            catch
            {
                Msg = "error";
            }

            return Json(Msg);

        }



        [HttpPost]
        public async Task<ActionResult> quiz_b_user_answer(int Q_content_Id, int answer)
        {

            string UserName = GetUserName();

            string Msg = "success";

            try
            {
                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                Quiz_B_User_Answer Quiz_B_answer = userinfo.Quiz_B_User_Answer.FirstOrDefault(e => e.QuizId == Q_content_Id);

                if (Quiz_B_answer != null)
                {

                    Quiz_B_answer.Answer = answer;


                }
                else
                {

                    Quiz_B_User_Answer newQuiz_B_answer = new Quiz_B_User_Answer();

                    newQuiz_B_answer.QuizId = Q_content_Id;
                    newQuiz_B_answer.UserId = userinfo.Id;

                    newQuiz_B_answer.Answer = answer;


                    db.Quiz_B_User_Answer.Add(newQuiz_B_answer);

                }

                await db.SaveChangesAsync();



            }
            catch
            {
                Msg = "error";
            }

            return Json(Msg);

        }


        [HttpPost]
        public async Task<ActionResult> quiz_c_user_answer(int Q_content_Id, string answer)
        {

            string UserName = GetUserName();

            string Msg = "success";

            try
            {
                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                Quiz_C_User_Answer Quiz_C_answer = userinfo.Quiz_C_User_Answer.FirstOrDefault(e => e.QuizId == Q_content_Id);

                if (Quiz_C_answer != null)
                {

                    Quiz_C_answer.Answer = answer;


                }
                else
                {

                    Quiz_C_User_Answer newQuiz_C_answer = new Quiz_C_User_Answer();

                    newQuiz_C_answer.QuizId = Q_content_Id;
                    newQuiz_C_answer.UserId = userinfo.Id;

                    newQuiz_C_answer.Answer = answer;


                    db.Quiz_C_User_Answer.Add(newQuiz_C_answer);

                }

                await db.SaveChangesAsync();



            }
            catch
            {
                Msg = "error";
            }

            return Json(Msg);

        }



        public async Task<ActionResult> QuizAGet(double QId, string lang, int prevnext)
        {

            string UserName = GetUserName();

            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


            Quiz quiz = new Quiz();

            bool prev = true;

            if (prevnext == 1)
            {

                quiz = db.Quizs.FirstOrDefault(e => e.QId > QId);
                prev = true;
            }
            else
            {

                quiz = db.Quizs.Where(e => e.QId < QId).OrderByDescending(e => e.QId).FirstOrDefault();
            }


            if (quiz.QId == 1)
            {

                prev = false;
            }

            Quiz_Content quiz_content = quiz.Quiz_Content.FirstOrDefault(e => e.language == lang);

            Quiz_A_User_Answer quiz_a_user_answer = userinfo.Quiz_A_User_Answer.FirstOrDefault(e => e.QuizId == quiz_content.Id);

            Quiz_Option quiz_option = quiz_content.Quiz_Option.FirstOrDefault();



            int answer = quiz_a_user_answer == null ? 0 : quiz_a_user_answer.Answer;
            string answerother = quiz_a_user_answer == null ? "" : quiz_a_user_answer.AnswerOther;

            Quiz_A quiz_a = new Quiz_A();

            quiz_a.QId = quiz.QId;
            quiz_a.Q_content_Id = quiz_content.Id;
            quiz_a.Quiz_Title = quiz_content.Title;
            quiz_a.User_Answer = answer;
            quiz_a.User_AnswerOther = answerother;
            quiz_a.A = quiz_option.A;
            quiz_a.B = quiz_option.B;
            quiz_a.C = quiz_option.C;
            quiz_a.D = quiz_option.D;
            quiz_a.E = quiz_option.E;
            quiz_a.prev = prev;
            ViewBag.language = lang;


            return View(quiz_a);
        }


        public async Task<ActionResult> QuizBGet(double QId, string lang, int prevnext)
        {
            string UserName = GetUserName();

            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


            Quiz quiz = new Quiz();

            if (prevnext == 1)
            {

                quiz = await db.Quizs.FirstOrDefaultAsync(e => e.QId > QId);
            }
            else
            {

                quiz = db.Quizs.Where(e => e.QId < QId).OrderByDescending(e => e.QId).FirstOrDefault();
            }


            Quiz_Content quiz_content = quiz.Quiz_Content.Where(e => e.language == lang).FirstOrDefault();

            Quiz_B_User_Answer quiz_b_user_answer = userinfo.Quiz_B_User_Answer.Where(e => e.QuizId == quiz_content.Id).FirstOrDefault();





            int answer = quiz_b_user_answer == null ? 0 : quiz_b_user_answer.Answer;

            Quiz_B quiz_b = new Quiz_B();

            quiz_b.QId = quiz.QId;
            quiz_b.Q_content_Id = quiz_content.Id;
            quiz_b.Quiz_Title = quiz_content.Title;
            quiz_b.User_Answer = answer;
            ViewBag.language = lang;


            return View(quiz_b);
        }


        public async Task<ActionResult> QuizCGet(double QId, string lang, int prevnext)
        {
            string UserName = GetUserName();

            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


            Quiz quiz = new Quiz();

            bool next = true;

            if (prevnext == 1)
            {

                quiz = db.Quizs.FirstOrDefault(e => e.QId > QId);
            }
            else
            {

                quiz = db.Quizs.Where(e => e.QId < QId).OrderByDescending(e => e.QId).FirstOrDefault();

            }

            if (quiz.QId == 14)
            {

                next = false;
            }


            Quiz_Content quiz_content = quiz.Quiz_Content.Where(e => e.language == lang).FirstOrDefault();

            Quiz_C_User_Answer quiz_c_user_answer = userinfo.Quiz_C_User_Answer.Where(e => e.QuizId == quiz_content.Id).FirstOrDefault();





            string answer = quiz_c_user_answer == null ? "" : quiz_c_user_answer.Answer;

            Quiz_C quiz_c = new Quiz_C();

            quiz_c.QId = quiz.QId;
            quiz_c.Q_content_Id = quiz_content.Id;
            quiz_c.Quiz_Title = quiz_content.Title;
            quiz_c.User_Answer = answer;
            quiz_c.next = next;
            ViewBag.language = lang;



            return View(quiz_c);
        }


        public ActionResult Quiz_Result(string language)
        {

            return RedirectToAction("certificates", "User", new { language = language });

        }



        public async Task<JsonResult> TestV(int id, int KB, string language)
        {

            try
            {
                string UserName = GetUserName();
                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                VideoL videols = await db.VideoLs.FindAsync(id);

                if (language == null) { language = "ru"; }



                var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= userinfo.LevelId).ToListAsync();
                var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();
                var EnableVideoXLs = new List<VideoXL>();
                foreach (var Module in Moduls)
                {
                    EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                }



                ValitO vo = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == videols.XId);
                JVLO jv = db.JVLOes.FirstOrDefault(e => e.UserName == UserName);


                VideoXL NextVideoXL = EnableVideoXLs.Where(e => e.XId > videols.XId).OrderBy(e => e.XId).FirstOrDefault();


                if (NextVideoXL != null)
                {


                    if (KB > 74)
                    {

                        if (NextVideoXL.XId > jv.X)
                        {
                            jv.X = NextVideoXL.XId;
                        }
                    }



                }




                if (vo != null)
                {
                    if (KB > vo.KB)
                    {
                        vo.KB = KB;
                    }

                }
                else if (vo == null)
                {
                    ValitO vonew = new ValitO();
                    vonew.KB = KB;
                    vonew.UserId = userinfo.Id;
                    vonew.VdeoLXId = videols.XId;
                    db.ValitOS.Add(vonew);

                }

                await db.SaveChangesAsync();

                int OB = 0;

                var valitos = userinfo.ValitOS.ToList();
                if (valitos.Count > 0)
                {
                    foreach (var item in userinfo.ValitOS.ToList())
                    {
                        OB += item.KB;
                    }

                    jv.TBB = OB / valitos.Count;
                    await db.SaveChangesAsync();
                }
            }
            catch
            {


            }
            return Json("");

        }
        public async Task<JsonResult> like(int id, int tf)
        {
            try
            {
                VideoL videols = await db.VideoLs.FindAsync(id);
                VideoLEM videolem = videols.VideoXL.VideoLEMs.FirstOrDefault();

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                if (tf == 1)
                {
                    videolem.Likes += 1;
                    LikeV lkv = new LikeV();
                    lkv.UserId = userinfo.Id;
                    lkv.VideoXId = videols.XId;
                    db.LikeVs.Add(lkv);
                }
                else
                {
                    videolem.Likes -= 1;
                    LikeV lkv = userinfo.LikeVs.FirstOrDefault(e => e.VideoXId == videols.XId);
                    db.LikeVs.Remove(lkv);

                }
                await db.SaveChangesAsync();
            }
            catch { }
            return Json("");
        }


        public async Task<JsonResult> Addeye(int id)
        {
            try
            {
                VideoL videols = await db.VideoLs.FindAsync(id);

                VideoLEM videoLEM = await db.VideoLEMs.FirstOrDefaultAsync(e => e.VideoXId == videols.XId);


                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                EyeV eye = userinfo.EyeVs.FirstOrDefault(e => e.VideoXId == videols.XId);

                if (eye == null)
                {
                    videoLEM.Eye += 1;
                    EyeV eyenew = new EyeV();
                    eyenew.UserId = userinfo.Id;
                    eyenew.VideoXId = videols.XId;
                    db.EyeVs.Add(eyenew);
                    await db.SaveChangesAsync();

                }
            }
            catch { }
            return Json("");

        }

        public async Task<JsonResult> izb(int id, int tf)
        {
            try
            {

                VideoL video = db.VideoLs.Find(id);
                string UserName = User.Identity.Name;

                UserInfo userinfo = db.UserInfoes.Where(e => e.UserName == UserName).FirstOrDefault();


                if (tf == 1)
                {

                    Isbranni izb = new Isbranni();
                    izb.UserId = userinfo.Id;
                    izb.VideoLXId = video.XId;
                    db.Isbrannis.Add(izb);
                }
                else
                {

                    Isbranni izb = userinfo.Isbrannis.Where(e => e.VideoLXId == video.XId).FirstOrDefault();
                    db.Isbrannis.Remove(izb);


                }
                await db.SaveChangesAsync();
            }
            catch { }
            return Json("");

        }


        [HttpPost]
        public async Task<ActionResult> lookVideoL(int id, int ts, string language)
        {

            try
            {
                string UserName = GetUserName();
                if (language == null) { language = "ru"; }

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);



                //-----GET User ENABLE VIDEOXLs----------------------------------------
                var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= userinfo.LevelId).ToListAsync();
                var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();
                var EnableVideoXLs = new List<VideoXL>();
                foreach (var Module in Moduls)
                {
                    EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                }
                //-----END------------------------------------------------------------




                VideoL videols = await db.VideoLs.FindAsync(id);

                if (videols != null)
                {



                    LessonVideoTime LessonVideoTime = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == videols.XId);
                    if (LessonVideoTime == null)
                    {
                        LessonVideoTime LessonVideo = new LessonVideoTime();
                        LessonVideo.LessonXId = videols.XId;
                        LessonVideo.UserId = userinfo.Id;
                        LessonVideo.Status = true;
                        db.LessonVideoTimes.Add(LessonVideo);

                    }
                    else
                    {
                        LessonVideoTime.Status = true;
                    }

                    JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);




                    VideoXL NextVideoXL = EnableVideoXLs.Where(e => e.XId > videols.XId).OrderBy(e => e.XId).FirstOrDefault();


                    if (NextVideoXL != null)
                    {
                        if (ts == 0)
                        {
                            if (NextVideoXL.XId > jv.X)
                            {
                                jv.X = NextVideoXL.XId;


                            }

                        }


                    }


                    //if (cert == 1)
                    //{


                    //    Users_Certificates certificate = userinfo.Users_Certificates.FirstOrDefault();
                    //    if (certificate != null)
                    //    {
                    //        certificate.PublicCert = true;
                    //        result = "cert";
                    //    }

                    //}

                    await db.SaveChangesAsync();

                }
            }
            catch { }

            return Json("");
        }


        [HttpPost]
        public async Task<ActionResult> certPublic()
        {
            try
            {
                string UserName = GetUserName();


                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                Users_Certificates certificate = userinfo.Users_Certificates.FirstOrDefault();
                if (certificate != null)
                {
                    certificate.PublicCert = true;
                    await db.SaveChangesAsync();
                }
            }
            catch { }


            return Json("");
        }



        public async Task<string> MakeCertificate(string UserName, bool certpub, string language)
        {

            string result = null;
            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

            language = "ru";

            try
            {


                string username = "";

                if (userinfo.Name != null)
                {
                    username = userinfo.Name;
                }

                if (userinfo.SureName != null)
                {
                    username += " " + userinfo.SureName;
                }




                DateTime CurrentDate = DateTime.Now.AddHours(6);
                string Day = CurrentDate.Day.ToString();
                string Month = CurrentDate.Month.ToString();
                string Year = CurrentDate.Year.ToString();
                string Date = Day + "." + Month + "." + Year;
                int usernameLg = username.Length;

                string imageName = "";


                await System.Threading.Tasks.Task.Run(() =>
                {

                    int fontsize = 110;
                    int top = 915;

                    if (usernameLg > 22)
                    {
                        fontsize = 80;
                        top = 945;
                    }

                    int position = 1375 - (usernameLg * 25);


                    //string imgsrc = "http://academy.marinehealth.asia/Images/cert/background-cert-" + language + ".jpg";


                    string imgsrc = "https://onlinecoursezertis.azurewebsites.net/Images/certificate.jpg";
                    string html = "";

                    html = @"<html>
                     <head><meta http-equiv=Content-Type content='text/html; charset=UTF-8'>
                     <style type='text/css'>
                <!-- span.cls_003{font-family:sans-serif;color: rgb(52, 116, 79); font-weight:bold; font-style:normal; text-decoration: none;text-transform: uppercase}
                div.cls_003{ font-family:monospace; font-size:150.3px; color: rgb(223, 198, 108); font-weight:bold; font-style:normal; text-decoration: none}
                span.cls_004{ font-family:monospace; font-size:84.0px; color: rgb(223, 198, 108); font-weight:bold; font-style:normal; text-decoration: none}
                div.cls_004{ font-family:monospace; font-size:84.0px; color: rgb(223, 198, 108); font-weight:bold; font-style:normal; text-decoration: none}
                span.cls_002{ font-family:monospace; font-size:39.7px; color: rgb(223, 198, 108); font-weight:bold; font-style:normal; text-decoration: none}
                div.cls_002{ font-family:monospace; font-size:39.7px; color: rgb(223, 198, 108); font-weight:bold; font-style:normal; text-decoration: none}
                -->
                        </style>
                    <script type = 'text/javascript' src = 'a8535914-f191-11e9-9d71-0cc47a792c0a_id_a8535914-f191-11e9-9d71-0cc47a792c0a_files/wz_jsgraphics.js' ></script>
               </head>
               <body >
               <div style = 'position:absolute;left:50%;margin-left:-1462px;top:0px;width:2924px;height:2100px;border-style:outset;overflow:hidden' >
                <div style = 'position:absolute;left:0px;top:0px' >
                 <img src='" + imgsrc + "'width=2924 height=2100></div>";
                    html = html + "<div style = 'position:absolute;left:" + position + "px;top:" + top + "px' class='cls_003'><span class='cls_003' style='font-size:" + fontsize + "'>" + username + "</span></div></body></html>";


                    var htmlToImageConv = new NReco.ImageGenerator.HtmlToImageConverter();
                    var img = htmlToImageConv.GenerateImage(html, ImageFormat.Jpeg);

                    //var htmlToImage = new NReco.ImageGenerator.HtmlToImageConverter();
                    //var img = htmlToImage.GenerateImageFromFile("http://localhost:57916/ConvertUrlToImage/Index/18", ImageFormat.Jpeg);

                    FileResult fileResult = new FileContentResult(img, "image/" + ImageFormat.Jpeg);
                    fileResult.FileDownloadName = "image." + "jpg";

                    imageName = "~/Images/UserCertificate/" + userinfo.Name + "_" + "(certificate_" + userinfo.Id + ")" + ".jpg";

                    String path = HttpContext.Server.MapPath(imageName); //Path
                    System.IO.File.WriteAllBytes(path, img);
                });


                Users_Certificates UserCerNew = new Users_Certificates();
                UserCerNew.CertificateURL = imageName;
                UserCerNew.PublicCert = certpub;
                UserCerNew.UserId = userinfo.Id;
                db.Users_Certificates.Add(UserCerNew);
                await db.SaveChangesAsync();


                result = imageName;


            }
            catch
            { }

            return result;


        }


    }
}