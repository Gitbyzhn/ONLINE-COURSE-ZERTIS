using OnlineCourseZertis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCourseZertis.Events
{
    public class VerifyAvailability
    {



        public static void CompleteTraining(string UserName)
        {

            Entities db = new Entities();
            UserInfo userinfo = db.UserInfoes.Where(e => e.UserName == UserName).FirstOrDefault();

            ExpiredTimeVL ExpiredTime = db.ExpiredTimeVLs.Find(1);

            if (userinfo.ExpiredVLDate == null || userinfo.Expired==true)
            {

                return;
            }


            if ((DateTime.Now.Date.Equals(userinfo.ExpiredVLDate.Value.Date) || DateTime.Now.Date > userinfo.ExpiredVLDate.Value.Date) && ExpiredTime.Enable)
            {

                userinfo.LevelId = 0;

                var UserPremium = userinfo.User_Premium.ToList();

                foreach (var item in UserPremium)
                {
                    item.buy = false;

                }

                userinfo.Expired = true;
                db.SaveChanges();

            }


        }

    }
}