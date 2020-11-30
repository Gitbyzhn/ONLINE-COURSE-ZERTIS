using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Text;

namespace OnlineCourseZertis.Models
{
    public class GetUser
    {
        
        public static UserInfo Info(string UserName)
        {
            Entities db = new Entities();
            UserInfo userinfo = db.UserInfoes.Where(e => e.UserName == UserName).FirstOrDefault();
            return userinfo;
        }


        public static MyProfileCs MyProfile(string language)
        {

            MyProfileCs TextWebSite = new MyProfileCs();

            switch (language)
            {
                case "kz":

                    TextWebSite.Title = "Менің профилім";
                    TextWebSite.EditBtn = "Өзгерту";
                    TextWebSite.SaveBtn = "Сақтау";
                    TextWebSite.VideoName = "Онлайн оқытуды үйренуге арналған бейне нұсқаулық";
                 


                    break;
                    
                case "ru":
                    TextWebSite.Title = "Мой профиль";
                    TextWebSite.EditBtn = "Изменить";
                    TextWebSite.SaveBtn = "Сохранить";
                    TextWebSite.VideoName = "Видео-инструкция для изучение онлайн обучение";
                    break;

            }



            return TextWebSite;

        }

        
        public static TextLayout language(string language)
        {

            TextLayout TextWebSite = new TextLayout();

            switch (language)
            {
                case "kz":

                    TextWebSite.MyProfile = "Менің профилім";
                    TextWebSite.Modules = "Модульдер";
                    TextWebSite.Lessons = "Барлық видеодәрістер";
                    TextWebSite.Tasks = "Тапсырмалар";
                    TextWebSite.Favorite = "Таңдаулы";
                    TextWebSite.UsefulServices = "Пайдалы қызметтер";
                    TextWebSite.ByPr = "Сатып алу";
                    TextWebSite.MyProgress = "Менің прогрессім";
                    TextWebSite.Passed = "Барлық видеодәрістен өтілгені";
                    TextWebSite.Performance = "Өтілген видеодәрістен үлегрімі";
                    TextWebSite.Reserved = "Барлық құқықтар сақталған";
                    TextWebSite.Logout = "Шығу";


                    break;

                case "uz":

                    TextWebSite.Modules = "Modul darslari";
                    TextWebSite.Lessons = "Barcha video darslar";
                    TextWebSite.Tasks = "Missiyalar";
                    TextWebSite.Favorite = "Sevimli";
                    TextWebSite.UsefulServices = "Foydali xizmatlar";
                    TextWebSite.MyProgress = "Mening taraqqiyotim";
                    TextWebSite.Passed = "Barcha darslardan";
                    TextWebSite.Performance = "O'quv faoliyati";
                    TextWebSite.Reserved = "Barcha huquqlar himoyalangan";
                    TextWebSite.Logout = "Chiqib chiq";
                    break;

                case "kr":

                    TextWebSite.Modules = "Модулдар сабак";
                    TextWebSite.Lessons = "Бардык видео сабактар";
                    TextWebSite.Tasks = "Тапшырмалар";
                    TextWebSite.Favorite = "Сүйүктүүлөр";
                    TextWebSite.UsefulServices = "Пайдалуу кызмат";
                    TextWebSite.MyProgress = "Менин прогресс";
                    TextWebSite.Passed = "Бардык видео дарстар үзүндү";
                    TextWebSite.Performance = "Видео дарстар үйрөнгөн ülegrimi";
                    TextWebSite.Reserved = "Barcha huquqlar himoyalangan";
                    TextWebSite.Logout = "Чыгуу";
                    break;


                case "tr":

                    TextWebSite.Modules = "Modül üroki";
                    TextWebSite.Lessons = "Tüm video eğiticileri";
                    TextWebSite.Tasks = "Atamaları";
                    TextWebSite.Favorite = "Sık";
                    TextWebSite.UsefulServices = "Faydalı Hizmetler";
                    TextWebSite.MyProgress = "İlerlemem";
                    TextWebSite.Passed = "Tüm derslerin";
                    TextWebSite.Performance = "O'quv faoliyati";
                    TextWebSite.Reserved = "Tüm hakları saklıdır.";
                    TextWebSite.Logout = "Çıkış Yap";
                    break;

                case "en":

                    TextWebSite.Modules = "Modules lessons";
                    TextWebSite.Lessons = "All video tutorials";
                    TextWebSite.Tasks = "Tasks";
                    TextWebSite.Favorite = "Favorite";
                    TextWebSite.UsefulServices = "Useful Services";
                    TextWebSite.MyProgress = "My progress";
                    TextWebSite.Passed = "Of all the lessons";
                    TextWebSite.Performance = "Progress";
                    TextWebSite.Reserved = "All rights reserved";
                    TextWebSite.Logout = "Logout";
                    break;


                case "ru":
                    TextWebSite.MyProfile = "Мой профиль";
                    TextWebSite.Modules = "Модули уроки";
                    TextWebSite.Lessons = "Все видеоуроки";
                    TextWebSite.Tasks = "Задания";
                    TextWebSite.Favorite = "Избранное";
                    TextWebSite.UsefulServices = "Полезные сервисы";
                    TextWebSite.ByPr = "Покупка";
                    TextWebSite.MyProgress = "Мой прогресс";
                    TextWebSite.Passed = "Из всех уроков";
                    TextWebSite.Performance = "Успеваемость";
                    TextWebSite.Reserved = "Все права защищены";
                    TextWebSite.Logout = "Выйти";
                    break;

            }



            return TextWebSite;

        }

        
        public static TextVideosСs TextVideos(string language)
        {

            TextVideosСs TextWebSite = new TextVideosСs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Title = "Барлық видеодәрістер";

                    TextWebSite.Lesson = "Бұл видеосабаққа өту үшін, алдыңғы видеосабақтан өтуіңіз керек.";
                    TextWebSite.Task = "Бұл видеосабаққа өту үшін, алдыңғы видеосабақтағы тапсырманы орындау керек.";
                    TextWebSite.Test = "Бұл видеосабаққа өту үшін, алдыңғы видеосабақтағы тестті тапсырып, жақсы баға алу керек.";

                    TextWebSite.PassedLesson = "Видеосабаққа өту";
                    TextWebSite.PassedTask = "Тапсырманы орындау";
                    TextWebSite.PassedTest = "Тестті тапсыру";


                    break;


                case "ru":


                    TextWebSite.Title = "Все видеоуроки";

                    TextWebSite.Lesson = "Чтобы получить доступ к данному видеоуроку, вам необходимо пройти предыдущий урок.";
                    TextWebSite.Task = "Чтобы получить доступ к данному видеоуроку, вам необходимо выполнить задание из предыдущего видеоурока.";
                    TextWebSite.Test = "Чтобы получить доступ к данному видеоуроку, вам необходимо пройти тест из предыдущего видеоурока и получить хорошую оценку.";

                    TextWebSite.PassedLesson = "Пройти видеоурок";
                    TextWebSite.PassedTask = "Пройти задание";
                    TextWebSite.PassedTest = "Пройти тест";


                    break;


                case "uz":


                    TextWebSite.Title = "Barcha video darslar";

                    TextWebSite.Lesson = "Ushbu video darsga kirish uchun siz avvalgi darsga o'tishingiz kerak.";
                    TextWebSite.Task = "Ushbu video darsiga kirish uchun avvalgi video darsligidagi vazifani bajarish kerak.";
                    TextWebSite.Test = "Ushbu video darsga kirish uchun avvalgi video darslikdan testni topshirish va yaxshi baho olish kerak.";

                    TextWebSite.PassedLesson = "Video darstan o'tish";
                    TextWebSite.PassedTask = "Vazifadan o'tish";
                    TextWebSite.PassedTest = "Testdan o'tish";

                    break;


                case "kr":


                    TextWebSite.Title = "Бардык видео сабак";

                    TextWebSite.Lesson = "Бул видео сабактан өтүү үчүн, буга чейинки видео сабактан өтүшүңүз керек.";
                    TextWebSite.Task = "Бул видео сабакка өтүү үчүн, мурунку видео сабактан тапшырманы аткарышыңыз керек.";
                    TextWebSite.Test = "Бул видео сабактан өтүү үчүн, мурунку видео сабакта тесттен өтүп, жакшы баа алуу керек.";

                    TextWebSite.PassedLesson = "Видео сабак өтүү";
                    TextWebSite.PassedTask = "Тапшырманы аткаруу";
                    TextWebSite.PassedTest = "Текшерүү өтүү";


                    break;


                case "tr":


                    TextWebSite.Title = "Tüm video eğiticileri";

                    TextWebSite.Lesson = "Bu eğitim videosuna erişmek için, önceki öğreticiyi tamamlamanız gerekir.";
                    TextWebSite.Task = "Bu eğitim videosuna erişmek için, önceki eğitim videosundaki görevi tamamlamanız gerekir.";
                    TextWebSite.Test = "Bu eğitim videosuna erişmek için, önceki eğitim videosundaki testi geçmeniz ve iyi bir not almanız gerekir.";

                    TextWebSite.PassedLesson = "Eğitim videosu alın";
                    TextWebSite.PassedTask = "Görevi tamamla";
                    TextWebSite.PassedTest = "Teste katılın";


                    break;


                case "en":


                    TextWebSite.Title = "All video tutorials";

                    TextWebSite.Lesson = "To access this video tutorial, you need to complete the previous tutorial.";
                    TextWebSite.Task = "To access this video tutorial, you need to complete the task from the previous video tutorial.";
                    TextWebSite.Test = "To access this video tutorial, you need to pass the test from the previous video tutorial and get a good grade.";

                    TextWebSite.PassedLesson = "Take a video tutorial";
                    TextWebSite.PassedTask = "Perform the task";
                    TextWebSite.PassedTest = "Take the test";


                    break;
            }



