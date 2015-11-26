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
    public class DocCompraController : ApiController
    {


        public IEnumerable<Lib_Primavera.Model.DocCompra> Get()
        {
            return Lib_Primavera.PriIntegration.Compras_List();
        }

        
        // GET api/cliente/5    
        public object Get(string id)
        {
            if (id == "total")
            {
                return Lib_Primavera.PriIntegration.Compras_Total();
            }
            else
            {
                int n;
                bool isNumeric = int.TryParse(id, out n);
                if (isNumeric)
                {
                    Lib_Primavera.Model.DocCompra doccompra = Lib_Primavera.PriIntegration.Compra_Get(id);
                    if (doccompra == null)
                    {
                        throw new HttpResponseException(
                                Request.CreateResponse(HttpStatusCode.NotFound));

                    }
                    else
                    {
                        return doccompra;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }

        }

        // GET api/Doccompra/months/x
        public IEnumerable<Lib_Primavera.Model.DocCompra> Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if (isNumeric && id == "months")
            {
                return Lib_Primavera.PriIntegration.Compras_List(id, n);
            }
            else
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));
        }

        // GET api/Doccompra/week/x
        // GET api/Doccompra/year/x
        // GET api/Doccompra/month/x
        public IEnumerable<Lib_Primavera.Model.DocCompra> Get(string id, string param, string param2)
        {
            if (id == "month" || id == "week" || id == "year")
            {
                return Lib_Primavera.PriIntegration.Compras_List_period(id, param, param2);
            }
            else
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));
        }


        /*public HttpResponseMessage Post(Lib_Primavera.Model.DocCompra dc)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.VGR_New(dc);

            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created, dc.id);
                string uri = Url.Link("DefaultApi", new { DocId = dc.id });
                response.Headers.Location = new Uri(uri);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }*/

    }
}
