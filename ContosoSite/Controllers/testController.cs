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
    public class testController : Controller
    {
        private DanilichDBEntities11 db = new DanilichDBEntities11();

        // GET: test
        public ActionResult Index()
        {
            var заказы = db.заказы.Include(з => з.запчасти).Include(з => з.сотрудники);
            return View(заказы.ToList());
        }

        // GET: test/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            заказы заказы = db.заказы.Find(id);
            if (заказы == null)
            {
                return HttpNotFound();
            }
            return View(заказы);
        }

        // GET: test/Create
        public ActionResult Create()
        {
            ViewBag.товары_для_ремонта = new SelectList(db.запчасти, "код_товара", "наименование_товара");
            ViewBag.сотрудник = new SelectList(db.сотрудники, "код_сотрудника", "ФИО");
            return View();
        }

        // POST: test/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "код_заказа,код_автомобиля,код_клиента,дата_приема,дата_выдачи,вид_работ,сумма_работ,товары_для_ремонта,сотрудник")] заказы заказы)
        {
            if (ModelState.IsValid)
            {
                db.заказы.Add(заказы);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.товары_для_ремонта = new SelectList(db.запчасти, "код_товара", "наименование_товара", заказы.товары_для_ремонта);
            ViewBag.сотрудник = new SelectList(db.сотрудники, "код_сотрудника", "ФИО", заказы.сотрудник);
            return View(заказы);
        }

        // GET: test/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            заказы заказы = db.заказы.Find(id);
            if (заказы == null)
            {
                return HttpNotFound();
            }
            ViewBag.товары_для_ремонта = new SelectList(db.запчасти, "код_товара", "наименование_товара", заказы.товары_для_ремонта);
            ViewBag.сотрудник = new SelectList(db.сотрудники, "код_сотрудника", "ФИО", заказы.сотрудник);
            return View(заказы);
        }

        // POST: test/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "код_заказа,код_автомобиля,код_клиента,дата_приема,дата_выдачи,вид_работ,сумма_работ,товары_для_ремонта,сотрудник")] заказы заказы)
        {
            if (ModelState.IsValid)
            {
                db.Entry(заказы).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.товары_для_ремонта = new SelectList(db.запчасти, "код_товара", "наименование_товара", заказы.товары_для_ремонта);
            ViewBag.сотрудник = new SelectList(db.сотрудники, "код_сотрудника", "ФИО", заказы.сотрудник);
            return View(заказы);
        }

        // GET: test/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            заказы заказы = db.заказы.Find(id);
            if (заказы == null)
            {
                return HttpNotFound();
            }
            return View(заказы);
        }

        // POST: test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            заказы заказы = db.заказы.Find(id);
            db.заказы.Remove(заказы);
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
