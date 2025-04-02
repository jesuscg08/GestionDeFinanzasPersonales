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
    public class PresupuestoController : Controller
    {
        private GestionFinanzasPersonalesEntities2 db = new GestionFinanzasPersonalesEntities2();

        // GET: Presupuesto
        public ActionResult Presupuesto()
        {
            var presupuesto = db.Presupuesto.Include(p => p.CategoriaPresupuesto).Include(p => p.Usuario);
            return View(presupuesto.ToList());
        }

        // GET: Presupuesto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presupuesto presupuesto = db.Presupuesto.Find(id);
            if (presupuesto == null)
            {
                return HttpNotFound();
            }
            return View(presupuesto);
        }

        // GET: Presupuesto/Create
        public ActionResult Create()
        {
            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto, "IdCategoriaPresupuesto", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre");
            return View();
        }

        // POST: Presupuesto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPresupuesto,IdUsuario,IdCategoriaPresupuesto,Monto,Mes,Año")] Presupuesto presupuesto)
        {
            if (ModelState.IsValid)
            {
                presupuesto.IdUsuario = (int)Session["Id"];

                db.Presupuesto.Add(presupuesto);
                db.SaveChanges();
                return RedirectToAction("Presupuesto");
            }

            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto, "IdCategoriaPresupuesto", "Nombre", presupuesto.IdCategoriaPresupuesto);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", presupuesto.IdUsuario);
            return View(presupuesto);
        }

        // GET: Presupuesto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presupuesto presupuesto = db.Presupuesto.Find(id);
            if (presupuesto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto, "IdCategoriaPresupuesto", "Nombre", presupuesto.IdCategoriaPresupuesto);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", presupuesto.IdUsuario);
            return View(presupuesto);
        }

        // POST: Presupuesto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPresupuesto,IdUsuario,IdCategoriaPresupuesto,Monto,Mes,Año")] Presupuesto presupuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presupuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Presupuesto");
            }
            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto, "IdCategoriaPresupuesto", "Nombre", presupuesto.IdCategoriaPresupuesto);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", presupuesto.IdUsuario);
            return View(presupuesto);
        }

        // GET: Presupuesto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presupuesto presupuesto = db.Presupuesto.Find(id);
            if (presupuesto == null)
            {
                return HttpNotFound();
            }
            return View(presupuesto);
        }

        // POST: Presupuesto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Presupuesto presupuesto = db.Presupuesto.Find(id);
            db.Presupuesto.Remove(presupuesto);
            db.SaveChanges();
            return RedirectToAction("Presupuesto");
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
