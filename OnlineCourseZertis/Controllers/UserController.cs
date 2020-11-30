using Newtonsoft.Json;
using OnlineCourseZertis.App_Start;
using OnlineCourseZertis.Events;
using OnlineCourseZertis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace OnlineCourseZertis.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private Entities db = new Entities();


        public string GetUserName()
        {

            return User.Identity.Name;
        }

        public string GetHost()
        {

            string host = Request.Url.Host;
            return host;
        }

        public async Task<ActionResult> Initialization(string language)
        {

            try
            {
                string UserName = GetUserName();

                VerifyAvailability.CompleteTraining(UserName);


                JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);
                if (jv == null)
                {
                    JVLO jvnew = new JVLO();
                    jvnew.OV = 0;
                    jvnew.TBB = 0;
                    jvnew.UserName = UserName;
                    jvnew.X = 1;
                    db.JVLOes.Add(jvnew);
                    await db.SaveChangesAsync();
                }
            }
            catch { }


            return RedirectToAction("Modules", "Learning", new { language = language });


        }


        public async Task<ActionResult> transition(string language, int? transition)
        {

            try
            {
                string UserName = GetUserName();
                string host = GetHost();

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                Premium premium = new Premium();


                switch (transition)
                {

                    case 1:

                        premium = db.Premiums.Find(1);
                        break;

                    case 2:

                        premium = db.Premiums.Find(2);
                        break;

                    case 3:

                        premium = db.Premiums.Find(3);
                        break;

                    case 4:
                        return RedirectToAction("Videos", "Learning", new { language = language, id = 11 });

                }



                if (premium != null)
                {



                    Premium_OrderId Premium_OrderIdOBJ = db.Premium_OrderId.FirstOrDefault();

                    Premium_OrderIdOBJ.OrderId += 1;


                    User_Premium User_PremiumOBJ = userinfo.User_Premium.FirstOrDefault(e => e.PremiumId == premium.Id);




                    if (User_PremiumOBJ == null)
                    {

                        db.User_Premium.Add(new User_Premium
                        {

                            UserId = userinfo.Id,
                            PremiumId = premium.Id,
                            OrderId = Premium_OrderIdOBJ.OrderId,
                            buy = false
                        });
                    }
                    else
                    {


                        if (User_PremiumOBJ.buy)
                        {

                            return RedirectToAction("premium", new { language = language });

                        }


                        User_PremiumOBJ.OrderId = Premium_OrderIdOBJ.OrderId;


                    }

                    await db.SaveChangesAsync();



                    Random random = new Random();
                    var chars = "0123456789aBhdktWiudshawopOElYq".ToCharArray();
                    var pw = new char[16];
                    for (int i = 0; i < 16; i++)
                    {
                        pw[i] = chars[random.Next(chars.Length)];
                    }


                    string pg_salt = new string(pw);





                    string md5string = "payment.php;";
                    md5string += premium.Price + ";";
                    md5string += "KZT;";
                    md5string += premium.Name + ";";
                    //md5string += "http://localhost:57916/User/premium" + ";";
                    md5string += "https://online.zertis.biz/userpayment/failure" + ";";
                    md5string += "POST;";
                    md5string += "ru;";
                    md5string += 517933 + ";";
                    md5string += Premium_OrderIdOBJ.OrderId + ";";
                    md5string += pg_salt + ";";
                    //md5string += "http://localhost:6957/userpayment/success" + ";";
                    md5string += "https://online.zertis.biz/userpayment/success" + ";";
                    md5string += "POST;";
                    //md5string += 1 + ";";
                    md5string += "TCHBTYSmb2qEvUPs";




                    var md5 = MD5.Create();
                    string hash = MD5generator.GetMd5Hash(md5, md5string);



                    string Url = "https://api.paybox.money/payment.php?";
                    Url += "pg_amount=" + premium.Price;
                    Url += "&pg_currency=KZT";
                    Url += "&pg_description=" + premium.Name;
                    //Url += "&pg_failure_url=" + "http://localhost:57916/User/premium";
                    Url += "&pg_failure_url=" + "https://online.zertis.biz/userpayment/failure";
                    Url += "&pg_language=ru";
                    Url += "&pg_merchant_id=" + 517933;
                    Url += "&pg_order_id=" + Premium_OrderIdOBJ.OrderId;
                    Url += "&pg_salt=" + pg_salt;
                    //Url += "&pg_success_url=" + "http://localhost:6957/userpayment/success";
                    Url += "&pg_success_url=" + "https://online.zertis.biz/userpayment/success";
                    Url += "&pg_success_url_method=POST";
                    Url += "&pg_failure_url_method=POST";
                    Url += "&pg_sig=" + hash;
                    //Url += "&pg_testing_mode=1";

                    return Redirect(Url);

                }
            }
            catch { }


            return RedirectToAction("premium", new { language = language });


        }




        public async Task<ActionResult> premium(string language, string Message)
        {

            if (Message != "ok")
            {
                await UserPremiumControl();
            }

            if (language == null) { language = "ru"; }

            ViewBag.Message = Message;
            ViewBag.language = language;

            var premium = await db.Premiums.ToListAsync();
            return View(premium);
        }



        public async Task<bool> UserPremiumControl()
        {

            string UserName = User.Identity.Name;
            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
            var userpremium = userinfo.User_Premium.Where(e => e.buy == false).ToList();
            bool result = true;

            if (userpremium.Count > 0)
            {

                foreach (var premium in userpremium)
                {
                    int pg_order_id = premium.OrderId;
                    try
                    {


                        Random random = new Random();
                        var chars = "0123456789aBhdktWiudshawopOElYq".ToCharArray();
                        var pw = new char[16];
                        for (int i = 0; i < 16; i++)
                        {
                            pw[i] = chars[random.Next(chars.Length)];
                        }


                        string pg_salt = new string(pw);





                        string md5string = "get_status.php;";
                        md5string += 517933 + ";";
                        md5string += pg_order_id + ";";
                        md5string += pg_salt + ";";
                        md5string += "TCHBTYSmb2qEvUPs";
                        var md5 = MD5.Create();

                        string hash = MD5generator.GetMd5Hash(md5, md5string);



                        var url = "https://api.paybox.money/get_status.php?";
                        url += "pg_merchant_id=" + 517933;
                        url += "&pg_order_id=" + pg_order_id;
                        url += "&pg_salt=" + pg_salt;
                        url += "&pg_sig=" + hash;



                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                            HttpResponseMessage response = client.GetAsync(url).Result;

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                                StringReader sr = new StringReader(xdoc.ToString());
                                DataSet ds = new DataSet();
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xdoc.ToString());

                                string json = JsonConvert.SerializeXmlNode(doc);

                                paybox_get_sts paybox_get_stsOBJ = JsonConvert.DeserializeObject<paybox_get_sts>(json);

                                if (paybox_get_stsOBJ.response.pg_transaction_status == "ok" && paybox_get_stsOBJ.response.pg_captured == 1)
                                {

                                    premium.buy = true;
                                    userinfo.LevelId = premium.Premium.LevelId;
                                    premium.Date = DateTime.Now.AddHours(6);
                                    
                                    ExpiredTimeVL CompleteTime = db.ExpiredTimeVLs.Find(1);

                                    DateTime ExpiredVLDate = DateTime.Now.AddYears(CompleteTime.DefaultVLYear).AddMonths(CompleteTime.DefaultVLMonth).AddDays(CompleteTime.DefaultVLDay);


                                    userinfo.ExpiredVLDate = ExpiredVLDate;

                                    userinfo.Expired = false;

                                    string MSG = "";
                                    MSG += "online.zertis.biz";
                                    MSG += "\n";
                                    MSG += "Новый Покупатель";
                                    MSG += "\n";
                                    MSG += userinfo.SureName + " " + userinfo.Name;
                                    MSG += "\n";
                                    MSG += userinfo.User_Premium.FirstOrDefault().Premium.Name;
                                    if (userinfo.User_Premium.FirstOrDefault().Date != null)
                                    {
                                        MSG += "\n";
                                        MSG += userinfo.User_Premium.FirstOrDefault().Date;
                                    }
                                    MSG += "\n";
                                    MSG += UserName;

                                    bool sms = await SMSPhone.sendPassword("+77753578449", MSG);


                                    await db.SaveChangesAsync();
                                }


                            }

                        }

                    }

                    catch
                    {

                        result = false;

                    }


                }
            }

            return result;


        }


        [HttpPost]
        public async Task<ActionResult> premium(int premiumID, string language)
        {

            Premium premium = await db.Premiums.FindAsync(premiumID);

            //string host = GetHost();

            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == User.Identity.Name);

            ViewBag.language = language;
            var premiumlist = await db.Premiums.ToListAsync();

            try
            {

             

                if (premium != null)
                {



                    Premium_OrderId Premium_OrderIdOBJ = db.Premium_OrderId.FirstOrDefault();

                    Premium_OrderIdOBJ.OrderId += 1;


                    User_Premium User_PremiumOBJ = userinfo.User_Premium.FirstOrDefault(e => e.PremiumId == premiumID);




                    if (User_PremiumOBJ == null)
                    {

                        db.User_Premium.Add(new User_Premium
                        {

                            UserId = userinfo.Id,
                            PremiumId = premiumID,
                            OrderId = Premium_OrderIdOBJ.OrderId,
                            buy = false
                        });
                    }
                    else
                    {


                        if (User_PremiumOBJ.buy)
                        {

                            return View(premiumlist);

                        }


                        User_PremiumOBJ.OrderId = Premium_OrderIdOBJ.OrderId;


                    }

                    await db.SaveChangesAsync();



                    Random random = new Random();
                    var chars = "0123456789aBhdktWiudshawopOElYq".ToCharArray();
                    var pw = new char[16];
                    for (int i = 0; i < 16; i++)
                    {
                        pw[i] = chars[random.Next(chars.Length)];
                    }


                    string pg_salt = new string(pw);


                    string md5string = "payment.php;";
                    md5string += premium.Price + ";";
                    md5string += "KZT;";
                    md5string += premium.Name + ";";
                    //md5string += "http://localhost:57916/User/premium" + ";";
                    md5string += "https://online.zertis.biz/userpayment/failure" + ";";
                    md5string += "POST;";
                    md5string += "ru;";
                    md5string += 517933 + ";";
                    md5string += Premium_OrderIdOBJ.OrderId + ";";
                    md5string += pg_salt + ";";
                    //md5string += "http://localhost:6957/userpayment/success" + ";";
                    md5string += "https://online.zertis.biz/userpayment/success" + ";";
                    md5string += "POST;";
                    //md5string += 1 + ";";
                    md5string += "TCHBTYSmb2qEvUPs";


                    var md5 = MD5.Create();
                    string hash = MD5generator.GetMd5Hash(md5, md5string);



                    string Url = "https://api.paybox.money/payment.php?";
                    Url += "pg_amount=" + premium.Price;
                    Url += "&pg_currency=KZT";
                    Url += "&pg_description=" + premium.Name;
                    //Url += "&pg_failure_url=" + "http://localhost:57916/User/premium";
                    Url += "&pg_failure_url=" + "https://online.zertis.biz/userpayment/failure";
                    Url += "&pg_language=ru";
                    Url += "&pg_merchant_id=" + 517933;
                    Url += "&pg_order_id=" + Premium_OrderIdOBJ.OrderId;
                    Url += "&pg_salt=" + pg_salt;
                    //Url += "&pg_success_url=" + "http://localhost:6957/userpayment/success";
                    Url += "&pg_success_url=" + "https://online.zertis.biz/userpayment/success";
                    Url += "&pg_success_url_method=POST";
                    Url += "&pg_failure_url_method=POST";
                    Url += "&pg_sig=" + hash;
                    //Url += "&pg_testing_mode=1";

                    return Redirect(Url);

                }



              
            }
            catch { }

            return View(premiumlist);

        }




        public ActionResult premiumTest(string language, string Message)
        {


            if (language == null) { language = "ru"; }

            ViewBag.Message = Message;
            ViewBag.language = language;

            var premium = db.Premiums.ToList();

            return View(premium);
        }




        [HttpPost]
        public ActionResult premiumTest(int premiumID, string language)
        {


            Premium premium = db.Premiums.Find(premiumID);

            string host = GetHost();

            UserInfo userinfo = db.UserInfoes.Where(e => e.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.language = language;
            var premiumlist = db.Premiums.ToList();

            if (premium != null)
            {



                Premium_OrderId Premium_OrderIdOBJ = db.Premium_OrderId.First();

                Premium_OrderIdOBJ.OrderId += 1;


                User_Premium User_PremiumOBJ = userinfo.User_Premium.Where(e => e.PremiumId == premiumID).FirstOrDefault();




                if (User_PremiumOBJ == null)
                {

                    db.User_Premium.Add(new User_Premium
                    {

                        UserId = userinfo.Id,
                        PremiumId = premiumID,
                        OrderId = Premium_OrderIdOBJ.OrderId,
                        buy = false
                    });
                }
                else
                {


                    if (User_PremiumOBJ.buy)
                    {

                        return View(premiumlist);

                    }


                    User_PremiumOBJ.OrderId = Premium_OrderIdOBJ.OrderId;


                }

                db.SaveChanges();



                Random random = new Random();
                var chars = "0123456789aBhdktWiudshawopOElYq".ToCharArray();
                var pw = new char[16];
                for (int i = 0; i < 16; i++)
                {
                    pw[i] = chars[random.Next(chars.Length)];
                }


                string pg_salt = new string(pw);

                if (premiumID == 1) { premium.Price = 100; }

                if (premiumID == 2) { premium.Price = 200; }

                if (premiumID == 3) { premium.Price = 300; }



                string md5string = "payment.php;";
                md5string += premium.Price + ";";
                md5string += "KZT;";
                md5string += premium.Name + ";";
                //md5string += "http://localhost:6957/userpayment/failure" + ";";
                md5string += "https://online.zertis.biz/userpayment/failure" + ";";
                md5string += "POST;";
                md5string += "ru;";
                md5string += 517933 + ";";
                md5string += Premium_OrderIdOBJ.OrderId + ";";
                md5string += pg_salt + ";";
                //md5string += "http://localhost:6957/userpayment/success" + ";";
                md5string += "https://online.zertis.biz/userpayment/success" + ";";
                md5string += "POST;";
                md5string += 1 + ";";
                md5string += "TCHBTYSmb2qEvUPs";




                var md5 = MD5.Create();
                string hash = MD5generator.GetMd5Hash(md5, md5string);



                string Url = "https://api.paybox.money/payment.php?";
                Url += "pg_amount=" + premium.Price;
                Url += "&pg_currency=KZT";
                Url += "&pg_description=" + premium.Name;
                //Url += "&pg_failure_url=" + "http://localhost:6957/userpayment/failure";
                Url += "&pg_failure_url=" + "https://online.zertis.biz/userpayment/failure";
                Url += "&pg_language=ru";
                Url += "&pg_merchant_id=" + 517933;
                Url += "&pg_order_id=" + Premium_OrderIdOBJ.OrderId;
                Url += "&pg_salt=" + pg_salt;
                //Url += "&pg_success_url=" + "http://localhost:6957/userpayment/success";
                Url += "&pg_success_url=" + "https://online.zertis.biz/userpayment/success";
                Url += "&pg_success_url_method=POST";
                Url += "&pg_failure_url_method=POST";
                Url += "&pg_sig=" + hash;
                Url += "&pg_testing_mode=1";

                return Redirect(Url);

            }



            return View(premiumlist);





        }



        public async Task<ActionResult> profile(string language)
        {

            try
            {

                if (language == null) { language = "ru"; }

                string UserName = User.Identity.Name;
                UserInfo UserInfoOBJ = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = UserGet.Progress(UserName, UserInfoOBJ.LevelId);

                ViewBag.language = language;

                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;

                return View(UserInfoOBJ);
            }
            catch { }

            return RedirectToAction("ErrorLogOff", "Account", new { language = language });
        }



        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> profile(UserProfileViewModel userprofile, string language)
        {
            try
            {
                if (language == null) { language = "ru"; }

                string UserName = User.Identity.Name;

                UserInfo UserInfoOBJ = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                UserProgress UserProgress = UserGet.Progress(UserName, UserInfoOBJ.LevelId);


                UserInfoOBJ.Name = userprofile.Name;
                UserInfoOBJ.SureName = userprofile.SureName;

                HttpPostedFileBase file1 = Request.Files["img1"];
                if (file1.FileName != "" && file1 != null)
                {
                    string fullPath = Request.MapPath(UserInfoOBJ.Image);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string ImageName = file1.FileName;
                    UserInfoOBJ.Image = "~/Images/UserImage/" + ImageName.Replace(" ", "_");

                    file1.SaveAs(HttpContext.Server.MapPath(UserInfoOBJ.Image));

                }

                await db.SaveChangesAsync();


                ViewBag.language = language;
                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                return View(UserInfoOBJ);
            }
            catch { }
            return RedirectToAction("ErrorLogOff", "Account", new { language = language });
        }


        public async  Task<ActionResult> favorites(string language)
        {

            try
            {
                string UserName = User.Identity.Name;



                if (language == null) { language = "ru"; }
                ViewBag.language = language;



                UserInfo UserInfoOBJ = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);

                UserProgress UserProgress = UserGet.Progress(UserName, UserInfoOBJ.LevelId);


                List<VideoL> videoizb = new List<VideoL>();
                foreach (var izb in UserInfoOBJ.Isbrannis.ToList())
                {
                    VideoL vid = await db.VideoLs.FirstOrDefaultAsync(e => e.language == language && e.XId == izb.VideoLXId);
                    if (vid == null)
                        vid = await db.VideoLs.FirstOrDefaultAsync(e => e.language == "ru" && e.XId == izb.VideoLXId);

                    videoizb.Add(vid);
                }

                ViewBag.UserLevelID = UserInfoOBJ.LevelId;
                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;

                return View(videoizb.ToList());
            }
            catch { }
            return RedirectToAction("ErrorLogOff", "Account", new { language = language });

        }


        public async Task<ActionResult> certificates(string language)
        {
            try
            {

                string UserName = User.Identity.Name;


                if (language == null) { language = "ru"; }
                ViewBag.language = language;


                UserInfo UserInfoOBJ = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                UserProgress UserProgress = UserGet.Progress(UserName, UserInfoOBJ.LevelId);

                var user_certificates = UserInfoOBJ.Users_Certificates.ToList();



                ViewBag.language = language;
                ViewBag.OUK = UserProgress.OUK;
                ViewBag.TBB = UserProgress.TBB;
                return View(user_certificates);
            }
            catch { }
            return RedirectToAction("ErrorLogOff", "Account", new { language = language });
        }


        public FilePathResult Contacts_of_all_suppliers()
        {

            string UserName = User.Identity.Name;

            UserInfo UserInfo = db.UserInfoes.FirstOrDefault(e => e.UserName == UserName);

            var User_Premium = UserInfo.User_Premium.Where(e => e.buy == true).ToList();

            string fileName = "Zertis.pdf";

            string rootpath = Server.MapPath("~/Images/Zertis.pdf");

            if (User_Premium.Count > 0)
            {
                rootpath = Server.MapPath("~/ServerFile/Zertis_Edu_Contacts_of_all_suppliers.pdf");
                fileName = "Zertis_Edu_Contacts_of_all_suppliers.pdf";
            }


            return File(rootpath, "multipart/form-data", fileName);
        }



        public FilePathResult tenbooks(int? id)
        {

            string UserName = User.Identity.Name;

            UserInfo UserInfo = db.UserInfoes.FirstOrDefault(e => e.UserName == UserName);

            var User_Premium = UserInfo.User_Premium.Where(e => e.buy == true && (e.PremiumId == 2 || e.PremiumId == 3)).ToList();

            string fileName = "Zertis.pdf";

            string rootpath = Server.MapPath("~/Images/Zertis.pdf");

            if (User_Premium.Count > 0)
            {
                switch (id)
                {
                    case 1:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Agrokhimia.pdf");
                        fileName = "Agrokhimia.pdf";
                        break;
                    case 2:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Effektivnaya_elektrifikatsia_zaschischennogo_grunta_Prischep_L_G.pdf");
                        fileName = "Effektivnaya_elektrifikatsia_zaschischennogo_grunta_Prischep_L_G.pdf";
                        break;
                    case 3:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Fotosintez_i_spektralny_analiz_sveta.pdf");
                        fileName = "Fotosintez_i_spektralny_analiz_sveta.pdf";
                        break;
                    case 4:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Gidroponika_dlya_lyubiteley_Zaltser_E.pdf");
                        fileName = "Gidroponika_dlya_lyubiteley_Zaltser_E.pdf";
                        break;
                    case 5:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Gidroponika_dlya_vsekh.pdf");
                        fileName = "Gidroponika_dlya_vsekh.pdf";
                        break;
                    case 6:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Gidroponika_dlya_vsekh_Vse_o_sadovodstve_na_domu_Uilyam_Texye.pdf");
                        fileName = "Gidroponika_dlya_vsekh_Vse_o_sadovodstve_na_domu_Uilyam_Texye.pdf";
                        break;
                    case 7:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Gidroponika_komnanykh_tsvetov.pdf");
                        fileName = "Gidroponika_komnanykh_tsvetov.pdf";
                        break;
                    case 8:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Growing Under Glass Choosing.pdf");
                        fileName = "Growing Under Glass Choosing.pdf";
                        break;
                    case 9:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/LED Lighting for Urban Agriculture.pdf");
                        fileName = "LED Lighting for Urban Agriculture.pdf";
                        break;
                    case 10:
                        rootpath = Server.MapPath("~/ServerFile/tenbooks/Rastenia_bez_pochvy.pdf");
                        fileName = "Rastenia_bez_pochvy.pdf";
                        break;
                    default:
                        break;
                }


            }


            return File(rootpath, "multipart/form-data", fileName);
        }


    }
}