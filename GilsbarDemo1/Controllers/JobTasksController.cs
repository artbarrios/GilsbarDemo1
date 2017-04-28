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
    public class JobTasksController : Controller
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // GET: AddPersonToJobTask
        public ActionResult AddPersonToJobTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = db.JobTasks.Find(id);
            var personsAvailable = db.Persons.ToList().Except(jobTask.Persons.ToList()).ToList();
            ViewBag.PersonId = new SelectList(personsAvailable, "Id", "Firstname");
            if (jobTask == null)
            {
                return HttpNotFound();
            }
            JobTaskPersonViewModel viewModel = new JobTaskPersonViewModel();
            viewModel.JobTaskId = jobTask.Id;
            viewModel.JobTask_Name = jobTask.Name;
            return View(viewModel);
        }

        // POST: JobTasks/AddPersonToJobTask/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPersonToJobTask([Bind(Include = "JobTaskId,JobTask_Name,PersonId,Person_Firstname")] JobTaskPersonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                JobTask jobTask = db.JobTasks.Find(viewModel.JobTaskId);
                Person person = db.Persons.Find(viewModel.PersonId);
                jobTask.Persons.Add(person);
                db.Entry(jobTask).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("JobTasks/AddPersonToJobTask/ - PersonId:" + person.Id.ToString() + " to JobTaskId: " + jobTask.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.JobTaskId });
            }
            return View(viewModel);
        }

        // GET: RemovePersonFromJobTask
        public ActionResult RemovePersonFromJobTask(int? jobTaskId, int? personId)
        {
            if (jobTaskId == null || personId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = db.JobTasks.Find(jobTaskId);
            Person person = db.Persons.Find(personId);
            if (jobTask == null || person == null)
            {
                return HttpNotFound();
            }
            jobTask.Persons.Remove(person);
            db.Entry(jobTask).State = EntityState.Modified;
            db.SaveChanges();
            LogManager.Log("JobTasks/RemovePersonFromJobTask/ - PersonId:" + person.Id.ToString() + " from JobTaskId: " + jobTask.Id.ToString());
            return RedirectToAction("Details", new { id = jobTaskId });
        }

        // GET: JobTasks
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/JobTasksIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.JobTasks.ToList());
        }

        // GET: JobTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = db.JobTasks.Find(id);
            if (jobTask == null)
            {
                return HttpNotFound();
            }
            return View(jobTask);
        }

        // GET: JobTasks/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
            }
            else
            {
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: JobTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ImageFilename")] JobTask jobTask, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.JobTasks.Add(jobTask);
                db.SaveChanges();
                LogManager.Log("JobTasks/Create - JobTaskId:" + jobTask.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("Person"))
                    {
                        db.Persons.Find(id).JobTasks.Add(jobTask);
                        db.SaveChanges();
                        LogManager.Log("JobTasks/Create added - JobTaskId:" + jobTask.Id.ToString() + " to PersonId: " + id.ToString());
                        controllerName = "Persons";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }

            return View(jobTask);
        }

        // GET: JobTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = db.JobTasks.Find(id);
            if (jobTask == null)
            {
                return HttpNotFound();
            }
            return View(jobTask);
        }

        // POST: JobTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ImageFilename")] JobTask jobTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobTask).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("JobTasks/Edit/ - JobTaskId:" + jobTask.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(jobTask);
        }

        // GET: JobTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = db.JobTasks.Find(id);
            if (jobTask == null)
            {
                return HttpNotFound();
            }
            return View(jobTask);
        }

        // POST: JobTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobTask jobTask = db.JobTasks.Find(id);
            db.JobTasks.Remove(jobTask);
            db.SaveChanges();
            LogManager.Log("JobTasks/Delete/ - JobTaskId:" + jobTask.Id.ToString());
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
