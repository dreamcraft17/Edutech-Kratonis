using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Edutech_Kratonis.Models;
using Edutech_Kratonis.ViewModel;
using System.Collections.Generic;


namespace Edutech_Kratonis.Controllers
{
    public class HomeController : Controller
    {

        EducationTechnologyEntities db = new EducationTechnologyEntities();

        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EducationTechUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.et_user.SingleOrDefault(u => u.username == model.username && u.password == model.password);
                if (user != null && user.activated !="a")
                {
                    FormsAuthentication.SetAuthCookie(user.username, false);

                    Session["UserId"] = user.id;
                    Session["UserName"] = user.username;
                    Session["UserEmail"] = user.email;
                    Session["Name"] = user.name;
                   

                    ViewBag.Message = "Login successful! Welcome, " + user.name;

                    return RedirectToAction("Index", "Home");
                }
                else if (user.activated == "i")
                {
                    TempData["ErrorMessage"] = "Account Not activated.";
                }
                else
                {
                    TempData["ErrorMessage2"] = "Invalid username and Password";
                }
            }
            return View(model);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(et_user model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.et_user.SingleOrDefault(u => u.username == model.username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username Already Exists");
                    return View(model);
                }

                if (existingUser == null)
                {
                    model.password = encrypt.GetMD5(model.password);
                    model.activated = "I";
                    model.created_at = DateTime.Now;
                    model.updated_at = DateTime.Now;
                    db.et_user.Add(model);
                    db.SaveChanges();

                    var emailfrom = "dozer.napitupulu@member.maribelajar.org";
                    var password = "qwertylolipop170602";
                    var to = "dozernapitupulu@gmail.com";
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "10.1.30.16";
                    smtpClient.Port = 25;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(emailfrom, password);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = false;
                    var from = emailfrom;
                    MailMessage mail = new MailMessage();
                    mail.To.Add(to);
                    mail.From = new MailAddress(from);
                    mail.Body ="Dear, Admin, Someone just registered an account."+ "<br/><br/>"
                        + "Request by : " + model.name+ "<br/>"
                        + "email : " + model.email+ "<br/><br/>"
                        + "http://edutech-kratonis.com/";
                    mail.Subject = "Registered Account need action, Request by: " + model.name + ", email: " + model.email;
                    mail.IsBodyHtml = true;
                    smtpClient.Send(mail);

                    TempData["ActivationMessage"] = "Account requested, wait for PIC to activate the account";
                    TempData["ShowActivationModal"] = true;

                    return RedirectToAction("Index");
                }

                //var newUser = new et_user
                //{
                //    name = model.name,
                //    email = model.email,
                //    username = model.username,
                //    password = model.password,
                //    activated = "i"
                //};

                //db.et_user.Add(newUser);
                //db.SaveChanges();

                //FormsAuthentication.SetAuthCookie(newUser.username, false);

                //Session["UserId"] = newUser.id;
                //Session["UserName"] = newUser.username;
                //Session["UserEmail"] = newUser.email;

      
                

                //return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

      


        public ActionResult Logout()
        {
            
            Session.Clear();
            FormsAuthentication.SignOut();

            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Courses()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Mentor()
        {
            ViewBag.Message = "your Mentor page";

            return View();
        }
    }
}