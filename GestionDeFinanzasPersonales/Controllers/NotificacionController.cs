using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDeFinanzasPersonales.Models.Database;

namespace GestionDeFinanzasPersonales.Controllers
{
    public class NotificacionController : Controller
    {
        private readonly GestionFinanzasPersonalesEntities2 _db;

        public NotificacionController()
        {
            _db = new GestionFinanzasPersonalesEntities2();
        }

        [ChildActionOnly]
        public ActionResult _NotificacionesWidget()
        {

            var userId = (int)Session["Id"];
            var notificaciones = _db.Notificacion
                .Where(n => n.IdUsuario == userId)
                .OrderByDescending(n => n.FechaCreacion)
                .Take(5)
                .ToList();

            return PartialView("_Notificaciones", notificaciones);
        }

        [HttpPost]
        public ActionResult MarcarComoLeidas()
        {
            var userId = (int)Session["Id"];
            
            var notificaciones = _db.Notificacion
                .Where(n => n.IdUsuario == userId && !n.Leida.Value)
                .ToList();

            notificaciones.ForEach(n => n.Leida = true);
            _db.SaveChanges();

            return Json(new { success = true });
        }

    }
}
