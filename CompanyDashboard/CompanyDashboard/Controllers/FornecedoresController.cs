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

        // GET: api/fornecedores/
        public IEnumerable<Lib_Primavera.Model.Fornecedor> Get()
        {
            return Lib_Primavera.PriIntegration.ListaFornecedores();
        }

        // GET api/fornecedor/F0005    
        public Object Get(string id)
        {
            Lib_Primavera.Model.Fornecedor fornecedor = Lib_Primavera.PriIntegration.GetFornecedor(id);
            if (fornecedor == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "O fornecedor com o id " + id + " não existe!");

            }
            else
            {
                return fornecedor;
            }
        }
        public Object Get(string id, string param, string param2, string param3)
        {
            int numFornecedores, year;
            bool isNumeric = int.TryParse(param, out numFornecedores);
            bool isNumeric2 = int.TryParse(param3, out year);
            if (id == "top" && param2 == "year" && isNumeric && isNumeric2)
            {
                IEnumerable<Lib_Primavera.Model.Fornecedor> fornecedores = Lib_Primavera.PriIntegration.GetTopFornecedoresYear(numFornecedores, year);
                return fornecedores;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/fornecedores/top/5/year/2013");
            }
        }
        //Precisamos de fazer novos webservices conforme o prof disse
        public Object Get(string id, string param, string param2, string param3, string param4)
        {
            //Adicionei mais parâmetros porque acho que vão ser precisos
            return 0;
        }
    }
}
