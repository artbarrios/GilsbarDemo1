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
    public class PersonsController : Controller
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // Persons/PersonJobTasksFlowchartDiagram/1
        public ActionResult PersonJobTasksFlowchartDiagram(int? id)
        {
            Person person = db.Persons.Find(id);

            if (person.JobTaskFlowchartDiagramData == null || person.JobTaskFlowchartDiagramData.Length == 0)
            {
                Flowchart flowchart = PersonJobTasksToFlowchart(person);
                person.JobTaskFlowchartDiagramData = flowchart.ToJSON();
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.FlowchartTitle = "JobTask Diagram for " + person.Firstname;
            ViewBag.FlowchartData = person.JobTaskFlowchartDiagramData;
            return View(person);
        }

        // POST: Persons/PersonJobTasksFlowchartDiagram/1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult PersonJobTasksFlowchartDiagram([Bind(Include = "Id")] Person person, string flowchartData)
        {
            person = db.Persons.Find(person.Id);
            if (flowchartData.Length > 0)
            {
                person.JobTaskFlowchartDiagramData = flowchartData;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = person.Id });
        }

        // PersonJobTasksToFlowchart
        public static Flowchart PersonJobTasksToFlowchart(Person person)
        {
            // converts the specified objects into a Flowchart object and returns it
            Flowchart flowchart = new Flowchart();
            FlowchartOperator fcOperator = null;
            FlowchartOperator fcOperatorPrevious = null;
            FlowchartConnector fcInput = null;
            FlowchartConnector fcOutput = null;
            FlowchartLink fcLink = null;
            int top = 0;
            int left = 0;
            int opCount = 0;

            // check for valid input
            if (person != null)
            {
                flowchart.Id = person.Id.ToString();
                // add operators
                opCount = 1;
                foreach (JobTask jobTask in person.JobTasks)
                {
                    fcOperator = new FlowchartOperator();
                    fcOperator.Id = "op" + person.Id.ToString() + jobTask.Id.ToString();
                    fcOperator.Title = jobTask.Name;
                    fcOperator.Top = top;
                    fcOperator.Left = left;
                    fcOperator.ImageSource = jobTask.ImageFilename;
                    top += 20;
                    left += 200;
                    // inputs
                    fcInput = new FlowchartConnector();
                    fcInput.Id = fcOperator.Id + "in1";
                    fcInput.Label = "";
                    fcOperator.Inputs.Add(fcInput);
                    // outputs
                    fcOutput = new FlowchartConnector();
                    fcOutput.Id = fcOperator.Id + "out1";
                    fcOutput.Label = "";
                    fcOperator.Outputs.Add(fcOutput);
                    // popup
                    fcOperator.Popup.header = "<h2>" + jobTask.Name + "</h2>";
                    fcOperator.Popup.body = @"";
                    fcOperator.Popup.body += @"<p>Name: " + jobTask.Name.ToString() + "</p>";
                    fcOperator.Popup.body += @"<p>Description: " + jobTask.Description.ToString() + "</p>";
                    fcOperator.Popup.body += @"<img src='" + jobTask.ImageFilename + "' alt='Image'>";
                    // add the operator
                    flowchart.Operators.Add(fcOperator);
                    opCount += 1;
                }

                // add links
                foreach (FlowchartOperator myOperator in flowchart.Operators)
                {
                    if (fcOperatorPrevious != null)
                    {
                        fcLink = new FlowchartLink();
                        fcLink.Id = myOperator.Id + "lnk1";
                        fcLink.FromOperatorId = fcOperatorPrevious.Id;
                        fcLink.FromConnectorId = fcOperatorPrevious.Outputs.FirstOrDefault().Id;
                        fcLink.ToOperatorId = myOperator.Id;
                        fcLink.ToConnectorId = myOperator.Inputs.FirstOrDefault().Id;
                        flowchart.Links.Add(fcLink);
                    }
                    fcOperatorPrevious = myOperator;
                }
            }
            return flowchart;
        } // PersonJobTasksToFlowchart ()

        // GET: AddJobTaskToPerson
        public ActionResult AddJobTaskToPerson(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            var jobTasksAvailable = db.JobTasks.ToList().Except(person.JobTasks.ToList()).ToList();
            ViewBag.JobTaskId = new SelectList(jobTasksAvailable, "Id", "Name");
            if (person == null)
            {
                return HttpNotFound();
            }
            JobTaskPersonViewModel viewModel = new JobTaskPersonViewModel();
            viewModel.PersonId = person.Id;
            viewModel.Person_Firstname = person.Firstname;
            return View(viewModel);
        }

        // POST: Persons/AddJobTaskToPerson/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddJobTaskToPerson([Bind(Include = "PersonId,Person_Firstname,JobTaskId,JobTask_Name")] JobTaskPersonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Person person = db.Persons.Find(viewModel.PersonId);
                JobTask jobTask = db.JobTasks.Find(viewModel.JobTaskId);
                person.JobTasks.Add(jobTask);
                Flowchart flowchart = PersonJobTasksToFlowchart(person);
                person.JobTaskFlowchartDiagramData = flowchart.ToJSON();
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Persons/AddJobTaskToPerson/ - JobTaskId:" + jobTask.Id.ToString() + " to PersonId: " + person.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.PersonId });
            }
            return View(viewModel);
        }

        // GET: RemoveJobTaskFromPerson
        public ActionResult RemoveJobTaskFromPerson(int? personId, int? jobTaskId)
        {
            if (personId == null || jobTaskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(personId);
            JobTask jobTask = db.JobTasks.Find(jobTaskId);
            if (person == null || jobTask == null)
            {
                return HttpNotFound();
            }
            person.JobTasks.Remove(jobTask);
                Flowchart flowchart = PersonJobTasksToFlowchart(person);
                person.JobTaskFlowchartDiagramData = flowchart.ToJSON();
            db.Entry(person).State = EntityState.Modified;
            db.SaveChanges();
            LogManager.Log("Persons/RemoveJobTaskFromPerson/ - JobTaskId:" + jobTask.Id.ToString() + " from PersonId: " + person.Id.ToString());
            return RedirectToAction("Details", new { id = personId });
        }

        // GET: Persons
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/PersonsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            var persons = db.Persons.Include(p => p.Manager);
            return View(persons.ToList());
        }

        // GET: Persons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Persons/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
                ViewBag.ManagerId = new SelectList(db.Managers, "Id", "Firstname", modelType.Contains("Manager") ? id : -1);
            }
            else
            {
                ViewBag.ManagerId = new SelectList(db.Managers, "Id", "Firstname");
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Firstname,Lastname,Email,HomePhone,CellPhone,WorkPhone,DateOfBirth,ManagerId,JobFlowchartDiagramData,JobTaskFlowchartDiagramData")] Person person, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Persons.Add(person);
                db.SaveChanges();
                LogManager.Log("Persons/Create - PersonId:" + person.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("Manager"))
                    {
                        db.Managers.Find(id).Persons.Add(person);
                        db.SaveChanges();
                        LogManager.Log("Persons/Create added - PersonId:" + person.Id.ToString() + " to ManagerId: " + id.ToString());
                        controllerName = "Managers";
                    }
                    if (modelType.Contains("JobTask"))
                    {
                        db.JobTasks.Find(id).Persons.Add(person);
                        db.SaveChanges();
                        LogManager.Log("Persons/Create added - PersonId:" + person.Id.ToString() + " to JobTaskId: " + id.ToString());
                        controllerName = "JobTasks";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "Id", "Firstname", person.ManagerId);

            return View(person);
        }

        // GET: Persons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "Id", "Firstname", person.ManagerId);

            return View(person);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstname,Lastname,Email,HomePhone,CellPhone,WorkPhone,DateOfBirth,ManagerId,JobFlowchartDiagramData,JobTaskFlowchartDiagramData")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Persons/Edit/ - PersonId:" + person.Id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "Id", "Firstname", person.ManagerId);

            return View(person);
        }

        // GET: Persons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
            db.SaveChanges();
            LogManager.Log("Persons/Delete/ - PersonId:" + person.Id.ToString());
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
