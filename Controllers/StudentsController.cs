using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
    public class StudentsController : Controller
    {
        // GET: Student
        private void InitSessionVariables()
        {
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult GetStudents(bool forceRefresh = false, string searchString = "")
        {
            try
            {
                bool searchChanged = Session["LastSearch"]?.ToString() != searchString;

                if (DB.Users.HasChanged || DB.Students.HasChanged || DB.Teachers.HasChanged || DB.Courses.HasChanged || forceRefresh || searchChanged)
                {
                    Session["LastSearch"] = searchString;
                    var students = DB.Students.ToList();

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        searchString = searchString.ToLower();
                        students = students.Where(s => s.FullName.ToLower().Contains(searchString) || s.Code.ToLower().Contains(searchString)).ToList();
                    }

                    Session["StudentsYearsList"] = students.Select(s => s.Year).Distinct().ToList();
                    ViewBag.Search = searchString;

                    return PartialView(students);
                }

                return Content("");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Content("Erreur interne " + ex.Message);
            }
        }
    }
}   
