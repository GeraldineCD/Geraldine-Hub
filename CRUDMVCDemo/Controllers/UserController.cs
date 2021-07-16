using CRUDMVCDemo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Encoder;


namespace CRUDMVCDemo.Controllers
{
    public class UserController : Controller
    {
        schoolManagementEntities6 db = new schoolManagementEntities6();
        UserAccount std = new UserAccount();
        Encoder.Encoder encoder = new Encoder.Encoder();
        // GET: User
        public ActionResult UserAccount()
        {

            std.SystemDate = DateTime.Now;
            return View(std);
        }

        [HttpPost]
        public ActionResult UserAccount(UserAccount User)
        {
            string Encrypted = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    if(User.Username != null & User.Password != null && User.Fullname != null)
                    {
                        var std = db.UserAccounts.Where(a => a.Username == User.Username).FirstOrDefault();
                        if(std == null)
                        {
                            Encrypted = encoder.EncryptPassword(User.Password);
                            User.EncryptedPassword = Encrypted;
                            User.isActive = true;
                            db.UserAccounts.Add(User);
                            db.SaveChanges();
                            return RedirectToAction("UserLogin");
                        }
                        else
                        {
                            ModelState.AddModelError("validation", "Username is already existing !!!");
                        }
                       
                        
                        
                    }
                    else if(User.Username == null & User.Password != null && User.Fullname != null)
                    {
                      
                        ModelState.AddModelError("Username", "   Please Enter Username");
                        
                    }
                    else if(User.Username != null & User.Password == null && User.Fullname != null)
                    {
                        ModelState.AddModelError("Password", "   Please Enter Password");
                        
                    }
                    else if(User.Username != null & User.Password != null && User.Fullname == null)
                    {
                        ModelState.AddModelError("Fullname", "   Please Enter Fullname");
                    }
                    else
                    {
                        ModelState.AddModelError("validation", "Please do not leave blank");
                    }


                }
            }
            catch (DbEntityValidationException ex)
            {
             
            }

            return View();
        }

        public ActionResult UserEdit(int? id)
        {
            var std = db.UserAccounts.Where(s => s.gkey == id).FirstOrDefault();
            std.SystemDate = DateTime.Now;
            return View(std);
        }

        [HttpPost]
        public ActionResult UserEdit(UserAccount User)
        {
            string Encrypted = string.Empty;


            try
            {

                if (ModelState.IsValid)
                {
                    var std = db.UserAccounts.Where(s => s.gkey == User.gkey).FirstOrDefault();
                    Encrypted = encoder.EncryptPassword(User.Password);
                    std.Password = User.Password;
                    std.EncryptedPassword = Encrypted;
                    std.Username = User.Username;
                    std.Fullname = User.Fullname;
                    std.isActive = true;
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
               
            }

            return View(User);
        }

        public ActionResult UserDelete(int? id)
        {
            var std = db.UserAccounts.Where(s => s.gkey == id).FirstOrDefault();
            std.SystemDate = DateTime.Now;
            return View(std);
        }

        [HttpPost]
        public ActionResult UserDelete(int id)
        {
           
            try
            {

                if (ModelState.IsValid)
                {
                    var std = db.UserAccounts.Where(s => s.gkey == id).FirstOrDefault();
                    db.UserAccounts.Remove(std);
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
               
            }

            return RedirectToAction("UserDetails");
        }
        public ActionResult UserDetails()
        {

            return View();
        }
        [HttpGet]
        public ActionResult UserDetails(UserAccount User)
        {
            
            UserAccount model = new UserAccount();
            try
            {
                if (ModelState.IsValid)
                {
                    var std = db.UserAccounts;
                    model.userlist = std.ToList();

                    return View(model);
                }
            }
            catch (DbEntityValidationException ex)
            {
               
            }
            return View();


        }

        public ActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]

        public ActionResult UserLogin(UserAccount account)
        {
            string decryptpassword = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {                   

                    var obj = db.UserAccounts.Where(a => a.Username == account.Username).FirstOrDefault();
                    if (obj != null)
                    {
                        if(account.Password != null)
                        {
                            decryptpassword = encoder.DecryptPassword(obj.EncryptedPassword);

                            if (account.Password.Equals(decryptpassword))
                            {
                                Session["UserName"] = obj.Username;
                                return RedirectToAction("Dashboard");
                            }
                            else
                            {
                                ModelState.AddModelError("validation", "Please Enter the correct Password ");
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("validation", "Please Enter the correct Password ");
                        }

                    }
                    else
                    {
                        if (account.Username == null && account.Password == null)
                        {
                            ModelState.AddModelError("validation", "Please Enter the Username and Password");
                        }
                        else if (account.Username == null)
                        {
                            ModelState.AddModelError("validation", "Please Enter the correct Username ");
                        }
                        else
                        {
                            ModelState.AddModelError("validation", "Please Enter the correct Password ");
                        }


                    }

                }
               
                
            }
            catch
            {

            }

            return View();
        }

        public ActionResult Dashboard()
        {
            if(Session["UserName"] != null )
            {
                return View(); 
            }
            else
            {
                return RedirectToAction("UserLogin");
            }
        }
    }
}