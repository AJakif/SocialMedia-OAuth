using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SocialBOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoacialOAuth.Controllers
{
    public class Account : Controller
    {
        private readonly IConfiguration _config;

        public Account(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [Route("facebook-login")]
        public IActionResult Facebook()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [Route("facebook-response")]
        public async Task<IActionResult> FacebookResponse()
        {
            string ConnectionString = _config["ConnectionStrings:DefaultConnection"];
            var data = HttpContext.User.Identities.ToList()[0].Claims.ToList();

            Authentication ga = new()
            {
                FacebookId = data[0].Value,
                FirstName = data[3].Value,
                LastName = data[4].Value,
                Email1 = data[1].Value,
                Status = "A"
            };


            Authentication gao = SocialBLL.Authentication.GetLoginByIdentifierAndEmail("Facebook", ga.FacebookId, ga.Email1, ConnectionString);
            if (gao == null)
            {
                int result = 0;
                Authentication emailExist = SocialBLL.Authentication.GetEmail(ga.Email1, ConnectionString);
                if(emailExist == null)
                {
                    result = SocialBLL.Authentication.AddUserByNameIdentifierAndEmail("Facebook", ga, ConnectionString);
                }
                else
                {
                    Authentication nameExist = SocialBLL.Authentication.GetName(ga.Email1, ga.FirstName, ga.LastName, ConnectionString);
                    
                    if (nameExist == null)
                    {
                        result = SocialBLL.Authentication.AddUserByNameAndIdentifie("Facebook", ga, ConnectionString);
                    }
                    else
                    {
                        result = SocialBLL.Authentication.AddUserByIdentifier("Facebook", ga, ConnectionString);
                    }
                }
                
                if (result > 0)
                {
                    Authentication go = SocialBLL.Authentication.GetLoginByIdentifierAndEmail("Facebook", ga.FacebookId, ga.Email1, ConnectionString);
                    var claims = new List<Claim>
                        {
                            new Claim("FId",go.FacebookId),
                            new Claim("Email", go.Email1)

                        };
                    var identity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
                    return View(go); //Redirects to Home accounts index view
                }
            }
            else if (gao != null && gao.FacebookId != null) //all data should be null checked
            {
                var claims = new List<Claim>
                        {
                            new Claim("FId",gao.FacebookId),
                            new Claim("Name", gao.FullName)

                        };
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

                return View(gao);

            }
            else
            {
                return View();
            }
            return View();
        }


        [HttpGet]
        public async Task Google()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            string ConnectionString = _config["ConnectionStrings:DefaultConnection"];
            var data = HttpContext.User.Identities.ToList()[0].Claims.ToList();
            Authentication ga = new()
            {
                GoogleId = data[0].Value,
                FirstName = data[2].Value,
                LastName = data[3].Value,
                Email1 = data[4].Value,
                Status = "A"
            };
            Authentication gao = SocialBLL.Authentication.GetLoginByIdentifierAndEmail("Google",ga.GoogleId, ga.Email1, ConnectionString);
            if (gao == null)
            {
                int result = 0;
                Authentication emailExist = SocialBLL.Authentication.GetEmail(ga.Email1, ConnectionString);
                if (emailExist == null)
                {
                    result = SocialBLL.Authentication.AddUserByNameIdentifierAndEmail("Google", ga, ConnectionString);
                }
                else
                {
                    Authentication nameExist = SocialBLL.Authentication.GetName(ga.Email1, ga.FirstName, ga.LastName, ConnectionString);

                    if (nameExist == null)
                    {
                        result = SocialBLL.Authentication.AddUserByNameAndIdentifie("Google", ga, ConnectionString);
                    }
                    else
                    {
                        result = SocialBLL.Authentication.AddUserByIdentifier("Google", ga, ConnectionString);
                    }
                }
                if (result > 0)
                {
                    Authentication go = SocialBLL.Authentication.GetLoginByIdentifierAndEmail("Google", ga.GoogleId, ga.Email1, ConnectionString);
                    var claims = new List<Claim>
                        {
                            new Claim("GId",go.GoogleId),
                            new Claim("Email", go.Email1)

                        };
                    var identity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
                    return View(go); //Redirects to Home accounts index view
                }
            }
            else if (gao != null && gao.GoogleId != null) //all data should be null checked
            {
                var claims = new List<Claim>
                        {
                            new Claim("GId",gao.GoogleId),
                            new Claim("Name", gao.FullName)

                        };
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

                return View(gao);

            }
            else
            {
                return View();
            }

            //var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //var claims = result.Principal.Identities
            //    .FirstOrDefault().Claims.Select(claim => new
            // {
            //     claim.Issuer,
            //    claim.OriginalIssuer,
            //    claim.Type,
            //    claim.Value
            // });
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToRoute("home");
        }
    }
}
