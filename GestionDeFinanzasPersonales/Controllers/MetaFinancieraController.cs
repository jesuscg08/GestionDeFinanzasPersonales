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
    [Authorize]
    public class MetaFinancieraController : Controller
    {
        private GestionFinanzasPersonalesEntities2 db = new GestionFinanzasPersonalesEntities2();

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

        // POST: MetaFinanciera/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
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
