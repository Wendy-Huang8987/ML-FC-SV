using GoMore_C2B1.DataContext;
using GoMore_C2B1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoMore_C2B1.Controllers
{
    public class UserModelController : Controller
    {
        private ApplicationDbContext manager = new ApplicationDbContext();
        // GET: UserModel
        public ActionResult UserManagement()
        {
            if (Session["Account"] != null)
            {
                if (Session["AccountType"].Equals("1"))
                {
                    List<Manager> managerobj = manager.Managers.ToList();
                    return View(managerobj);
                }
                else
                {
                    TempData["Msg"] = "not admin!";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["Msg"] = "Login first!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult UserManagement(Manager managers,string permission)
        {
            
            if (ModelState.IsValid)
            {

                if (managers.Email.Equals(Session["Account"].ToString()))
                {
                    List<Manager> managerobj_ = manager.Managers.ToList();
                    TempData["Msg"] = "can't change yourself account type";
                    return View(managerobj_);
                }
                else if (managers.AccountType.Equals(permission))
                {
                    List<Manager> managerobj_ = manager.Managers.ToList();
                    TempData["Msg"] = "Already is this Account Type";
                    return View(managerobj_);
                }
                else
                {
                    managers.AccountType = permission;
                    manager.Entry(managers).State = EntityState.Modified;
                    manager.SaveChanges();
                }
            }
            

            List<Manager> managerobj = manager.Managers.ToList();
            return View(managerobj);
        }

        public ActionResult ModelManagement()
        {
            if (Session["Account"] != null)
            {
                if (Session["AccountType"].Equals("1"))
                {
                    return View();
                }
                else
                {
                    TempData["Msg"] = "this acc no have permit!";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["Msg"] = "Login first!";
            return RedirectToAction("Index", "Home");
        }
    }
}