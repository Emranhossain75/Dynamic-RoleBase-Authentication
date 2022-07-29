using PermissionBaseRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PermissionBaseRole.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User model)
        {
            UserRolesEntities db = new UserRolesEntities();
            var result = db.Users.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();
            if (result != null)
            {
                UserSession u = new UserSession();
                u.user = result;
                u.Permission = result.Role.RoleDetails.Select(x => new RoleDetail
                {
                    PageName = x.Role.Name,
                    ID=x.ID,
                    Action = x.Action,
                    PageID= x.PageID,
                    RoleID = x.RoleID
                }).ToList();
                u.LoginTime = DateTime.Now;
                Session["UserSession"]=u;
                FormsAuthentication.SetAuthCookie(model.Email, false);
                return RedirectToAction("Index", "students");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        public class UserSession
        {
         public User user { get; set; }
            public DateTime LoginTime { get; set; }
            public List<RoleDetail> Permission { get; set; }
        }
        

    }
}