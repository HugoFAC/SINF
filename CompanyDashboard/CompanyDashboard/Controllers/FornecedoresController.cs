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
    public class FornecedoresController : ApiController
    {
        //
        // GET: /Fornecedores/

        // GET: /Clientes/

        public IEnumerable<Lib_Primavera.Model.Fornecedor> Get()
        {
            return Lib_Primavera.PriIntegration.ListaFornecedores();
        }


        // GET api/Fornecedor/F0005    
        public Fornecedor Get(string id)
        {
            Lib_Primavera.Model.Fornecedor fornecedor = Lib_Primavera.PriIntegration.GetFornecedor(id);
            if (fornecedor == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return fornecedor;
            }
        }


        /*public ActionResult Index()
        {
            return View();
        }
        */
    }
}
