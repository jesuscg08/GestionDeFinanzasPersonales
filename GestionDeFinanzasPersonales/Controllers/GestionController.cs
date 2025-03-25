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
    public class GestionController : Controller
    {
        private GestionFinanzasPersonalesEntities1 db = new GestionFinanzasPersonalesEntities1();

        // GET: Gestion
        public ActionResult Index()
        {
            var gestion = db.Gestion.Include(g => g.Categoria).Include(g => g.Usuario);
            return View(gestion.ToList());
        }

        // GET: Gestion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gestion gestion = db.Gestion.Find(id);
            if (gestion == null)
            {
                return HttpNotFound();
            }
            return View(gestion);
        }

        // GET: Gestion/Create
        public ActionResult Create()
        {
            ViewBag.IdCategoria = new SelectList(db.Categoria, "IdCategoria", "NombreCategoria");
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre");
            return View();
        }

        // POST: Gestion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdGestion,Monto,IdUsuario,IdCategoria")] Gestion gestion)
        {
            if (ModelState.IsValid)
            {
                db.Gestion.Add(gestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCategoria = new SelectList(db.Categoria, "IdCategoria", "NombreCategoria", gestion.IdCategoria);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", gestion.IdUsuario);
            return View(gestion);
        }

        // GET: Gestion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gestion gestion = db.Gestion.Find(id);
            if (gestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategoria = new SelectList(db.Categoria, "IdCategoria", "NombreCategoria", gestion.IdCategoria);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", gestion.IdUsuario);
            return View(gestion);
        }

        // POST: Gestion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdGestion,Monto,IdUsuario,IdCategoria")] Gestion gestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategoria = new SelectList(db.Categoria, "IdCategoria", "NombreCategoria", gestion.IdCategoria);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", gestion.IdUsuario);
            return View(gestion);
        }

        // GET: Gestion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gestion gestion = db.Gestion.Find(id);
            if (gestion == null)
            {
                return HttpNotFound();
            }
            return View(gestion);
        }

        // POST: Gestion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gestion gestion = db.Gestion.Find(id);
            db.Gestion.Remove(gestion);
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
