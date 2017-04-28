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
    public class BuildingsController : Controller
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // GET: AddDepartmentToBuilding
        public ActionResult AddDepartmentToBuilding(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            var departmentsAvailable = db.Departments.ToList().Except(building.Departments.ToList()).ToList();
            ViewBag.DepartmentId = new SelectList(departmentsAvailable, "Id", "Name");
            if (building == null)
            {
                return HttpNotFound();
            }
            BuildingDepartmentViewModel viewModel = new BuildingDepartmentViewModel();
            viewModel.BuildingId = building.Id;
            viewModel.Building_Name = building.Name;
            return View(viewModel);
        }

        // POST: Buildings/AddDepartmentToBuilding/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDepartmentToBuilding([Bind(Include = "BuildingId,Building_Name,DepartmentId,Department_Name")] BuildingDepartmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Building building = db.Buildings.Find(viewModel.BuildingId);
                Department department = db.Departments.Find(viewModel.DepartmentId);
                building.Departments.Add(department);
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Buildings/AddDepartmentToBuilding/ - DepartmentId:" + department.Id.ToString() + " to BuildingId: " + building.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.BuildingId });
            }
            return View(viewModel);
        }

        // GET: RemoveDepartmentFromBuilding
        // NOTE: the Department.BuildingId property is not nullable so this routine must be omitted

        // GET: Buildings
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/BuildingsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.Buildings.ToList());
        }

        // GET: Buildings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // GET: Buildings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address")] Building building, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Buildings.Add(building);
                db.SaveChanges();
                LogManager.Log("Buildings/Create - BuildingId:" + building.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(building);
        }

        // GET: Buildings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Buildings/Edit/ - BuildingId:" + building.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(building);
        }

        // GET: Buildings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Building building = db.Buildings.Find(id);
            db.Buildings.Remove(building);
            db.SaveChanges();
            LogManager.Log("Buildings/Delete/ - BuildingId:" + building.Id.ToString());
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
