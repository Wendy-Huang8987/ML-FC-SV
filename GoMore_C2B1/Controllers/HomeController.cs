using GoMore_C2B1.DataContext;
using GoMore_C2B1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace GoMore_C2B1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext user = new ApplicationDbContext();
        private ApplicationDbContext manager = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register([Bind(Include = "Firstname,Lastname,Username,Email,Occupation,CompanyOrOrganization,Country,Password,Confitmpassword")] RegisterView uSERCLASS)
        {
            USERCLASS userobj = new USERCLASS();
            Manager managerobj = new Manager();
            var regex = new Regex(@"
                        (?=.*[0-9])                     #必须包含数字
                        (?=.*[a-zA-Z])                  #必须包含小写或大写字母
                        (?=.*[^a-zA-Z0-9])              #必须包含特殊符号
                        .{8,30}                         #至少8个字符，最多30个字符
                        ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            //校验密码是否符合

            bool pwdIsMatch = regex.IsMatch(uSERCLASS.Password);
            if (pwdIsMatch) {
                if (uSERCLASS.Password.Equals(uSERCLASS.Confitmpassword))
                {
                    if (ModelState.IsValid)
                    {
                        Random generator = new Random();
                        userobj.uid = generator.Next(0, 1000000).ToString("D6");
                        userobj.Firstname = uSERCLASS.Firstname;
                        userobj.Lastname = uSERCLASS.Lastname;
                        userobj.Username = uSERCLASS.Username;
                        managerobj.Email = userobj.Email = uSERCLASS.Email;
                        userobj.Occupation = uSERCLASS.Occupation;
                        userobj.CompanyOrOrganization = uSERCLASS.CompanyOrOrganization;
                        userobj.Country = uSERCLASS.Country;
                        userobj.Password = uSERCLASS.Password;
                        user.USER.Add(userobj);
                        managerobj.AccountType = "0";
                        manager.Managers.Add(managerobj);
                        user.SaveChanges();
                        manager.SaveChanges();
                        TempData["Msg"] = "Register Successful";
                        return RedirectToAction("Index", "Home");
                    }
                    TempData["Msg"] = "Register Error";
                    return RedirectToAction("Index", "Home");
                }
                else {
                    TempData["Msg"] = "Password not same";
                    return RedirectToAction("Index", "Home");
                }
                
            }
            TempData["Msg"] = "密碼無效！";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView) {

            List<USERCLASS> userclass = user.USER.Where(x => x.Email.Equals(loginView.Email)).ToList();
            Manager GetAccountType = manager.Managers.Find(loginView.Email);

            if (userclass != null)
            {
                
                foreach (USERCLASS user in userclass)
                {
                    if (loginView.Password.Equals(user.Password))
                    {
                        Session["uid"] = user.uid;
                        Session["Account"] = user.Email;
                        Session["Firstname"] = user.Firstname;
                        Session["Lastname"] = user.Lastname;
                        Session["Username"] = user.Username;
                        Session["Country"] = user.Country;
                        Session["CompanyOrOrganization"] = user.CompanyOrOrganization;
                        if (GetAccountType != null && GetAccountType.AccountType.Equals("1"))
                        {
                            Session["Identity"] = "Admin";
                            Session["AccountType"] = "1";
                        }
                        else {
                            Session["Identity"] = "Normal user";
                            Session["AccountType"] = "0";
                        }
                        TempData["Msg"] = "Welcome " + user.Lastname + " " + user.Firstname + "!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Msg"] = "Password not correct!";
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Msg"] = "No have this account.";
                return RedirectToAction("Index", "Home");
            }
            else {
                TempData["Msg"] = "DB error!";
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult Logout()
        {
            Session.Clear();
            TempData["Message"] = "已登出";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ChangeLanguage(string lang)
        {
            // 将所选的语言设置为会话变量或 Cookie，以便后续请求使用
            Session["Language"] = lang;

            return new EmptyResult();
        }

    }
}