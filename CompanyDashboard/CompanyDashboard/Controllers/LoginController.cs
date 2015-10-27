using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CompanyDashboard.Lib_Primavera.Model;

namespace CompanyDashboard.Controllers
{
    public class LoginController : ApiController
    {
        //
        // GET: /Login/

        // GET api/Login/post   
        public Boolean Post(Lib_Primavera.Model.Login login)
        {
            return Lib_Primavera.PriIntegration.Login(login);

        }

    }
}
