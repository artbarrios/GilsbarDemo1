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
    public class BuildingsDataController : ApiController
    {
        private GilsbarDemo1Context db = new GilsbarDemo1Context();

        // GET: api/GetBuildingDepartments/?BuildingId=1
        [Route("api/GetBuildingDepartments/")]
        public List<Department> GetBuildingDepartments(int BuildingId)
        {
            Building building = db.Buildings.Find(BuildingId);
            if (building == null)
            {
                return null;
            }
            return building.Departments;
        }

        // PUT: api/AddDepartmentToBuilding/?BuildingId=1&DepartmentId=1
        [HttpPut]
        [Route("api/AddDepartmentToBuilding/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddDepartmentToBuilding(int BuildingId, int DepartmentId)
        {
            Building building = db.Buildings.Find(BuildingId);
            Department department = db.Departments.Find(DepartmentId);
            if (building != null && department != null)
            {
                try
                {
                    building.Departments.Add(department);
                    db.Entry(building).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddDepartmentToBuilding - BuildingId:" + building.Id.ToString() + " DepartmentId:" + department.Id.ToString());
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
        } // AddDepartmentToBuilding

        // PUT: api/RemoveDepartmentFromBuilding/?BuildingId=1&DepartmentId=1
        // NOTE: the Department.BuildingId property is not nullable so this routine must be omitted

        // GET: api/BuildingsData
        public IQueryable<Building> GetBuildings()
        {
            return db.Buildings;
        }

        // GET: api/BuildingsData/5
        [ResponseType(typeof(Building))]
        public IHttpActionResult GetBuilding(int id)
        {
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return NotFound();
            }

            return Ok(building);
        }

        // PUT: api/BuildingsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBuilding(int id, Building building)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != building.Id)
            {
                return BadRequest();
            }

            db.Entry(building).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/BuildingsData/ - BuildingId:" + building.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(id))
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

        // POST: api/BuildingsData
        [ResponseType(typeof(Building))]
        public IHttpActionResult PostBuilding(Building building)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Buildings.Add(building);
            db.SaveChanges();
            LogManager.Log("POST: api/BuildingsData/ - BuildingId:" + building.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = building.Id }, building);
        }

        // DELETE: api/BuildingsData/5
        [ResponseType(typeof(Building))]
        public IHttpActionResult DeleteBuilding(int id)
        {
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return NotFound();
            }

            db.Buildings.Remove(building);
            db.SaveChanges();
            LogManager.Log("DELETE: api/BuildingsData/ - BuildingId:" + building.Id.ToString());

            return Ok(building);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BuildingExists(int id)
        {
            return db.Buildings.Count(e => e.Id == id) > 0;
        }
    }
}
