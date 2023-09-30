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
    public class поставщикиController : Controller
    {
        private DanilichDBEntities11 db = new DanilichDBEntities11();

        // GET: поставщики
        public ActionResult Index()
        {
            return View(db.поставщики.ToList());
        }

        // GET: поставщики/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            поставщики поставщики = db.поставщики.Find(id);
            if (поставщики == null)
            {
                return HttpNotFound();
            }
            return View(поставщики);
        }

        // GET: поставщики/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: поставщики/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "код_поставщика,ФИО,дата_рождения,номер_телефона,компания")] поставщики поставщики)
        {
            if (ModelState.IsValid)
            {
                db.поставщики.Add(поставщики);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(поставщики);
        }

        // GET: поставщики/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            поставщики поставщики = db.поставщики.Find(id);
            if (поставщики == null)
            {
                return HttpNotFound();
            }
            return View(поставщики);
        }

        // POST: поставщики/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "код_поставщика,ФИО,дата_рождения,номер_телефона,компания")] поставщики поставщики)
        {
            if (ModelState.IsValid)
            {
                db.Entry(поставщики).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(поставщики);
        }

        // GET: поставщики/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            поставщики поставщики = db.поставщики.Find(id);
            if (поставщики == null)
            {
                return HttpNotFound();
            }
            return View(поставщики);
        }

        // POST: поставщики/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            поставщики поставщики = db.поставщики.Find(id);
            db.поставщики.Remove(поставщики);
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
