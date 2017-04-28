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
    public class PersonsDataController : ApiController
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // GET: api/GetPersonJobTasks/?PersonId=1
        [Route("api/GetPersonJobTasks/")]
        public List<JobTask> GetPersonJobTasks(int PersonId)
        {
            Person person = db.Persons.Find(PersonId);
            if (person == null)
            {
                return null;
            }
            return person.JobTasks;
        }

        // PUT: api/AddJobTaskToPerson/?PersonId=1&JobTaskId=1
        [HttpPut]
        [Route("api/AddJobTaskToPerson/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddJobTaskToPerson(int PersonId, int JobTaskId)
        {
            Person person = db.Persons.Find(PersonId);
            JobTask jobTask = db.JobTasks.Find(JobTaskId);
            if (person != null && jobTask != null)
            {
                try
                {
                    person.JobTasks.Add(jobTask);
                    db.Entry(person).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddJobTaskToPerson - PersonId:" + person.Id.ToString() + " JobTaskId:" + jobTask.Id.ToString());
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
        } // AddJobTaskToPerson

        // PUT: api/RemoveJobTaskFromPerson/?PersonId=1&JobTaskId=1
        [HttpPut]
        [Route("api/RemoveJobTaskFromPerson/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemoveJobTaskFromPerson(int PersonId, int JobTaskId)
        {
            Person person = db.Persons.Find(PersonId);
            JobTask jobTask = db.JobTasks.Find(JobTaskId);
            if (person != null && jobTask != null)
            {
                try
                {
                    person.JobTasks.Remove(jobTask);
                    db.Entry(person).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/RemoveJobTaskToPerson - PersonId:" + person.Id.ToString() + " JobTaskId:" + jobTask.Id.ToString());
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
        } // RemoveJobTaskFromPerson

        // GET: api/PersonsData
        public IQueryable<Person> GetPersons()
        {
            return db.Persons;
        }

        // GET: api/PersonsData/5
        [ResponseType(typeof(Person))]
        public IHttpActionResult GetPerson(int id)
        {
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/PersonsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            db.Entry(person).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/PersonsData/ - PersonId:" + person.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/PersonsData
        [ResponseType(typeof(Person))]
        public IHttpActionResult PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Persons.Add(person);
            db.SaveChanges();
            LogManager.Log("POST: api/PersonsData/ - PersonId:" + person.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = person.Id }, person);
        }

        // DELETE: api/PersonsData/5
        [ResponseType(typeof(Person))]
        public IHttpActionResult DeletePerson(int id)
        {
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            db.Persons.Remove(person);
            db.SaveChanges();
            LogManager.Log("DELETE: api/PersonsData/ - PersonId:" + person.Id.ToString());

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int id)
        {
            return db.Persons.Count(e => e.Id == id) > 0;
        }
    }
}
