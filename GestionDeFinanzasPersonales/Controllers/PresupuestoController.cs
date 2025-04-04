using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
            // Verificar que el usuario esté autenticado
            if (!User.Identity.IsAuthenticated || Session["Id"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            var userId = (int)Session["Id"];
            var presupuesto = db.Presupuesto
                               .Where(g => g.IdUsuario == userId)
                               .ToList();
            return View(presupuesto);
        }

        // GET: Presupuesto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var presupuesto = db.Presupuesto
                .Include(p=>p.CategoriaPresupuesto)
                .Include(p=>p.Usuario)
                .FirstOrDefault(p=>p.IdPresupuesto==id);

            
            if (presupuesto == null)
            {
                return HttpNotFound();
            }

            var movimientos = db.Gestion
                .Where(
                g=>g.IdCategoriaPresupuesto==presupuesto.IdCategoriaPresupuesto &&
                g.FechaOperacion.Month==presupuesto.Mes &&
                g.FechaOperacion.Year==presupuesto.Año &&
                g.IdUsuario==presupuesto.IdUsuario)
                .Include(g=>g.Tipo)
                .ToList();

            ViewBag.Movimientos = movimientos;
            ViewBag.TotalGastado = movimientos.Sum(g => g.Monto);
            ViewBag.SaldoDisponible = presupuesto.Monto - ViewBag.TotalGastado;

            return View(presupuesto);
        }

        // GET: Presupuesto/Create
        public ActionResult Create()
        {
            //lista para los meses
         var meses = Enumerable.Range(1, 12)
        .Select(m => new SelectListItem
        {
            Value = m.ToString(),
            Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
        });

            //Lista para los años en rango de 5 años antes y 5 años despues
         var años = Enumerable.Range(DateTime.Now.Year - 5, 11)
        .Select(y => new SelectListItem
        {
            Value = y.ToString(),
            Text = y.ToString()
        });
            ViewBag.Meses = new SelectList(meses, "Value","Text",DateTime.Now.Month);
            ViewBag.Años = new SelectList(años, "Value", "Text", DateTime.Now.Year);
            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto, "IdCategoriaPresupuesto", "Nombre");
           
            return View();
        }

        // POST: Presupuesto/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPresupuesto,IdUsuario,IdCategoriaPresupuesto,Monto,Mes,Año")] Presupuesto presupuesto)
        {
           
                presupuesto.IdUsuario = (int)Session["Id"];

                bool existPresupuesto = db.Presupuesto.Any(
                    p=> 
                    p.IdCategoriaPresupuesto==presupuesto.IdCategoriaPresupuesto &&
                    p.Mes==presupuesto.Mes &&
                    p.Año== presupuesto.Año &&
                    p.IdUsuario==presupuesto.IdUsuario);

                if (existPresupuesto)
                {
                    ModelState.AddModelError("", "Ya existe un presupuesto para esta categoría en el mes y año seleccionados.");
                }

            if (ModelState.IsValid)
            {
                db.Presupuesto.Add(presupuesto);
                db.SaveChanges();
                return RedirectToAction("Presupuesto");
            }

            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto, "IdCategoriaPresupuesto", "Nombre", presupuesto.IdCategoriaPresupuesto);
            //lista para los meses
            var meses = Enumerable.Range(1, 12)
           .Select(m => new SelectListItem
           {
               Value = m.ToString(),
               Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
           });

            //Lista para los años en rango de 5 años antes y 5 años despues
            var años = Enumerable.Range(DateTime.Now.Year - 5, 11)
           .Select(y => new SelectListItem
           {
               Value = y.ToString(),
               Text = y.ToString()
           });

            ViewBag.Meses = new SelectList(meses, "Value", "Text", DateTime.Now.Month);
            ViewBag.Años = new SelectList(años, "Value", "Text", DateTime.Now.Year);
            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto, "IdCategoriaPresupuesto", "Nombre", presupuesto.IdCategoriaPresupuesto);
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
