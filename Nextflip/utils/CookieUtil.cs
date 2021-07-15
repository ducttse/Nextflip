using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Nextflip.Models.account;

namespace Nextflip.utils
{
    public class CookieUtil : CookieAuthenticationEvents
    {

        public override async Task ValidatePrincipal(
            CookieValidatePrincipalContext context)
        {
            if (context?.Principal?.Claims != null)
            {
                string roleInCookie = null;
                string roleInDatabase = null;
                string userId = null;
                foreach (var claim in context.Principal.Claims)
                {
                    if (claim.Type == ClaimTypes.Role)
                    {
                        roleInCookie = claim.Value;
                    }

                    if (claim.Type == ClaimTypes.Name)
                    {
                        userId = claim.Value;
                    }
                }
                if (roleInCookie != null && userId != null && roleInCookie != "subcribed user")
                {
                    roleInDatabase = new AccountDAO().GetAccountByID(userId).roleName;
                    if (roleInDatabase != roleInCookie)
                    {
                        /*    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, userId),
                                new Claim(ClaimTypes.Role, roleInDatabase),
                            };
                            var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            context.ReplacePrincipal(new ClaimsPrincipal(claimsIdentity));
                        */
                        context.RejectPrincipal();
                        await context.HttpContext.SignOutAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
            }
        }
    }
}
