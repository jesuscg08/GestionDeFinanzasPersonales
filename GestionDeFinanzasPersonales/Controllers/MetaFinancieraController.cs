using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDeFinanzasPersonales.Models.Database;
using GestionDeFinanzasPersonales.Models.Services;

namespace GestionDeFinanzasPersonales.Controllers
{
    [Authorize]
    public class MetaFinancieraController : Controller
    {
        private GestionFinanzasPersonalesEntities2 db = new GestionFinanzasPersonalesEntities2();

        private readonly NotificacionService  _notificacionService;
        public MetaFinancieraController() 
        {
            _notificacionService = new NotificacionService(db);
        }

        // GET: MetaFinanciera
        public ActionResult Meta()
        {
            // Verificar que el usuario esté autenticado
            if (!User.Identity.IsAuthenticated || Session["Id"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var metaFinanciera = db.MetaFinanciera.Include(m => m.Usuario);
            return View(metaFinanciera.ToList());
        }

        // GET: MetaFinanciera/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetaFinanciera metaFinanciera = db.MetaFinanciera.Find(id);
            if (metaFinanciera == null)
            {
                return HttpNotFound();
            }
            return View(metaFinanciera);
        }

        // GET: MetaFinanciera/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre");
            return View();
        }

        // POST: MetaFinanciera/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMeta,IdUsuario,Nombre,TipoMeta,MontoObjetivo,MontoAcumulado,FechaInicio,FechaObjetivo")] MetaFinanciera metaFinanciera)
        {
            if (ModelState.IsValid)
            {
                metaFinanciera.IdUsuario = (int)Session["Id"];
                db.MetaFinanciera.Add(metaFinanciera);
                db.SaveChanges();

                // Usas el servicio de notificaciones// int idUsuario, string titulo, string mensaje, string tipo
                if (metaFinanciera.MontoAcumulado >= metaFinanciera.MontoObjetivo)
                {
                    _notificacionService.CrearNotificacionMeta
                    (metaFinanciera.IdUsuario,
                    "¡Meta Alcanzada!",
                    $"Has alcanzado tu meta '{metaFinanciera.Nombre}' de  {metaFinanciera.MontoObjetivo}",
                    "MetaAlcanzada"
                    );
                }

                if (metaFinanciera.FechaObjetivo.AddDays(-3) <= DateTime.Now)
                {
                    _notificacionService.CrearNotificacionMeta
                        (metaFinanciera.IdUsuario,
                        "¡Meta cerca!",
                        $"Tu meta '{metaFinanciera.Nombre}' vence el {metaFinanciera.FechaObjetivo:dd/MM/yyyy}",
                        "MetaAlcanzada"
                        );
                }


                return RedirectToAction("Meta");
            }

            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", metaFinanciera.IdUsuario);
            return View(metaFinanciera);
        }

        // GET: MetaFinanciera/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetaFinanciera metaFinanciera = db.MetaFinanciera.Find(id);
            if (metaFinanciera == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", metaFinanciera.IdUsuario);
            return View(metaFinanciera);
        }


        // POST: MetaFinanciera/Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMeta,IdUsuario,Nombre,TipoMeta,MontoObjetivo,MontoAcumulado,FechaInicio,FechaObjetivo")] MetaFinanciera metaFinanciera)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metaFinanciera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Meta");
            }
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", metaFinanciera.IdUsuario);
            return View(metaFinanciera);
        }

        //GET: Agregar al monto acumulado
        public ActionResult AddMontoAcumulado(int? id)
        {
            MetaFinanciera metaFinanciera = db.MetaFinanciera.Find(id);
            if (metaFinanciera == null)
            {
                return HttpNotFound();
            }
            return View(metaFinanciera);
        }

        //POST: Agregar al monto acumulado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMontoAcumulado(int id, FormCollection form)
        {
            var meta = db.MetaFinanciera.Find(id);

            if (meta == null)
            {
                return HttpNotFound();
            }

            //Monto a agregar
            if (decimal.TryParse(form["montoAAgregar"], out decimal montoAAgregar) && montoAAgregar>0) 
            {
                if (meta.MontoAcumulado == null)
                {
                    meta.MontoAcumulado = montoAAgregar;
                }
                else
                {
                    meta.MontoAcumulado += montoAAgregar;
                }
                   

                //Notificar si el monto ya llego a la meta
                if (meta.MontoAcumulado>=meta.MontoObjetivo) { 
                
                }


            }


            if (ModelState.IsValid)
            {
                db.Entry(meta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Meta");
            }
            return View(meta);

        }



        // GET: MetaFinanciera/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetaFinanciera metaFinanciera = db.MetaFinanciera.Find(id);
            if (metaFinanciera == null)
            {
                return HttpNotFound();
            }
            return View(metaFinanciera);
        }

        // POST: MetaFinanciera/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MetaFinanciera metaFinanciera = db.MetaFinanciera.Find(id);
            db.MetaFinanciera.Remove(metaFinanciera);
            db.SaveChanges();
            return RedirectToAction("Meta");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
