using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnlineCourseZertis.Models;
using OnlineCourseZertis.App_Start;

namespace OnlineCourseZertis.Controllers
{
   
    public class AccountController : Controller
    {

        private Entities db = new Entities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        
        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl,string language)
        {
            if (language == null) language = "ru";
            ViewBag.language = language;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl,string language)
        {

            if (language == null) language = "ru";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ViewBag.language = language;

            // Сбои при входе не приводят к блокированию учетной записи
            // Чтобы ошибки при вводе пароля инициировали блокирование учетной записи, замените на shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Initialization", "User", new { language = model.Language });
                default:
                    ModelState.AddModelError("", "Неудачная попытка входа.");
                    return View(model);
            }


        }


        //
        // GET: /Account/Register
        public ActionResult Register(int? transition,string language)
        {
            if (language == null) language = "ru";

            ViewBag.language = language;
            ViewBag.transition = transition;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model,int? transition,string language)
        {

            if (language == null) language = "ru";


            if (ModelState.IsValid)
            {
                
                //-----Create (Random Password)
                Random random = new Random();
                var chars = "0123456789".ToCharArray();

                
                var pw = new char[6];
                for (int i = 0; i < 6; i++)
                {
                    pw[i] = chars[random.Next(chars.Length)];
                }
                string Password = new string(pw);


                var user = new ApplicationUser { UserName = model.PhoneNumber,Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {

                    UserInfo Userinfo = db.UserInfoes.Where(e => e.UserName == model.PhoneNumber).FirstOrDefault();
                    if (Userinfo == null)
                    {
                        UserInfo newUserInfo = new UserInfo();
                        newUserInfo.UserName = model.PhoneNumber;
                        newUserInfo.Name = model.Name;
                        newUserInfo.SureName = model.SureName;
                        newUserInfo.LevelId = 0;
                        newUserInfo.RegTime = DateTime.Now.AddHours(6);
                        db.UserInfoes.Add(newUserInfo);
                    }


                    JVLO jv = db.JVLOes.Where(e => e.UserName == model.PhoneNumber).FirstOrDefault();
                    if (jv == null)
                    {
                        JVLO jvnew = new JVLO();
                        jvnew.OV = 0;
                        jvnew.TBB = 0;
                        jvnew.UserName = model.PhoneNumber;
                        jvnew.X = 1;
                        db.JVLOes.Add(jvnew);
                    }


                    db.SaveChanges();

                    string MSG = "Vash parol'" + " - " + Password;
                    MSG += "\n";
                    MSG += "online.zertis.biz";
                    

                    bool sts =  await SMSPhone.sendPassword(model.PhoneNumber, MSG);

                    if (user.Email != null)
                    {
                        string bodyMSG = @"<table border='0' cellspacing='0' cellpadding='0' style='max-width:600px'><tbody><tr><td><table width='100%' border='0' cellspacing='0' cellpadding='0'><tbody>
                        <tr><td align='left'><img width='92' height='32' src='https://online.zertis.biz/Images/8f052742e2ebac31ce5c69dd70c8ef75.png' style='display:block;width:92px;height:32px' class='CToWUd'></td><td align='right'></td></tr>
                        </tbody></table></td></tr><tr><td><table bgcolor='#4caf50' width='100%' border='0' cellspacing='0' cellpadding='0' style='min-width:332px;max-width:600px;border:1px solid #e0e0e0;border-bottom:0;border-top-left-radius:3px;border-top-right-radius:3px'>
                        <tbody><tr style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:24px;color:#ffffff;line-height:1.25;text-align:center'><td height='72px' colspan='3'>" + model.PhoneNumber + @"</td></tr><tr><td width='32px'></td><td style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:24px;color:#ffffff;line-height:1.25'>Код подтверждения для аккаунта Online Zertis</td>
                        <td width='32px'></td></tr><tr><td height='18px' colspan='3'></td></tr></tbody></table></td></tr><tr><td>
                        <table bgcolor='#FAFAFA' width='100%' border='0' cellspacing='0' cellpadding='0' style='min-width:332px;max-width:600px;border:1px solid #f0f0f0;border-bottom:1px solid #c0c0c0;border-top:0;border-bottom-left-radius:3px;border-bottom-right-radius:3px'>
                        <tbody><tr height='16px'><td width='32px' rowspan='3'></td><td></td><td width='32px' rowspan='3'></td></tr><tr>
                        <td><div style='text-align:center'><p dir='ltr'><strong style='text-align:center;font-size:24px;font-weight:bold'>" + Password + "</strong></p></div></td></tr><tr height='32px'></tr></tbody></table></td></tr></tbody></table>";

                        try
                        {
                            await UserManager.SendEmailAsync(user.Id, "ZERTIS-Код подтверждения для аккаунта", bodyMSG);
                            sts = true;
                        }
                        catch
                        {
                            if (!sts) { sts = false; }
                        }

                    }

                    if (sts)
                    {
                        return RedirectToAction("ConfirmPassword", "Account", new { userId = user.Id, transition = transition, language=language});
                    }
                }
                else {
               
                    ModelState.AddModelError("", "Имя пользователя уже используется");
                }
           
            }

            ViewBag.language = language;

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }


        //
        // GET: /Account/ConfirmPassword
        public ActionResult ConfirmPassword(string userId,int? transition,string language)
        {

            if (language == null) language = "ru";

            ViewBag.language = language;
            ViewBag.transition = transition;
            ViewBag.userId = userId;
            return View();
        }




        //
        // POST: /Account/ConfirmPassword
        [HttpPost]
        public async Task<ActionResult> ConfirmPassword(ChangePasswordViewModel model, string userId,int? transition,string language)
        {

            if (language == null) language = "ru";
            string Message = "Success";
            
      
            try
            {

                
                if (ModelState.IsValid)
                {
                    var result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                        
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);



                            if (transition != null)
                            {
                                return RedirectToAction("transition", "User", new { language = language, transition = transition });

                            }


                            return RedirectToAction("Modules", "Learning", new { langugae = language });

                        }
                        
                    }
                    else
                    {

                        ModelState.AddModelError("", "Неправильный CМС пароль.");

                    }


                }

                
            }
            catch
            {
                Message = "Error";
            }


            ViewBag.Message = Message;
            ViewBag.userId = userId;

            ViewBag.language = language;
            return View();
        }
        

        // GET: /Account/ForgotPassword
        public ActionResult ForgotPassword(string language)
        {
            if (language == null) language = "ru";
            ViewBag.language = language;
            return View();
        }


        //
        // POST: /Account/ForgotPassword
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string UserName, string language)
        {
            if (language == null) language = "ru";
            var user = await UserManager.FindByNameAsync(UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Имя пользователя неверно.");
                return View();
            }


            //-----Create (Random Password)
            Random random = new Random();
            var chars = "0123456789".ToCharArray();
            var pw = new char[6];
            for (int i = 0; i < 6; i++)
            {
                pw[i] = chars[random.Next(chars.Length)];
            }
            string Password = new string(pw);

            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

          

            var result = await UserManager.ResetPasswordAsync(user.Id, code, Password);

            if(result.Succeeded){


                string MSG = "Vash parol'" + " - " + Password;
                MSG += "\n";
                MSG += "online.zertis.biz";
                
                bool sts = await SMSPhone.sendPassword(UserName, MSG);


                if (user.Email != null)
                {
                    string bodyMSG = @"<table border='0' cellspacing='0' cellpadding='0' style='max-width:600px'><tbody><tr><td><table width='100%' border='0' cellspacing='0' cellpadding='0'><tbody>
                    <tr><td align='left'><img width='92' height='32' src='https://online.zertis.biz/Images/8f052742e2ebac31ce5c69dd70c8ef75.png' style='display:block;width:92px;height:32px' class='CToWUd'></td><td align='right'></td></tr>
                    </tbody></table></td></tr><tr><td><table bgcolor='#4caf50' width='100%' border='0' cellspacing='0' cellpadding='0' style='min-width:332px;max-width:600px;border:1px solid #e0e0e0;border-bottom:0;border-top-left-radius:3px;border-top-right-radius:3px'>
                    <tbody><tr style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:24px;color:#ffffff;line-height:1.25;text-align:center'><td height='72px' colspan='3'>"+UserName+ @"</td></tr><tr><td width='32px'></td><td style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:24px;color:#ffffff;line-height:1.25'>Код подтверждения для аккаунта Online Zertis</td>
                    <td width='32px'></td></tr><tr><td height='18px' colspan='3'></td></tr></tbody></table></td></tr><tr><td>
                    <table bgcolor='#FAFAFA' width='100%' border='0' cellspacing='0' cellpadding='0' style='min-width:332px;max-width:600px;border:1px solid #f0f0f0;border-bottom:1px solid #c0c0c0;border-top:0;border-bottom-left-radius:3px;border-bottom-right-radius:3px'>
                    <tbody><tr height='16px'><td width='32px' rowspan='3'></td><td></td><td width='32px' rowspan='3'></td></tr><tr>
                    <td><div style='text-align:center'><p dir='ltr'><strong style='text-align:center;font-size:24px;font-weight:bold'>" + Password+"</strong></p></div></td></tr><tr height='32px'></tr></tbody></table></td></tr></tbody></table>";

                    try
                    {
                        await UserManager.SendEmailAsync(user.Id, "ZERTIS -Код подтверждения для аккаунта", bodyMSG);
                        sts = true;
                    }
                    catch
                    {
                        if (!sts) { sts = false; }
                    }

                }
                
              
                if (sts)
                {
                    return RedirectToAction("ConfirmPassword", "Account", new { userId = user.Id, language = language });
                }


            }

            ViewBag.language = language;

            return View();
        }

        

        public ActionResult ErrorLogOff(string language, string ActionName)
        {
            if (language == null) { language = "ru"; }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account", new { language = language });
        }

        
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(int? result,string language)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account",new { language= language });
        }

        //
        // GET: /Account/ExternalLoginFailure
  
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}