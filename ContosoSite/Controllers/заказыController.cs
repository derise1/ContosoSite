using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoSite.Models;
using GemBox.Document;
using OfficeOpenXml;

namespace ContosoSite.Controllers
{
    public class заказыController : Controller
    {
        private DanilichDBEntities11 db = new DanilichDBEntities11();

        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var sort = from s in db.заказы select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                sort = sort.Where(s => s.вид_работ.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    sort = sort.OrderByDescending(s => s.код_клиента);
                    break;
                case "Date":
                    sort = sort.OrderBy(s => s.дата_приема);
                    break;
                case "date_desc":
                    sort = sort.OrderByDescending(s => s.дата_приема);
                    break;
                default:
                    sort = sort.OrderBy(s => s.код_клиента);
                    break;
            }
            var заказы = db.заказы.Include(з => з.запчасти).Include(з => з.сотрудники).Include(з => з.авто);
            return View(sort.ToList());
        }

        public ActionResult DocxDownload(string id, string id_sotr)
        {
            DateTime dt = DateTime.Now;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
            var user = db.заказы.ToList();
            var user_sotr = db.сотрудники.ToList();
            var path = Path.GetFullPath("C:\\Users\\bezru\\Documents\\doc\\empty.docx");
            var document = DocumentModel.Load(path);

            заказы usrdata = user.First(p => p.код_заказа == Convert.ToInt32(id));
            сотрудники usrdata_sotr = user_sotr.First(p => p.код_сотрудника == Convert.ToInt32(id_sotr));
            document.Content.Replace("[код автомобиля]", usrdata.код_автомобиля.ToString());
            document.Content.Replace("[код клиента]", usrdata.код_клиента.ToString());
            document.Content.Replace("[дата приема]", usrdata.дата_приема.ToString("MM/dd/yyyy"));
            document.Content.Replace("[дата выдачи]", usrdata.дата_выдачи.ToString("MM/dd/yyyy"));
            document.Content.Replace("[вид работ]", usrdata.вид_работ.ToString());
            document.Content.Replace("[сотрудник]", usrdata_sotr.ФИО.ToString());
            document.Content.Replace("[итоговая сумма]", usrdata.сумма_работ.ToString());
            document.Content.Replace("[день]", dt.Day.ToString());
            document.Content.Replace("[месяц]", dt.ToString("MMMM"));
            document.Content.Replace("[год]", dt.Year.ToString());
            var stream = new MemoryStream();
            document.Save(stream, new DocxSaveOptions() { Format = DocxFormat.Docx });
            return File(stream.ToArray(), "DOCX", $"{usrdata.код_автомобиля}.docx");
        }

        // GET: заказы/Details/5
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

        // GET: заказы/Create
        public ActionResult Create()
        {
            ViewBag.товары_для_ремонта = new SelectList(db.запчасти, "код_товара", "наименование_товара");
            ViewBag.сотрудник = new SelectList(db.сотрудники, "код_сотрудника", "ФИО");
            return View();
        }

        // POST: заказы/Create
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

        // GET: заказы/Edit/5
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

        // POST: заказы/Edit/5
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

        // GET: заказы/Delete/5
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

        // POST: заказы/Delete/5
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
