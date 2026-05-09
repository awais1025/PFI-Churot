using DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
    public class CoursesController : Controller
    {
        private void InitSessionVariables() { }



        public ActionResult List()
        {
            return View();
        }
        public ActionResult GetCourses(bool forceRefresh = false, string searchString = "")
        {
            try
            {
                bool searchChanged = Session["LastSearch"]?.ToString() != searchString;

                if (DB.Users.HasChanged || DB.Students.HasChanged || DB.Teachers.HasChanged || DB.Courses.HasChanged || forceRefresh || searchChanged)
                {
                    Session["LastSearch"] = searchString;
                    var courses = DB.Courses.ToList();

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        searchString = searchString.ToLower();
                        courses = courses.Where(c => c.Title.ToLower().Contains(searchString) || c.Code.ToLower().Contains(searchString)).ToList();
                    }

                    var sessionsList = courses.Select(c => c.Session).Distinct().ToList();
                    Session["CoursesSessionsList"] = sessionsList;
                    ViewBag.Search = searchString;

                    return PartialView(courses);
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