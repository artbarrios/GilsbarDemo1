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
    public class ManagersDataController : ApiController
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // GET: api/GetManagerPersons/?ManagerId=1
        [Route("api/GetManagerPersons/")]
        public List<Person> GetManagerPersons(int ManagerId)
        {
            Manager manager = db.Managers.Find(ManagerId);
            if (manager == null)
            {
                return null;
            }
            return manager.Persons;
        }

        // PUT: api/AddPersonToManager/?ManagerId=1&PersonId=1
        [HttpPut]
        [Route("api/AddPersonToManager/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddPersonToManager(int ManagerId, int PersonId)
        {
            Manager manager = db.Managers.Find(ManagerId);
            Person person = db.Persons.Find(PersonId);
            if (manager != null && person != null)
            {
                try
                {
                    manager.Persons.Add(person);
                    db.Entry(manager).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddPersonToManager - ManagerId:" + manager.Id.ToString() + " PersonId:" + person.Id.ToString());
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
        } // AddPersonToManager

        // PUT: api/RemovePersonFromManager/?ManagerId=1&PersonId=1
        // NOTE: the Person.ManagerId property is not nullable so this routine must be omitted

        // GET: api/ManagersData
        public IQueryable<Manager> GetManagers()
        {
            return db.Managers;
        }

        // GET: api/ManagersData/5
        [ResponseType(typeof(Manager))]
        public IHttpActionResult GetManager(int id)
        {
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return NotFound();
            }

            return Ok(manager);
        }

        // PUT: api/ManagersData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutManager(int id, Manager manager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != manager.Id)
            {
                return BadRequest();
            }

            db.Entry(manager).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/ManagersData/ - ManagerId:" + manager.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManagerExists(id))
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

        // POST: api/ManagersData
        [ResponseType(typeof(Manager))]
        public IHttpActionResult PostManager(Manager manager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Managers.Add(manager);
            db.SaveChanges();
            LogManager.Log("POST: api/ManagersData/ - ManagerId:" + manager.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = manager.Id }, manager);
        }

        // DELETE: api/ManagersData/5
        [ResponseType(typeof(Manager))]
        public IHttpActionResult DeleteManager(int id)
        {
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return NotFound();
            }

            db.Managers.Remove(manager);
            db.SaveChanges();
            LogManager.Log("DELETE: api/ManagersData/ - ManagerId:" + manager.Id.ToString());

            return Ok(manager);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ManagerExists(int id)
        {
            return db.Managers.Count(e => e.Id == id) > 0;
        }
    }
}
