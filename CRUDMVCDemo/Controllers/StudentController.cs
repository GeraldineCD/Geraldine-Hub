using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUDMVCDemo.Models;

namespace CRUDMVCDemo.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            Student std = new Student();
            std.SystemDate = DateTime.Now;
            return View(std);
        }

        [HttpPost]
        public ActionResult Index(Student student)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    schoolManagementEntities5 db = new schoolManagementEntities5();
                    db.Students.Add(student);                   
                    db.SaveChanges();
                }
            }
            catch(DbEntityValidationException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return View(student);
        }
    }
}