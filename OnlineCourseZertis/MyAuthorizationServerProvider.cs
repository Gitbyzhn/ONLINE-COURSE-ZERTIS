using OnlineCourseZertis.Models;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;


namespace OnlineCourseZertis
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override async System.Threading.Tasks.Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async System.Threading.Tasks.Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);


            if (user == null)
            {
                context.SetError("invalid_grant", "Имя пользователя или пароль указаны неправильно.");
                return;
            }


            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("username", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            context.Validated(identity);


        }
    }
}