using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Configuration;
using GilsbarDemo1.Models;

namespace GilsbarDemo1.Controllers
{
    public class ManagersController : Controller
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // GET: AddPersonToManager
        public ActionResult AddPersonToManager(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            var personsAvailable = db.Persons.ToList().Except(manager.Persons.ToList()).ToList();
            ViewBag.PersonId = new SelectList(personsAvailable, "Id", "Firstname");
            if (manager == null)
            {
                return HttpNotFound();
            }
            ManagerPersonViewModel viewModel = new ManagerPersonViewModel();
            viewModel.ManagerId = manager.Id;
            viewModel.Manager_Firstname = manager.Firstname;
            return View(viewModel);
        }

        // POST: Managers/AddPersonToManager/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPersonToManager([Bind(Include = "ManagerId,Manager_Firstname,PersonId,Person_Firstname")] ManagerPersonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Manager manager = db.Managers.Find(viewModel.ManagerId);
                Person person = db.Persons.Find(viewModel.PersonId);
                manager.Persons.Add(person);
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Managers/AddPersonToManager/ - PersonId:" + person.Id.ToString() + " to ManagerId: " + manager.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.ManagerId });
            }
            return View(viewModel);
        }

        // GET: RemovePersonFromManager
        // NOTE: the Person.ManagerId property is not nullable so this routine must be omitted

        // GET: Managers
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/ManagersIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.Managers.ToList());
        }

        // GET: Managers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // GET: Managers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Firstname,Lastname,Email,HomePhone,CellPhone,WorkPhone,DateOfBirth")] Manager manager, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                LogManager.Log("Managers/Create - ManagerId:" + manager.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(manager);
        }

        // GET: Managers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstname,Lastname,Email,HomePhone,CellPhone,WorkPhone,DateOfBirth")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Managers/Edit/ - ManagerId:" + manager.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(manager);
        }

        // GET: Managers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager manager = db.Managers.Find(id);
            db.Managers.Remove(manager);
            db.SaveChanges();
            LogManager.Log("Managers/Delete/ - ManagerId:" + manager.Id.ToString());
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
