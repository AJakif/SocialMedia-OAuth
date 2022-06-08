using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SocialBOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoacialOAuth.Controllers
{
    public class Account : Controller
    {
        const string relayStateReturnUrl = "ReturnUrl";
        private readonly IConfiguration _config;
        private readonly Saml2Configuration _Sconfig;
        

        public Account(IConfiguration config, IOptions<Saml2Configuration> configAccessor)
        {
            _config = config;
           _Sconfig = configAccessor.Value;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [Route("saml2")]
        public IActionResult Saml2(string returnUrl = null)
        {
            var binding = new Saml2RedirectBinding();
            binding.SetRelayStateQuery(new Dictionary<string, string> { { relayStateReturnUrl, returnUrl ?? Url.Content("~/") } });

            return binding.Bind(new Saml2AuthnRequest(_Sconfig)).ToActionResult();
        }


        public async Task<IActionResult> AssertionConsumerService()
        {
            var binding = new Saml2PostBinding();
            var saml2AuthnResponse = new Saml2AuthnResponse(_Sconfig);

            binding.ReadSamlResponse(Request.ToGenericHttpRequest(), saml2AuthnResponse);
            if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
            {
                throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");
            }
            return Redirect("/");
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
        public async Task Twitter()
        {
            await HttpContext.ChallengeAsync(TwitterDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("TwitterResponse")
            });
        }


        public async Task<IActionResult> TwitterResponse()
        {
            string ConnectionString = _config["ConnectionStrings:DefaultConnection"];
            var data = HttpContext.User.Identities.ToList()[0].Claims.ToList();

            Authentication ga = new()
            {
                TwitterId = data[0].Value,
                Email1 = data[4].Value,
                Status = "A"
            };
            await HttpContext.SignOutAsync();
            Authentication gao = SocialBLL.Authentication.GetLoginByIdentifierAndEmail("Twitter", ga.TwitterId, ga.Email1, ConnectionString);
            if (gao == null)
            {
                int result = 0;
                Authentication emailExist = SocialBLL.Authentication.GetEmail(ga.Email1, ConnectionString);
                if (emailExist == null)
                {
                    result = SocialBLL.Authentication.AddUserByNameIdentifierAndEmail("Twitter", ga, ConnectionString);
                }
                else
                {
                    Authentication nameExist = SocialBLL.Authentication.GetName(ga.Email1, ga.FirstName, ga.LastName, ConnectionString);

                    if (nameExist == null)
                    {
                        result = SocialBLL.Authentication.AddUserByNameAndIdentifie("Twitter", ga, ConnectionString);
                    }
                    else
                    {
                        result = SocialBLL.Authentication.AddUserByIdentifier("Twitter", ga, ConnectionString);
                    }
                }
                if (result > 0)
                {
                    Authentication go = SocialBLL.Authentication.GetLoginByIdentifierAndEmail("Twitter", ga.GoogleId, ga.Email1, ConnectionString);
                    var claims = new List<Claim>
                        {
                            new Claim("TId",go.TwitterId),
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
                            new Claim("TId",gao.TwitterId),
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
        public async Task Microsoft()
        {
            await HttpContext.ChallengeAsync(MicrosoftAccountDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("MicrosoftResponse")
            });
        }

        public async Task<IActionResult> MicrosoftResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });
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
            await HttpContext.SignOutAsync();
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
