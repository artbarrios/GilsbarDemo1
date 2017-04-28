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
    public class DepartmentsController : Controller
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // GET: Departments
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/DepartmentsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            var departments = db.Departments.Include(d => d.Building);
            return View(departments.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
                ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name", modelType.Contains("Building") ? id : -1);
            }
            else
            {
                ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name");
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,BuildingId")] Department department, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                LogManager.Log("Departments/Create - DepartmentId:" + department.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("Building"))
                    {
                        db.Buildings.Find(id).Departments.Add(department);
                        db.SaveChanges();
                        LogManager.Log("Departments/Create added - DepartmentId:" + department.Id.ToString() + " to BuildingId: " + id.ToString());
                        controllerName = "Buildings";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name", department.BuildingId);

            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name", department.BuildingId);

            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,BuildingId")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Departments/Edit/ - DepartmentId:" + department.Id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "Id", "Name", department.BuildingId);

            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            LogManager.Log("Departments/Delete/ - DepartmentId:" + department.Id.ToString());
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