            return TextWebSite;

        }
        
        public static TextTestsСs TextTests(string language)
        {

            TextTestsСs TextWebSite = new TextTestsСs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Notbad = "Жаман емес, бірақ жұмыс жасау керек.";
                    TextWebSite.Good = "Жақсы, осылай жалғыстыра беріңіз.";
                    TextWebSite.Excellent = "Тамаша, осылай жалғыстыра беріңіз.";
                    TextWebSite.TotalText = "Жинаған баллыңыз: ";

                    TextWebSite.ResultTest = "Тест нәтижесі";
                    TextWebSite.RepeatTest = "Тестті қайтадан өту";
                    TextWebSite.NextVideo = "Келесі видеодәріс";
                    TextWebSite.ListVideos = "Видеодәрістер тізімі";
                    TextWebSite.QuizText = "Саулнамадан өтіңіз";
                    break;


                case "ru":

                    TextWebSite.Notbad = "Неплохо, но есть над чем поработать.";
                    TextWebSite.Good = "Хорошо, продолжай в том же духе.";
                    TextWebSite.Excellent = "Отлично, так держать.";
                    TextWebSite.TotalText = "Вы набрали: ";

                    TextWebSite.ResultTest = "Результат теста ";
                    TextWebSite.RepeatTest = "Повторить тест";
                    TextWebSite.NextVideo = "Следующее видеоурок";
                    TextWebSite.ListVideos = "Список видеоуроков";
                    TextWebSite.QuizText = "Пройти Опрос";
                    break;

                case "uz":

                    TextWebSite.Notbad = "Yomon emas, lekin qilish kerak bo'lgan ishlar ham bor.";
                    TextWebSite.Good = "Xo'sh, ishni davom ettiring.";
                    TextWebSite.Excellent = "Ajoyib, uni ushlab turing.";
                    TextWebSite.TotalText = "Siz to'pladingiz: ";
                    break;


                case "kr":

                    TextWebSite.Notbad = "Жаман эмес, бирок муну менен иши жок.";
                    TextWebSite.Good = "Ооба, жакшы иштей берет.";
                    TextWebSite.Excellent = "Абдан жакшы, аны сактап калат.";
                    TextWebSite.TotalText = "Сиз топтодуңуз: ";
                    break;

                case "tr":

                    TextWebSite.Notbad = "Fena değil, ama yapacak işler var.";
                    TextWebSite.Good = "İyi işlere devam edin.";
                    TextWebSite.Excellent = "Harika, devam et.";
                    TextWebSite.TotalText = "Puan kazandınız: ";
                    break;

                case "en":

                    TextWebSite.Notbad = "Not bad, but there is work to do.";
                    TextWebSite.Good = "Well, keep up the good work.";
                    TextWebSite.Excellent = "Great, keep it up.";
                    TextWebSite.TotalText = "You scored: ";
                    break;
            }



