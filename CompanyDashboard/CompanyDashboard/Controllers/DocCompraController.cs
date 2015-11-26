using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CompanyDashboard.Lib_Primavera.Model;
using System.Globalization;

namespace CompanyDashboard.Controllers
{
    public class DocCompraController : ApiController
    {


        public IEnumerable<Lib_Primavera.Model.DocCompra> Get()
        {
            return Lib_Primavera.PriIntegration.Compras_List();
        }

        
        // GET api/doccompra/5    
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

        public IEnumerable<Lib_Primavera.Model.DocCompra> Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if (isNumeric && id == "month")
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

        public IEnumerable<Lib_Primavera.Model.DocCompra> Get(string id, string param, string param2)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            bool isNumeric2 = int.TryParse(param2, out n);

            if (id == "data")
            {
                string[] formats = {"d-M-yyyy", "dd-MM-yyyy", "d-MM-yyyy", "dd-M-yyyy"};
                DateTime dateValue;
                DateTime dateValue2;

                if (DateTime.TryParseExact(param, formats, new CultureInfo("en-US"), DateTimeStyles.None, out dateValue) 
                    && DateTime.TryParseExact(param2, formats, new CultureInfo("en-US"), DateTimeStyles.None, out dateValue2))
                {
                    return Lib_Primavera.PriIntegration.Compras_List(id, dateValue, dateValue2);
                }
                else
                {
                    throw new HttpResponseException(
                       Request.CreateResponse(HttpStatusCode.BadRequest));
                }

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
