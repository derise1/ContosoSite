using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoSite.Models;

namespace ContosoSite.Controllers
{
    public class HomeController : Controller
    {
        private DanilichDBEntities11 db = new DanilichDBEntities11();

        public ActionResult Index()
        {
            ViewBag.Message = "PRIVET ROSSIA";
            return View("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("About");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View("Contact");
        }

        public ActionResult Authorize(Models.авторизация model)
        {
            using (DanilichDBEntities11 db = new DanilichDBEntities11())
            {
                var userDetais = db.авторизация.FirstOrDefault(x => x.логин == model.логин && x.пароль == model.пароль);

                if (userDetais != null)
                {
                    var userDetails = db.авторизация.Single(x => x.логин == model.логин && x.пароль == model.пароль);
                    Session["Admin"] = userDetails.логин;
                    return View("~/Views/Home/Index.cshtml"); //RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["CustomError"] = "Неверные данные";
                    ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                    return View("~/Views/Home/Login.cshtml");
                }
            }
        }
        public ActionResult LogOut()        //Выход из системы
        {
            Session["login"] = null;
            Session.Abandon();
            return View("~/Views/Home/Index.cshtml");
        }
    }
}