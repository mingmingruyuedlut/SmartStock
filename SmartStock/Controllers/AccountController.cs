using SmartStock.Core.DBManager;
using SmartStock.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStock.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            LoginUser loginUser =  new AccountManager().AccountSignInValidation(model.UserName, model.Password);

            if (loginUser.ValidateResult.ValidateResultType == LoginUserValidateResultType.Success)
            {
                Session["CurrentLoginUser"] = loginUser;
                return RedirectToAction("Index", "Dashboard");
            }
            else if (loginUser.ValidateResult.ValidateResultType == LoginUserValidateResultType.UserNotExist)
            {
                ModelState.AddModelError("UserName", "用户不存在");
                return View(model);
            }
            else if (loginUser.ValidateResult.ValidateResultType == LoginUserValidateResultType.PasswordInvalid)
            {
                ModelState.AddModelError("Password", "用户名与密码不匹配");
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            Session["CurrentLoginUser"] = null;
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            RegisterInfo registerUser = new AccountManager().AccountRegister(model);
            if (registerUser.ValidateResult.ValidateResultType == RegisterValidateResultType.UserIsExist)
            {
                ModelState.AddModelError("UserName", "用户名已经存在");
                return View(model);
            }
            else if (registerUser.ValidateResult.ValidateResultType == RegisterValidateResultType.InvalidPassword)
            {
                ModelState.AddModelError("Password", "请输入相同的密码与确认密码");
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}