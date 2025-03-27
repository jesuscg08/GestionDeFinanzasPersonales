using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GestionDeFinanzasPersonales.Models;
using GestionDeFinanzasPersonales.Models.Database;
using GestionDeFinanzasPersonales.Models.Security;

namespace GestionDeFinanzasPersonales.Controllers
{
    public class UsuarioController : Controller
    {
        private GestionFinanzasPersonalesEntities2 db = new GestionFinanzasPersonalesEntities2();

        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        { // Limpia la sesión al cargar el login
            Session.Clear();
            FormsAuthentication.SignOut();

            return View();
        }

        // Post: Login Account
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                ViewBag.ErrorMessage = "TRABAJANDO";
                var user = db.Usuario.FirstOrDefault(
                    u => u.Correo == model.Correo
                    );

                if (user != null)
                {
                    if (PasswordHasher.VerificarClave(model.Clave, user.Clave))
                    {
                        //Correcta, Inicia sesión
                        FormsAuthentication.SetAuthCookie(user.Correo, false);
                        Session["Id"] = user.Id;
                        Session["Nombre"] = user.Nombre;
                        Session["Correo"] = user.Correo;

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Gestion");
                    }
                    else
                    {
                        ModelState.AddModelError("Clave", "Contraseña incorrecta");
                        ViewBag.ErrorMessage = "Correo o contraseña incorrectos";
                    }
                }
                else
                {
                    ModelState.AddModelError("Correo", "Usuario no encontrado");
                    ViewBag.ErrorMessage = "Usuario no encontrado";
                }

            }
            else {
                ViewBag.ErrorMessage = "NO VALIDO";
            }

                return View(model);
        }

        // GET: Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }


        //GET: Recuperar contraseña
        public ActionResult RecuperarClave() {
            return View();
        }

        //POST: Recuperar contraseña
        public ActionResult RecuperarClave(string correo) {
            var usuario = db.Usuario.FirstOrDefault(u=> u.Correo==correo);

            if (usuario != null)
            {

            }
            else {
                ModelState.AddModelError("", "No existe un usuario con ese correo electrónico");
            }

                return View();
        }


        // GET: Usuario/Registro
        public ActionResult Registro()
        {
            return View();
        }

        //POST: Registro de usuarios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(Usuario model) {


            if (ModelState.IsValid) 
            {
                //Verificación si hay usuarios ya registrados con el correo
                if (db.Usuario.Any(u=>u.Correo==model.Correo))
                {
                    ModelState.AddModelError("","El correo electrónico ya está registrado");

                    return View(model);
                }

                var nuevoUsuario= new Usuario 
                { Nombre=model.Nombre,
                  Correo=model.Correo,
                  Clave=PasswordHasher.HashClave(model.Clave)           
                };  

                //Agregar
                db.Usuario.Add(nuevoUsuario);
                db.SaveChanges();


                // Redireccionar con mensaje de éxito
                TempData["RegistroExitoso"] = "¡Registro exitoso! Por favor inicie sesión.";
                return RedirectToAction("Login", "Usuario");

            }


            return View(model);

        }

    }
}
