using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Planner.DataAccessLayer;
using Planner.Models;

namespace Planner.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TasksController : ApiController
    {
        private PlannerContext db = new PlannerContext();

        // GET: api/Tasks
        [HttpGet]
        [Route("api/tasks")]
        public IQueryable<Models.Task> GetTasks()
        {
            return db.Tasks;
        }


        // GET: api/Tasks/5
        [HttpGet]
        [Route("api/tasks/{id}")]
        public IHttpActionResult GetTask(int id)
        {
            Models.Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [HttpPut]
        [Route("api/tasks")]
        async public Task<IHttpActionResult> PutTask([FromBody] Models.Task task)
        {
            int id = task.Id;
            Models.Task taskTemp = new Models.Task();

            taskTemp = await db.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskTemp.Id)
            {
                return BadRequest();
            }

            taskTemp.Text = task.Text;
            taskTemp.Solved = task.Solved;
            taskTemp.Date = task.Date;

            db.Entry(taskTemp).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(taskTemp);
        }

        [HttpPost]
        // POST: api/Tasks
        [Route("api/tasks")]
        public IHttpActionResult PostTask([FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            db.Tasks.Add(task);
            db.SaveChanges();

            return Ok(task);
        }

        [HttpDelete]
        // DELETE: api/Tasks/5
        [Route("api/tasks/{id}")]
        public IHttpActionResult DeleteTask(int id)
        {
            Models.Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Id == id) > 0;
        }
    }
}