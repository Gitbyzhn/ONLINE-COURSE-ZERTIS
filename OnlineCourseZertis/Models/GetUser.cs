using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCourseZertis.Models
{
    public class UserGet
    {



        public static UserProgress Progress(string UserName, int? UserLevelId)
        {

            Entities db = new Entities();

            if (UserLevelId == null)
            {
                UserLevelId = 0;
            }

            JVLO jv = db.JVLOes.Where(e => e.UserName == UserName).FirstOrDefault();
            var EnableVideoXLs = new List<VideoXL>();
            int NextV = 0;
            bool JVXNot = false;
            bool EndCount = false;
            bool EmtyProcess = false;

            UserProgress progress = new UserProgress();
            try
            {
                
                //-----GET User ENABLE VIDEOXLs----------------------------------------

                var Moduls_userLevel = db.Modul_userLevel.Where(e => e.LevelId <= UserLevelId).ToList();
                if (Moduls_userLevel.Count == 0)
                {
                    EmtyProcess = true;
                }


                var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();

                if (Moduls.Count() == 0)
                {
                    EmtyProcess = true;
                }

                if (EmtyProcess == false)
                {
                    foreach (var Module in Moduls)
                    {
                        EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                    }

                    if (EnableVideoXLs.Count == 0)
                    {
                        EmtyProcess = true;
                    }
                }


                EnableVideoXLs = EnableVideoXLs.OrderBy(e => e.XId).ToList();

                //-----END------------------------------------------------------------



                if (!EmtyProcess)
                {





                    UserInfo userinfo = db.UserInfoes.Where(e => e.UserName == UserName).FirstOrDefault();



                    //Error increase JVXID
                    double LastVXId = EnableVideoXLs.Max(e => e.XId);


                    if (jv.X > LastVXId)
                    {
                        jv.X = LastVXId;
                        JVXNot = true;
                    }


                    var VideosOverXID = EnableVideoXLs.Where(e => e.XId >= jv.X).ToList();

                    foreach (var videoEXL in VideosOverXID)
                    {



                       VideoL videols = videoEXL.VideoLs.Where(e => e.language == "ru").FirstOrDefault();
                        


                        if (videols != null)
                        {
                            ValitO vo = userinfo.ValitOS.Where(e => e.VdeoLXId == videols.XId).FirstOrDefault();
                        

                            var tst = videols.Tests.ToList();
                            if (tst.Count > 0)
                            {

                                NextV = 1;

                                if (vo != null)
                                {
                                    if (vo.KB > 74)
                                    {
                                        NextV = 0;

                                    }

                                }
                            }

                         

                            LessonVideoTime lookvideo = userinfo.LessonVideoTimes.Where(e => e.LessonXId == videols.XId).FirstOrDefault();
                            if (lookvideo == null)
                            {
                                NextV = 3;
                            }
                            else if (lookvideo.Status == false)
                            {
                                NextV = 3;

                            }

                            if (NextV != 0)
                            {
                                if (jv.X < videols.XId)
                                {
                                    jv.X = videols.XId;
                                    JVXNot = true;

                                }
                                break;
                            }

                        }



                    }



                    if (NextV == 0)
                    {

                        jv.X = LastVXId;
                        JVXNot = true;
                    }


                    if (JVXNot)
                    {
                        db.SaveChanges();
                    }


                    int ALc = EnableVideoXLs.Where(e => e.XId < jv.X && (e.XId - (int)e.XId) < 0.1).Count() + 1;
                    int RALc = EnableVideoXLs.Where(e => e.XId > jv.X).Count();


                    int c = ALc + RALc;
                    int k = (ALc - 1) * 100;





                    if (ALc == c)
                    {



                        VideoL videols = db.VideoLs.Where(e => e.language == "ru" && e.XId == jv.X).FirstOrDefault();

                        if (videols != null)
                        {



                            var tst = db.Tests.Where(e => e.LessonId == videols.Id);

                            if (tst.Count() > 0)
                            {

                                ValitO valito = userinfo.ValitOS.Where(e => e.VdeoLXId == jv.X).FirstOrDefault();
                                if (valito != null)
                                {
                                    if (valito.KB > 74)
                                    {
                                        EndCount = true;
                                    }
                                }

                            }
                            else if (tst.Count() == 0)
                            {

                                LessonVideoTime look = userinfo.LessonVideoTimes.Where(e => e.LessonXId == jv.X).FirstOrDefault();
                                if (look != null)
                                {
                                    if (look.Status)
                                    {
                                        EndCount = true;
                                    }
                                }
                            }
                        }

                        if (EndCount)
                        {
                            k = ALc * 100;
                        }



                    }

                    progress.OUK = k / c;
                    progress.TBB = jv.TBB;
                    progress.EnableVideoXLs = EnableVideoXLs;
                    progress.JVLO = jv;
                    progress.NextV = NextV;
                }

            }

            catch
            { EmtyProcess = true; }


            if (EmtyProcess)
            {
                progress.OUK = 0;
                progress.TBB = 0;
                progress.EnableVideoXLs = EnableVideoXLs;
                progress.JVLO = jv;
                progress.NextV = 0;

            }

            return progress;

        }

    }
}