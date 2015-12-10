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

        // GET api/doccompra/5    
        public object Get(string id)
        {
            if (id == "totalabs")
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
                        return Request.CreateResponse(HttpStatusCode.NotFound, "O DocCompra com o id " + id + " não existe!");

                    }
                    else
                    {
                        return doccompra;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/doccompra/totalabs ou api/doccompra/[idDocCompra]");
                }
            }

        }
        // GET api/doccompra/months/x
        public Object Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if (isNumeric && id == "months")
            {
                return Lib_Primavera.PriIntegration.Compras_List_Months(n);
            }
            else if (isNumeric && id == "years")
            {
                return Lib_Primavera.PriIntegration.Compras_List_Years2(n);
            }
            else if (isNumeric && id == "year")
            {
                return Lib_Primavera.PriIntegration.Compras_List_Year(n);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: (1)api/doccompra/months/[numMeses] (2)api/doccompra/years/[numAnos] (3)api/doccompra/weeks/[numSemanas] (4)api/doccompra/year/[ano]");
            }
        }

        // GET api/Doccompra/week/DD-MM-YYYY/DD-MM-YYYY
        // GET api/Doccompra/year/DD-MM-YYYY/DD-MM-YYYY
        // GET api/Doccompra/month/DD-MM-YYYY/DD-MM-YYYY
        public Object Get(string id, string param, string param2)
        {
            string pattern = "([1-9]|0[1-9]|[12][0-9]|3[01])[-]([1-9]|0[1-9]|1[012])[-][0-9]{4}$";

            switch (id)
            {
                case "totalabs":
                    int year;
                    bool isNumeric = int.TryParse(param2, out year);
                    if (isNumeric && param == "year")
                    {
                        return Lib_Primavera.PriIntegration.ComprasTotalAbsYear(year);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/doccompra/totalabs/year/YYYY");
                    };
                case "week":
                    if (!(System.Text.RegularExpressions.Regex.IsMatch(param, pattern) && System.Text.RegularExpressions.Regex.IsMatch(param2, pattern)))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Data inválida, deve estar no formato dd-mm-aaaa!\nExemplo: 01-01-1901");
                    }
                    return Lib_Primavera.PriIntegration.Compras_List_period(param, param2);
                case "month":
                    if (!(System.Text.RegularExpressions.Regex.IsMatch(param, pattern) && System.Text.RegularExpressions.Regex.IsMatch(param2, pattern)))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Data inválida, deve estar no formato dd-mm-aaaa!\nExemplo: 01-01-1901");
                    }
                    return Lib_Primavera.PriIntegration.Compras_List_period(param, param2);
                case "year":
                    if (!(System.Text.RegularExpressions.Regex.IsMatch(param, pattern) && System.Text.RegularExpressions.Regex.IsMatch(param2, pattern)))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Data inválida, deve estar no formato dd-mm-aaaa!\nExemplo: 01-01-1901");
                    }
                    return Lib_Primavera.PriIntegration.Compras_List_period(param, param2);
                case "totalper":
                    if (!(System.Text.RegularExpressions.Regex.IsMatch(param, pattern) && System.Text.RegularExpressions.Regex.IsMatch(param2, pattern)))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Data inválida, deve estar no formato dd-mm-aaaa!\nExemplo: 01-01-1901");
                    }
                    return Lib_Primavera.PriIntegration.Compras_Total_per(param, param2);
                default:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/doccompra/month/DD-MM-YYYY/DD-MM-YYYY ou api/doccompra/totalabs/year/YYYY");
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
