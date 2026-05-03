using DAL;
using EmailHandling;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Registrar
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private static Timer appTimer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var culture = new CultureInfo("fr-FR");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            
            // cleaning
            
            foreach (var uvEmail in DB.UnverifiedEmails.ToList().Copy())
            {
                if (uvEmail.User == null) DB.UnverifiedEmails.Delete(uvEmail.Id);
            }
            foreach (RenewPasswordCommand renewPC in DB.RenewPasswordCommands.ToList().Copy())
            {
                if (renewPC.User == null) DB.RenewPasswordCommands.Delete(renewPC.Id);
            }
            foreach (Models.Event @event in DB.Events.ToList().Copy())
            {
                if (@event.User == null) DB.RenewPasswordCommands.Delete(@event.Id);
            }
            foreach (var login in DB.Logins.ToList().Copy())
            {
                if (login.User == null) DB.Logins.Delete(login.Id);
            }
        }
    
        protected void Session_Start()
        {
            // do session intialisations

        }
        protected void Session_End()
        {
            var connectedUser = Models.User.ConnectedUser;
            if (connectedUser != null)
                connectedUser.Online = false;
        }
        protected void Application_End(object sender, EventArgs e)
        {
            /*if (appTimer != null)
            {
                appTimer.Enabled = false;
                appTimer.Dispose();
            }*/
        }
    }
}
