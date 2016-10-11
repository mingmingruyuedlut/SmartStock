using SmartStock.Core.DBManager;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStock.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult Index()
        {
            List<RoleInfo> roles = new RoleManager().GetAllRoles();
            return View(roles);
        }
    }
}