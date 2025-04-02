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
        private GestionFinanzasPersonalesEntities2 db = new GestionFinanzasPersonalesEntities2();

        // GET: Notificacion
        public ActionResult Index()
        {
            var notificacion = db.Notificacion.Include(n => n.Usuario);
            return View(notificacion.ToList());
        }

        // GET: Notificacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notificacion notificacion = db.Notificacion.Find(id);
            if (notificacion == null)
            {
                return HttpNotFound();
            }
            return View(notificacion);
        }

        // GET: Notificacion/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre");
            return View();
        }

        // POST: Notificacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNotificacion,IdUsuario,Titulo,Mensaje,Tipo,FechaCreacion,Leida")] Notificacion notificacion)
        {
            if (ModelState.IsValid)
            {
                db.Notificacion.Add(notificacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", notificacion.IdUsuario);
            return View(notificacion);
        }

        // GET: Notificacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notificacion notificacion = db.Notificacion.Find(id);
            if (notificacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", notificacion.IdUsuario);
            return View(notificacion);
        }

        // POST: Notificacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdNotificacion,IdUsuario,Titulo,Mensaje,Tipo,FechaCreacion,Leida")] Notificacion notificacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notificacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", notificacion.IdUsuario);
            return View(notificacion);
        }

        // GET: Notificacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notificacion notificacion = db.Notificacion.Find(id);
            if (notificacion == null)
            {
                return HttpNotFound();
            }
            return View(notificacion);
        }

        // POST: Notificacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notificacion notificacion = db.Notificacion.Find(id);
            db.Notificacion.Remove(notificacion);
            db.SaveChanges();
            return RedirectToAction("Index");
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
