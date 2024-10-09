using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ZatratyCore.Models;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace ZatratyCore.Controllers
{
    public class AccountController : Controller
    {
        public ZatratyContext db1;
        public AccountController(ZatratyContext context)
        {
            db1 = context;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {
            List<Autorize> ListUser = new List<Autorize>();
            ListUser = db1.Autorizes.ToList();

            Autorize us = new Autorize();
            us = ListUser.FirstOrDefault(g => g.Login == modelLogin.Login & g.Password == md5.hashPassword(modelLogin.PassWord));
            //us = ListUser.FirstOrDefault(g => g.Login == modelLogin.Login & g.Password == modelLogin.PassWord);
            //определяем роли аутенцифицированного пользователя


            if (us != null)
            {
                List<Role> usrol = new List<Role>();
                usrol = db1.Roles.Include(h => h.Plans).Where(r => r.FillialId == us.FillialId).ToList();

                List<Claim> claims = new List<Claim>() {
                    
                    new Claim("Admin", us.Admin.ToString()),
                    new Claim(ClaimTypes.Name, modelLogin.Login),
                    
                    
                    new Claim("OtherProperties","Example Role")

                };

                foreach (var u in usrol)
                {
                    claims.Add(new Claim(ClaimTypes.Role, u.PlansId.ToString()));
                }


                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    //IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);


                return RedirectToAction("Index", "Home");
            }



            ViewData["ValidateMessage"] = "Неверно введены даные!!!";
            return View();
        }
    }
}
