using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineCourseZertis.Models;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Xml;
using System.Threading.Tasks;
using OnlineCourseZertis.App_Start;
using System.Data.Entity;

namespace OnlineCourseZertis.Controllers
{

    public class userpaymentController : Controller
    {

        private Entities db = new Entities();


        [HttpPost]
        public async Task<ActionResult> success(int? pg_order_id)
        {

            string Message = "";

            if (pg_order_id != null)
            {
                try
                {

                    string UserName = User.Identity.Name;
                    UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
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

                                User_Premium User_PremiumOBJ = userinfo.User_Premium.Where(e => e.OrderId == pg_order_id).FirstOrDefault();

                                if (User_PremiumOBJ != null)
                                {

                                    if (!User_PremiumOBJ.buy)
                                    {

                                        User_PremiumOBJ.buy = true;

                                        userinfo.LevelId = User_PremiumOBJ.Premium.LevelId;

                                        ExpiredTimeVL CompleteTime = db.ExpiredTimeVLs.Find(1);

                                        DateTime ExpiredVLDate = DateTime.Now.AddYears(CompleteTime.DefaultVLYear).AddMonths(CompleteTime.DefaultVLMonth).AddDays(CompleteTime.DefaultVLDay);


                                        userinfo.ExpiredVLDate = ExpiredVLDate;

                                        userinfo.Expired = false;



                                        User_PremiumOBJ.Date = DateTime.Now.AddHours(6);

                                        await db.SaveChangesAsync();

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

                                        Message = "OK";

                                    }

                                }

                            }


                        }

                    }

                }

                catch
                {

                    Message = "error";

                }

            }


            return RedirectToAction("premium", "User", new { language = "ru", Message = Message });
        }




        [HttpPost]
        public async Task<ActionResult> result(int? pg_order_id)
        {

            if (pg_order_id == null)
            {

                return Json("not found");
            }

            string UserName = User.Identity.Name;
            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
            Random random = new Random();
            var chars = "0123456789aBhdktWiudshawopOElYq".ToCharArray();
            var pw = new char[16];
            for (int i = 0; i < 16; i++)
            {
                pw[i] = chars[random.Next(chars.Length)];
            }


            string pg_salt = new string(pw);



            try
            {
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

                            User_Premium User_PremiumOBJ = userinfo.User_Premium.Where(e => e.OrderId == pg_order_id).FirstOrDefault();

                            if (User_PremiumOBJ != null)
                            {

                                if (!User_PremiumOBJ.buy)
                                {
                                    User_PremiumOBJ.buy = true;

                                    userinfo.LevelId = User_PremiumOBJ.Premium.LevelId;

                                    if (userinfo.ExpiredVLDate == null)
                                    {
                                        ExpiredTimeVL CompleteTime = db.ExpiredTimeVLs.Find(1);

                                        DateTime ExpiredVLDate = DateTime.Now.AddYears(CompleteTime.DefaultVLYear).AddMonths(CompleteTime.DefaultVLMonth).AddDays(CompleteTime.DefaultVLDay);


                                        userinfo.ExpiredVLDate = ExpiredVLDate;

                                        userinfo.Expired = false;
                                    }


                                   await db.SaveChangesAsync();
                                }
                            }

                        }

                    }
                }
            }
            catch
            {

                return Json("not found");

            }


            return Json("OK");


        }



        [HttpPost]
        public ActionResult failure(int? pg_order_id)
        {

            return RedirectToAction("premium", "User", new { language = "ru" });
        }






    }
}