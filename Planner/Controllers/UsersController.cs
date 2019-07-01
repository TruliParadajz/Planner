using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Planner.DataAccessLayer;
using Planner.Models;

namespace Planner.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private PlannerContext db = new PlannerContext();

        // GET: api/Users
        [HttpGet]
        [Route("api/users")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            IEnumerable<User> user = null;
            user =  await db.Users.ToListAsync();
            return db.Users;
        }

        // GET: api/Users/5
        [HttpGet]
        [Route("api/users/{id}")]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.Include(x => x.Tasks).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut]
        [Route("api/users/{id}/{password}")]
        async public Task<IHttpActionResult> PutUser(int id, string password)
        {
            User user = new User();
            user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user == null)
            {
                return BadRequest("User not found with id: " + id);
            }

            user.Password = PasswordHashing(password);

            db.Entry(user).State = EntityState.Modified;

            try
            {
                
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Users
        [HttpPost]
        [Route("api/users")]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var exists = await db.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

            if (exists != null)
            {
                return BadRequest("User already exists!");
            }

            user.Password = PasswordHashing(user.Password);

            db.Users.Add(user);
            db.SaveChanges();

            return Ok(user);
        }

        // POST: api/Users/login
        [HttpPost]
        [Route("api/users/login")]
        public async Task<IHttpActionResult> CheckUser ([FromBody] User loginUser /*string email, string password*/)
        {
            User user = new User();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var hashedPassword = PasswordHashing(loginUser.Password);
                user = await db.Users.Where(x => x.Email == loginUser.Email && x.Password == hashedPassword).FirstAsync();
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest("User not found, check your entry!");
            }


        }

        // DELETE: api/Users/5
        [HttpDelete]
        [Route("api/users/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }




        // POST: api/Users
        [HttpPost]
        [Route("api/users/post/hash")]
        public async Task<IHttpActionResult> PostUserHash(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var exists = await db.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

            if (exists != null)
            {
                return BadRequest("User already exists!");
            }

            user.Password = PasswordHashing(user.Password);

            db.Users.Add(user);
            db.SaveChanges();

            return Ok(user);
        }


        // funkcija za sifriranje lozinke - SHA256
        private string PasswordHashing(string password)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var password_bytes = ASCIIEncoding.ASCII.GetBytes(password);

            byte[] passwordWithSalt = new byte[password_bytes.Length + salt.Length];

            for (int i = 0; i < password.Length; i++)
            {
                passwordWithSalt[i] = password_bytes[i];
            }

            for (int i = 0; i < salt.Length; i++)
            {
                passwordWithSalt[password_bytes.Length + i] = salt[i];
            }

            var hashedPassword = algorithm.ComputeHash(passwordWithSalt);
            var hashedStringPassword = Convert.ToBase64String(hashedPassword);

            return hashedStringPassword;
        }





    }
}