using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
//using GoogleLogin.Services;

namespace GoogleLogin.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal?.Identities.FirstOrDefault()?.Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            //return Json(claims); // returning json data

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return View("Index");
        }

        //    public async Task<IActionResult> GoogleResponse()
        //    {
        //        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //        var claims = result.Principal?.Identities.FirstOrDefault()?.Claims;

        //        if (claims == null)
        //        {
        //            return Unauthorized("Authentication failed.");
        //        }

        //        var userClaims = claims.ToDictionary(claim => claim.Type, claim => claim.Value);

        //        // Generate JWT token using TokenService
        //        var tokenService = HttpContext.RequestServices.GetService<TokenService>();
        //        var token = tokenService.GenerateJwtToken(new Dictionary<string, string>
        //{
        //    { "Email", userClaims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] },
        //    { "Name", userClaims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] }
        //});

        //        return Ok(new
        //        {
        //            Token = token,
        //            User = new
        //            {
        //                Email = userClaims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
        //                Name = userClaims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]
        //            }
        //        });
        //    }


    }
}
