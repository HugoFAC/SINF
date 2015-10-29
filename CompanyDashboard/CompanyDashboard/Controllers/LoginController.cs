using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CompanyDashboard.Lib_Primavera.Model;

namespace CompanyDashboard.Controllers
{
    public class LoginController : ApiController
    {
        //
        

        // POST api/Login/   
        public Boolean Post(Lib_Primavera.Model.Login login)
        {
            return Lib_Primavera.PriIntegration.Login(login);

        }
        // GET api/Login/ 
        public Boolean GET(string id,string param)
        {
            Lib_Primavera.Model.Login login = new Lib_Primavera.Model.Login();
            login.Password = param;
            login.Username = id;
            return Lib_Primavera.PriIntegration.Login(login);

        }
    }
}
