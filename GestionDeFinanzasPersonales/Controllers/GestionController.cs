using GestionDeFinanzasPersonales.Models;
using GestionDeFinanzasPersonales.Models.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GestionDeFinanzasPersonales.Controllers
{
    [Authorize]
    public class GestionController : Controller
    {
        private GestionFinanzasPersonalesEntities2 db = new GestionFinanzasPersonalesEntities2();
        DateTime fechaActual = DateTime.UtcNow;


        //GET 
        public ActionResult Dashboard()
        {

            // Verificar que el usuario esté autenticado
            if (!User.Identity.IsAuthenticated || Session["Id"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var userId = (int)Session["Id"];

            //Total de ingresos
            var totalIngresos = db.Gestion.Where
                (g => g.Usuario.Id == userId && g.Tipo.Categoria.NombreCategoria == "Ingreso")
                .Sum(g => (decimal?)g.Monto) ?? 0;

            //Total de gastos
            var totalGastos = db.Gestion.Where
                (g => g.Usuario.Id == userId && g.Tipo.Categoria.NombreCategoria == "Gasto")
                .Sum(g => (decimal?)g.Monto) ?? 0;

            //Balance
            var balance = totalIngresos - totalGastos;

            //Todos los movimientos del usuario
            var movimientos = db.Gestion
                               .Where(g => g.IdUsuario == userId)
                               .ToList();

            var model = new DashboardViewModel
            {
                TotalIngresos = totalIngresos,
                TotalGastos = totalGastos,
                Balance = balance,
                Movimientos = movimientos

            };

            return View(model);
        }





        // GET: Gestion

        public ActionResult Movimientos()
        {

            // Verificar que el usuario esté autenticado
            if (!User.Identity.IsAuthenticated || Session["Id"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var userId = (int)Session["Id"];
            var gestionesUsuario = db.Gestion
                               .Where(g => g.IdUsuario == userId)
                               .ToList();
            
            return View(gestionesUsuario);
        }

        //Filtrar movimientos por categoria
        public ActionResult MovimientosFiltro(string categoria) {
            if (!User.Identity.IsAuthenticated || Session["Id"]==null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var userId = (int)Session["Id"];
            var query = db.Gestion.Where(g=>g.IdUsuario == userId);

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(g => g.Tipo.Categoria.NombreCategoria == categoria);
            }

            return PartialView("MovimientosFiltro", query.ToList());

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
            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto.ToList(),"IdCategoriaPresupuesto", "Nombre");


            return View();
        }

        //Metodo para obtener los tipos según las categorias
        public JsonResult GetTiposByCategoria(int idCategoria)
        {
            var tipos = db.Tipo
                    .Where(t => t.IdCategoria == idCategoria)
                    .Select(t => new { t.IdTipo, t.NombreTipo })
                    .ToList();
            return Json(tipos, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdGestion,Monto,IdUsuario,IdTipo, IdCategoriaPresupuesto")] Gestion gestion)
        {
            if (ModelState.IsValid)
            {
                // Asignar automáticamente el usuario logueado
                gestion.IdUsuario = (int)Session["Id"];
                if (gestion.FechaOperacion == null)
                {
                    gestion.FechaOperacion=fechaActual;
                }

                db.Gestion.Add(gestion);
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            ViewBag.IdCategoria = new SelectList(db.Categoria, "IdCategoria", "NombreCategoria");
            ViewBag.IdTipo = new SelectList(db.Tipo, "IdTipo", "NombreTipo", gestion.IdTipo);
            ViewBag.IdCategoriaPresupuesto = new SelectList(db.CategoriaPresupuesto, "IdCategoriaPresupuesto", "Nombre", gestion.IdCategoriaPresupuesto);

            return View(gestion);

            
        }

        // GET: Gestion/Edit/5

        public ActionResult Edit(int? id)
        {
            var userId = (int)Session["Id"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gestion gestion = db.Gestion.Find(id);
            if (gestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategoria = new SelectList(db.Categoria, "IdCategoria", "NombreCategoria", gestion.Tipo?.IdCategoria);

            if (gestion.Tipo != null)
            {
                var tiposCategoria = db.Tipo.Where(t => t.IdCategoria == gestion.Tipo.IdCategoria);
                ViewBag.IdTipo = new SelectList(tiposCategoria, "IdTipo", "NombreTipo", gestion.IdTipo);
            }
            else
            {
                ViewBag.IdTipo = new SelectList(new List<Tipo>(), "IdTipo", "NombreTipo");
            }
            return View(gestion);
        }

        // POST: Gestion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdGestion,Monto,IdUsuario,IdTipo")] Gestion gestion)
        { // Asignar automáticamente el usuario logueado
            gestion.IdUsuario = (int)Session["Id"];

            if (ModelState.IsValid)
            {
                db.Entry(gestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Movimientos");
            }
            ViewBag.IdCategoria = new SelectList(db.Categoria, "IdCategoria", "NombreCategoria");
            ViewBag.IdTipo = new SelectList(db.Tipo, "IdTipo", "NombreTipo", gestion.IdTipo);
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
            return RedirectToAction("Movimientos");
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
