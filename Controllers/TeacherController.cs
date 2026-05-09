using DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
    public class TeachersController : Controller
    {
        private void InitSessionVariables() { }


        public ActionResult List()
        {
            return View();
        }
        public ActionResult GetTeachers(bool forceRefresh = false, string searchString = "")
        {
            try
            {
                bool searchChanged = Session["LastSearch"]?.ToString() != searchString;

                if (DB.Users.HasChanged || DB.Teachers.HasChanged || forceRefresh || searchChanged)
                {
                    Session["LastSearch"] = searchString;
                    var teachers = DB.Teachers.ToList();

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        searchString = searchString.ToLower();
                        teachers = teachers.Where(t => t.FullName.ToLower().Contains(searchString) || t.Code.ToLower().Contains(searchString)).ToList();
                    }

                    ViewBag.Search = searchString;

                    return PartialView(teachers);
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