using Newtonsoft.Json;
using OnlineCourseZertis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineCourseZertis.App_Start
{

   

    public class SMSPhone
    {


        public static async Task<bool> sendPassword(string phones, string Message)
        {

            
          

            bool sts = false;
            try
            {

                string URL = "https://smsc.kz/sys/send.php?";
                URL += "login=marinehealth";
                URL += "&fmt=3";
                URL += "&psw=M9cwcZfKRn";
                URL += "&phones=" + phones;
                URL += "&mes=";
                URL += Message;


                var client = new HttpClient();
                var content = await client.GetStringAsync(URL);


               

                sts = true;
            }
            catch { }



            return sts;

        }

    }
}