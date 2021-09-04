using ElBuenSaborAdmin.Data;
using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult PerformLogin([Bind] Usuario usuario)
        //{
        //    if ((!string.IsNullOrEmpty(usuario.NombreUsuario)) && (!string.IsNullOrEmpty(usuario.Clave)))
        //    {
        //        var encontrado = _context.Usuarios.Where(a => a.Disabled.Equals(false)).Where(u => u.NombreUsuario == usuario.NombreUsuario).FirstOrDefault();
        //        var clave = GetSHA256(usuario.Clave);

        //        if (encontrado != null)
        //        {
        //            if ((encontrado.RolId.Equals(2) && encontrado.Clave.Equals(clave)))
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }

        //    }
        //    return View("Index");
        //}

        [HttpPost]
        public async Task<IActionResult> PerformLogin([Bind] Usuario usuario)
        {
            if ((!string.IsNullOrEmpty(usuario.NombreUsuario)) && (!string.IsNullOrEmpty(usuario.Clave)))
            {

                var encontrado = _context.Usuarios.Where(a => a.Disabled.Equals(false)).Where(u => u.NombreUsuario == usuario.NombreUsuario).FirstOrDefault();
                var clave = GetSHA256(usuario.Clave);

                if (encontrado != null)
                {
                    if ((encontrado.RolId.Equals(2) && encontrado.Clave.Equals(clave)))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                            new Claim(ClaimTypes.Role, "User"),
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.Now.AddMinutes(10),
                        };

                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                }

            }
            return View("Index");
        }

        [NonAction]
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
