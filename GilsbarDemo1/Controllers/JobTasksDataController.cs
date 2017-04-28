using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GilsbarDemo1.Models;

namespace GilsbarDemo1.Controllers
{
    public class JobTasksDataController : ApiController
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // GET: api/GetJobTaskPersons/?JobTaskId=1
        [Route("api/GetJobTaskPersons/")]
        public List<Person> GetJobTaskPersons(int JobTaskId)
        {
            JobTask jobTask = db.JobTasks.Find(JobTaskId);
            if (jobTask == null)
            {
                return null;
            }
            return jobTask.Persons;
        }

        // PUT: api/AddPersonToJobTask/?JobTaskId=1&PersonId=1
        [HttpPut]
        [Route("api/AddPersonToJobTask/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddPersonToJobTask(int JobTaskId, int PersonId)
        {
            JobTask jobTask = db.JobTasks.Find(JobTaskId);
            Person person = db.Persons.Find(PersonId);
            if (jobTask != null && person != null)
            {
                try
                {
                    jobTask.Persons.Add(person);
                    db.Entry(jobTask).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddPersonToJobTask - JobTaskId:" + jobTask.Id.ToString() + " PersonId:" + person.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // AddPersonToJobTask

        // PUT: api/RemovePersonFromJobTask/?JobTaskId=1&PersonId=1
        [HttpPut]
        [Route("api/RemovePersonFromJobTask/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemovePersonFromJobTask(int JobTaskId, int PersonId)
        {
            JobTask jobTask = db.JobTasks.Find(JobTaskId);
            Person person = db.Persons.Find(PersonId);
            if (jobTask != null && person != null)
            {
                try
                {
                    jobTask.Persons.Remove(person);
                    db.Entry(jobTask).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/RemovePersonToJobTask - JobTaskId:" + jobTask.Id.ToString() + " PersonId:" + person.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // RemovePersonFromJobTask

        // GET: api/JobTasksData
        public IQueryable<JobTask> GetJobTasks()
        {
            return db.JobTasks;
        }

        // GET: api/JobTasksData/5
        [ResponseType(typeof(JobTask))]
        public IHttpActionResult GetJobTask(int id)
        {
            JobTask jobTask = db.JobTasks.Find(id);
            if (jobTask == null)
            {
                return NotFound();
            }

            return Ok(jobTask);
        }

        // PUT: api/JobTasksData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJobTask(int id, JobTask jobTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobTask.Id)
            {
                return BadRequest();
            }

            db.Entry(jobTask).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/JobTasksData/ - JobTaskId:" + jobTask.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/JobTasksData
        [ResponseType(typeof(JobTask))]
        public IHttpActionResult PostJobTask(JobTask jobTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.JobTasks.Add(jobTask);
            db.SaveChanges();
            LogManager.Log("POST: api/JobTasksData/ - JobTaskId:" + jobTask.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = jobTask.Id }, jobTask);
        }

        // DELETE: api/JobTasksData/5
        [ResponseType(typeof(JobTask))]
        public IHttpActionResult DeleteJobTask(int id)
        {
            JobTask jobTask = db.JobTasks.Find(id);
            if (jobTask == null)
            {
                return NotFound();
            }

            db.JobTasks.Remove(jobTask);
            db.SaveChanges();
            LogManager.Log("DELETE: api/JobTasksData/ - JobTaskId:" + jobTask.Id.ToString());

            return Ok(jobTask);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobTaskExists(int id)
        {
            return db.JobTasks.Count(e => e.Id == id) > 0;
        }
    }
}
