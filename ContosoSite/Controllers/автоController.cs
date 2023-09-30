using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoSite.Models;

namespace ContosoSite.Controllers
{
    public class автоController : Controller
    {
        private DanilichDBEntities11 db = new DanilichDBEntities11();

        // GET: авто
        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var sort = from s in db.авто select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                sort = sort.Where(s => s.марка_автомобиля.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    sort = sort.OrderByDescending(s => s.марка_автомобиля);
                    break;
                case "Date":
                    sort = sort.OrderBy(s => s.год_выпуска);
                    break;
                case "date_desc":
                    sort = sort.OrderByDescending(s => s.год_выпуска);
                    break;
                default:
                    sort = sort.OrderBy(s => s.марка_автомобиля);
                    break;
            }
            var авто = db.авто.Include(а => а.заказы);
            return View(sort.ToList());
        }

        // GET: авто/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            авто авто = db.авто.Find(id);
            if (авто == null)
            {
                return HttpNotFound();
            }
            return View(авто);
        }

        // GET: авто/Create
        public ActionResult Create()
        {
            ViewBag.код_автомобиля = new SelectList(db.заказы, "код_заказа", "код_автомобиля");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "код,код_автомобиля,vin,марка_автомобиля,модель_автомобиля,год_выпуска")] авто авто)
        {
            if (ModelState.IsValid)
            {
                db.авто.Add(авто);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.код_автомобиля = new SelectList(db.заказы, "код_заказа", "код_автомобиля", авто.код_автомобиля);
            return View(авто);
        }

        // GET: авто/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            авто авто = db.авто.Find(id);
            if (авто == null)
            {
                return HttpNotFound();
            }
            ViewBag.код_автомобиля = new SelectList(db.заказы, "код_заказа", "код_автомобиля", авто.код_автомобиля);
            return View(авто);
        }

        // POST: авто/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "код,код_автомобиля,vin,марка_автомобиля,модель_автомобиля,год_выпуска")] авто авто)
        {
            if (ModelState.IsValid)
            {
                db.Entry(авто).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.код_автомобиля = new SelectList(db.заказы, "код_заказа", "код_автомобиля", авто.код_автомобиля);
            return View(авто);
        }

        // GET: авто/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            авто авто = db.авто.Find(id);
            if (авто == null)
            {
                return HttpNotFound();
            }
            return View(авто);
        }

        // POST: авто/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            авто авто = db.авто.Find(id);
            db.авто.Remove(авто);
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
