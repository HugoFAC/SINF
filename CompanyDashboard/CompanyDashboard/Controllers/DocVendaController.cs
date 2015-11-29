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
    public class DocVendaController : ApiController
    {
        // GET: api/docvenda/

        public IEnumerable<Lib_Primavera.Model.DocVenda> Get()
        {
            return Lib_Primavera.PriIntegration.Vendas_List();
        }

        // GET api/docvenda/5    
        public object Get(string id)
        {
            if (id == "totalabs")
            {
                return Lib_Primavera.PriIntegration.Vendas_Total();
            }
            else 
            {
                int n;
                bool isNumeric = int.TryParse(id, out n);
                if (isNumeric)
                {
                    Lib_Primavera.Model.DocVenda docvenda = Lib_Primavera.PriIntegration.Venda_Get(id);
                    if (docvenda == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "O DocVenda com o id " + id + " não existe!");

                    }
                    else
                    {
                        return docvenda;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/docvenda/totalabs ou api/docvenda/[idDocVenda]");
                }
            }
        }

        // GET api/docvenda/months/6   
        public Object Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if (isNumeric && id == "months")
            {
                return Lib_Primavera.PriIntegration.Vendas_List(n);
            }
            else if (isNumeric && id == "years")
            {
                return Lib_Primavera.PriIntegration.Vendas_List_Years2(n);
            }
            else if (isNumeric && id == "year")
            {
                return Lib_Primavera.PriIntegration.Vendas_List_Year(n);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: (1)api/docvenda/months/[numMeses] (2)api/docvenda/years/[numAnos] (3)api/docvenda/weeks/[numSemanas] (4)api/docvenda/year/[ano]");
            }
        }
        // GET api/docvenda/week/x
        // GET api/docvenda/year/x
        // GET api/docvenda/month/x
        public Object Get(string id, string param, string param2)
        {
            string pattern = "([1-9]|0[1-9]|[12][0-9]|3[01])[-]([1-9]|0[1-9]|1[012])[-][0-9]{4}$";

            if(!(System.Text.RegularExpressions.Regex.IsMatch(param, pattern) && System.Text.RegularExpressions.Regex.IsMatch(param2, pattern))){
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Data inválida, deve estar no formato dd-mm-aaaa!\nExemplo: 01-01-1901");
            }

            if (id == "month" || id == "week" || id == "year")
            {
                return Lib_Primavera.PriIntegration.Vendas_List_period(param,param2);
            }
            else if (id == "totalper")
            {
                return Lib_Primavera.PriIntegration.Vendas_Total_per(param, param2);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/docvenda/month/DD-MM-YYYY/DD-MM-YYYY");
            }
        }

        public HttpResponseMessage Post(Lib_Primavera.Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.Vendas_New(dv);

            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created, dv.id);
                string uri = Url.Link("DefaultApi", new {DocId = dv.id });
                response.Headers.Location = new Uri(uri);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
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
