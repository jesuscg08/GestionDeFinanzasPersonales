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
    public class GestionController : Controller
    {
        private GestionFinanzasPersonalesEntities2 db = new GestionFinanzasPersonalesEntities2();

        // GET: Gestion
        
        public ActionResult Index()
        {

            // Verificar que el usuario esté autenticado
            if (!User.Identity.IsAuthenticated || Session["Id"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Tu lógica para el dashboard
            var userId = (int)Session["Id"];
            var gestionesUsuario = db.Gestion
                               .Where(g => g.IdUsuario == userId)
                               .ToList();
            //var gestion = db.Gestion.Include(g => g.Tipo).Include(g => g.Usuario);
            return View(gestionesUsuario);
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
            ViewBag.IdTipo = new SelectList(new List<Tipo>(), "IdTipo", "NombreTipo");
            

            return View();
        }

        //Metodo para obtener los tipos según las categorias
        public JsonResult GetTiposByCategoria(int idCategoria) {
            var tipos = db.Tipo
                    .Where(t => t.IdCategoria == idCategoria)
                    .Select(t => new { t.IdTipo, t.NombreTipo })
                    .ToList();
            return Json(tipos, JsonRequestBehavior.AllowGet);
        }

       
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdGestion,Monto,IdUsuario,IdTipo")] Gestion gestion)
        {
            if (ModelState.IsValid)
            {
                // Asignar automáticamente el usuario logueado
                gestion.IdUsuario = (int)Session["Id"];

                db.Gestion.Add(gestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategoria = new SelectList(db.Categoria, "IdCategoria", "NombreCategoria");
            ViewBag.IdTipo = new SelectList(db.Tipo, "IdTipo", "NombreTipo", gestion.IdTipo);
            
            return View(gestion);

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
            ViewBag.IdTipo = new SelectList(db.Tipo, "IdTipo", "NombreTipo", gestion.IdTipo);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "Id", "Nombre", gestion.IdUsuario);
            return View(gestion);
        }

        // POST: Gestion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdGestion,Monto,IdUsuario,IdTipo")] Gestion gestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTipo = new SelectList(db.Tipo, "IdTipo", "NombreTipo", gestion.IdTipo);
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
