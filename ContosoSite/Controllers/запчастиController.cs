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
    public class запчастиController : Controller
    {
        private DanilichDBEntities11 db = new DanilichDBEntities11();

        // GET: запчасти
        public ActionResult Index()
        {
            var запчасти = db.запчасти.Include(з => з.поставщики);
            return View(запчасти.ToList());
        }

        // GET: запчасти/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            запчасти запчасти = db.запчасти.Find(id);
            if (запчасти == null)
            {
                return HttpNotFound();
            }
            return View(запчасти);
        }

        // GET: запчасти/Create
        public ActionResult Create()
        {
            ViewBag.поставщик = new SelectList(db.поставщики, "код_поставщика", "ФИО");
            return View();
        }

        // POST: запчасти/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "код_товара,наименование_товара,количество_на_складе,поставщик")] запчасти запчасти)
        {
            if (ModelState.IsValid)
            {
                db.запчасти.Add(запчасти);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.поставщик = new SelectList(db.поставщики, "код_поставщика", "ФИО", запчасти.поставщик);
            return View(запчасти);
        }

        // GET: запчасти/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            запчасти запчасти = db.запчасти.Find(id);
            if (запчасти == null)
            {
                return HttpNotFound();
            }
            ViewBag.поставщик = new SelectList(db.поставщики, "код_поставщика", "ФИО", запчасти.поставщик);
            return View(запчасти);
        }

        // POST: запчасти/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "код_товара,наименование_товара,количество_на_складе,поставщик")] запчасти запчасти)
        {
            if (ModelState.IsValid)
            {
                db.Entry(запчасти).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.поставщик = new SelectList(db.поставщики, "код_поставщика", "ФИО", запчасти.поставщик);
            return View(запчасти);
        }

        // GET: запчасти/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            запчасти запчасти = db.запчасти.Find(id);
            if (запчасти == null)
            {
                return HttpNotFound();
            }
            return View(запчасти);
        }

        // POST: запчасти/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            запчасти запчасти = db.запчасти.Find(id);
            db.запчасти.Remove(запчасти);
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
