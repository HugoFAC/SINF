using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using CompanyDashboard.Lib_Primavera.Model;

namespace CompanyDashboard.Controllers
{
    public class LucroController : ApiController
    {
        // GET api/clientes/lucro/year/2014
        public Object Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if (id == "year" && isNumeric)
            {
                IEnumerable<Lib_Primavera.Model.Lucro> lucro = Lib_Primavera.PriIntegration.GetLucro(n);
                return lucro;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/clientes/top/5");
            }
        }

    }
}
