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
    public class авторизацияController : Controller
    {
        private DanilichDBEntities11 db = new DanilichDBEntities11();

        // GET: авторизация
        public ActionResult Index()
        {
            return View(db.авторизация.ToList());
        }

        // GET: авторизация/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            авторизация авторизация = db.авторизация.Find(id);
            if (авторизация == null)
            {
                return HttpNotFound();
            }
            return View(авторизация);
        }

        // GET: авторизация/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: авторизация/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "логин,пароль")] авторизация авторизация)
        {
            if (ModelState.IsValid)
            {
                db.авторизация.Add(авторизация);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(авторизация);
        }

        // GET: авторизация/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            авторизация авторизация = db.авторизация.Find(id);
            if (авторизация == null)
            {
                return HttpNotFound();
            }
            return View(авторизация);
        }

        // POST: авторизация/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "логин,пароль")] авторизация авторизация)
        {
            if (ModelState.IsValid)
            {
                db.Entry(авторизация).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(авторизация);
        }

        // GET: авторизация/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            авторизация авторизация = db.авторизация.Find(id);
            if (авторизация == null)
            {
                return HttpNotFound();
            }
            return View(авторизация);
        }

        // POST: авторизация/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            авторизация авторизация = db.авторизация.Find(id);
            db.авторизация.Remove(авторизация);
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
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register([Bind(Include = "Логин,Пароль")] авторизация авторизация)
        {
            if (ModelState.IsValid)
            {
                db.авторизация.Add(авторизация);
                db.SaveChanges();
            }
            ViewBag.Message = авторизация.логин + " вы успешно зарегистрированы";
            return View();

        }
        //Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind(Include = "Логин,Пароль")] авторизация авторизация)
        {
            try
            {
                var usr = db.авторизация.Single(u => u.логин == авторизация.логин && u.пароль == авторизация.пароль);
                Session["login"] = usr.логин.ToString();
                return View("~/Views/Home/Index.cshtml");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "UserName or Password is wrong");
            }

            return View();
        }
    }
}