            return TextWebSite;

        }
        
        public static TextTasktendreamsCs TextTasktendreams(string language)
        {

            TextTasktendreamsCs TextWebSite = new TextTasktendreamsCs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Title = "Тапсырма";
                    TextWebSite.Task_First_Title = "Осы бизнесте қол жеткізгіңіз келетін 10 арман туралы жазыңыз.";
                    TextWebSite.SaveBtn = "Сақтау";


                    break;


                case "ru":


                    TextWebSite.Title = "Задания";
                    TextWebSite.Task_First_Title = "Запишите 10 ваших конкретных мечты, которые Вы хотите достичь в этом бизнесе.";
                    TextWebSite.SaveBtn = "Сохранить";

                    break;

                case "uz":


                    TextWebSite.Title = "Missiyalar";
                    TextWebSite.Task_First_Title = "Ushbu biznesda erishmoqchi bo'lgan aniq 10 ta orzularingizni yozing.";
                    TextWebSite.SaveBtn = "Saqlash";

                    break;

                case "kr":


                    TextWebSite.Title = "Тапшырмалар";
                    TextWebSite.Task_First_Title = "Сиздин атайын эсепке 10 Эгер бул бизнеске жетүү үчүн келет деп кыялданат.";
                    TextWebSite.SaveBtn = "Кармоо";

                    break;

                case "tr":


                    TextWebSite.Title = "Atamaları";
                    TextWebSite.Task_First_Title = "Bu işte başarmak istediğiniz 10 hayalinizi yazın.";
                    TextWebSite.SaveBtn = "Tutmak";

                    break;

                case "en":


                    TextWebSite.Title = "Tasks";
                    TextWebSite.Task_First_Title = "Write down 10 of your specific dreams that you want to achieve in this business.";
                    TextWebSite.SaveBtn = "Save";

                    break;
            }



            return TextWebSite;

        }
        
        public static VideoLessonCs VideoLesson(string language)
        {

            VideoLessonCs TextWebSite = new VideoLessonCs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Next = "Келесі";
                    TextWebSite.Previous = "Алдыңғы";
                    

                    break;


                case "ru":


                    TextWebSite.Next = "Следующий";
                    TextWebSite.Previous = "Предыдущий";


                    break;

                case "uz":


                    TextWebSite.Next = "Keyingi";
                    TextWebSite.Previous = "Oldingi";


                    break;

                case "kr":


                    TextWebSite.Next = "Кийинки";
                    TextWebSite.Previous = "Мурунку";


                    break;

                case "tr":


                    TextWebSite.Next = "Aşağıdaki";
                    TextWebSite.Previous = "önceki";
                    ;

                    break;

                case "en":


                    TextWebSite.Next = "Tasks";
                    TextWebSite.Previous = "Previous";
                    break;
            }



            return TextWebSite;

        }

        public static QuizCs Quiz(string language)
        {

            QuizCs TextWebSite = new QuizCs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Title = "Cауланама";
                    TextWebSite.Prev = "Келесі";
                    TextWebSite.Next = "Алдыңғы";
                    TextWebSite.Complete = "Аяқтау";

                 
                    break;


                case "ru":

                    TextWebSite.Title = "Опрос";
                    TextWebSite.Prev = "Предыдущий";
                    TextWebSite.Next = "Cледующий";
                    TextWebSite.Complete = "Завершить";


                    break;

             
            }



            return TextWebSite;

        }


    }



    public partial class VideoLessonCs
    {

        public string Next { get; set; }

        public string Previous { get; set; }

        public string Quiz { get; set; }



    }

    public partial class TextTasktendreamsCs
    {

        public string Title { get; set; }

        public string Task_First_Title { get; set; }
        public string SaveBtn { get; set; }


    }



    public partial class TextLayout
    {

        public string MyProfile { get; set; }
        public string Modules { get; set; }
        public string Lessons { get; set; }
        public string Tasks { get; set; }
        public string Favorite { get; set; }
        public string UsefulServices { get; set; }
        public string ByPr { get; set; }

        public string MyProgress { get; set; }

        public string Passed { get; set; }

        public string Performance { get; set; }
        public string Reserved { get; set; }

        public string Logout { get; set; }

    }



    public partial class TextVideosСs
    {

        public string Title { get; set; }

        public string Test { get; set; }
        public string Task { get; set; }
        public string Lesson { get; set; }

        public string PassedTest { get; set; }
        public string PassedTask { get; set; }
        public string PassedLesson { get; set; }

    }



    public partial class TextTestsСs
    {
        public string ResultTest { get; set; }
        public string RepeatTest { get; set; }
        public string NextVideo { get; set; }
        public string ListVideos { get; set; }

        public string QuizText { get; set; }
        public string Notbad { get; set; }
        public string Good { get; set; }
        public string Excellent { get; set; }
        public string TotalText { get; set; }


    }


    public partial class QuizCs
    {
        public string Title { get; set; }
        public string Prev { get; set; }
        public string Next { get; set; }
        public string Complete { get; set; }
    }


    public class MyProfileCs
    {

        public string Title { get; set; }
        public string VideoName { get; set; }
        public string EditBtn { get; set; }
        public string SaveBtn { get; set; }
       
    }
    
    public class UserProgress
    {

        public int OUK { get; set; }
        public int TBB { get; set; }
        public int NextV { get; set; }
        public JVLO JVLO { get; set; }
        public List<VideoXL> EnableVideoXLs { get; set; }
    }

    public class ModuleCs
    {
        public int ModuleId { get; set; }
        public string Image { get; set; }
        public string NameKZ { get; set; }
        public string NameRU { get; set; }
        public string NameUZ { get; set; }
        public string NameKR { get; set; }
        public string NameEN { get; set; }
        public string NameTR { get; set; }

        public int LevelId { get; set; }



    }


    public partial class Modules_Property_Enable
    {
        public Modules_Property Modul_Property_IN { get; set; }

        public bool Enable { get; set; }
    }


    public class paybox_get_sts
    {

        public paybox_get_sts_response response { get; set; }

    }


    public class paybox_get_sts_response
    {

        public string pg_transaction_status { get; set; }

        public int pg_captured { get; set; }

    }
    public class UserQuizArray
    {

        public string[][] data {get;set;}
    }

    public class MD5generator
    {


        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }



    }



    public partial class Quiz_A {


        public int QId { get; set; }

        public int Q_content_Id { get; set; }
        public string Quiz_Title { get; set; }

        public int User_Answer { get; set; }

        public string User_AnswerOther { get; set; }

        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public string E { get; set; }

        public bool prev { get; set; }

    }


    public partial class Quiz_B
    {


        public int QId { get; set; }

        public int Q_content_Id { get; set; }
        public string Quiz_Title { get; set; }
        public int User_Answer { get; set; }
    }



    public partial class Quiz_C
    {


        public int QId { get; set; }

        public int Q_content_Id { get; set; }
        public string Quiz_Title { get; set; }
        public string User_Answer { get; set; }

        public bool next{ get; set; }
    }


    public class SMSPhoneMessage
    {
        public string login { get; set; }
        public string psw { get; set; }
        public int fmt { get; set; }
        public string phones { get; set; }
        public string mes { get; set; }


    }

}