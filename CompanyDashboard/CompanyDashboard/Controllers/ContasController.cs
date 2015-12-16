using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyDashboard.Controllers
{
    public class ContasController : ApiController
    {
        // GET api/accounts/receber{pagar}/year
        public Object Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if ((id == "receber" || id == "pagar") && isNumeric)
            {
                float valor = Lib_Primavera.PriIntegration.GetContaTotal(id, n);
                return valor;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/contas/receber/2013");
            }
        }

    }
}
