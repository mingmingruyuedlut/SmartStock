using SmartStock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStock.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<UserModel> users = new List<UserModel>();
            for (int i = 0; i < 123; i++)
            {
                UserModel user = new UserModel() { UserId = i, UserName = "Name" + i.ToString(), UserEmail = "Name" + i.ToString() + "@163.com" };
                users.Add(user);
            }
            UserViewModel usersView = new UserViewModel() { Users = users };
            return View(usersView);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}