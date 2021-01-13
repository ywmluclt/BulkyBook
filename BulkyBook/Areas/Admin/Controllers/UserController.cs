using BulkyBook.DataAccess.Data;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allUsers = _db.ApplicationUsers.Include(x => x.Company).ToList();
            var roles = _db.Roles.ToList();
            var userRoles = _db.UserRoles.ToList();
            foreach (var user in allUsers)
            {
                var roleId = userRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;
                var roleName = roles.FirstOrDefault(x => x.Id == roleId).Name;
                user.Role = roleName;
                if (user.Company == null)
                {
                    user.Company = new Models.Company()
                    {
                        Name = string.Empty
                    };
                }
            }

            return Json(new { data = allUsers });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var userFromDb = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (userFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
            {
                userFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                userFromDb.LockoutEnd = DateTime.Now.AddYears(100);
            }
            _db.SaveChanges();

            return Json(new { success = true, message = "Operation successful." });
        }

        #endregion
    }
}
