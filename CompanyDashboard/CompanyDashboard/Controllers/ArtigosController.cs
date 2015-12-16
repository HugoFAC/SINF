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
    public class ArtigosController : ApiController
    {
        // GET: api/artigos/

        public IEnumerable<Lib_Primavera.Model.Artigo> Get()
        {
            return Lib_Primavera.PriIntegration.ListaArtigos();
        }

        // GET api/artigos/5
        public Object Get(string id)
        {

            Lib_Primavera.Model.Artigo artigo = Lib_Primavera.PriIntegration.GetArtigo(id);
            if (artigo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "O artigo com o id " + id + " não existe!");
            }
            else
            {
                return artigo;
            }
        }

        // GET api/artigos/top/5
        public Object Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if (id == "top" && isNumeric)
            {
                IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.GetTopArtigos(n);
                return artigos;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/artigos/top/5");
            }
        }

        public Object Get(string id, string param, string param2)
        {
            int year;
            bool isNumeric = int.TryParse(param2, out year);
            if (id == "categoria" && param == "year" && isNumeric)
            {
                IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.GetCatArtigosYear(year);
                return artigos;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/artigos/top/5/year/2013");
            }
        }

        // GET pi/artigos/top/5/year/2013
        public Object Get(string id, string param, string param2, string param3)
        {
            int numArtigos, year;
            bool isNumeric = int.TryParse(param, out numArtigos);
            bool isNumeric2 = int.TryParse(param3, out year);
            if (id == "top" && param2 == "year" && isNumeric && isNumeric2)
            {
                IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.GetTopArtigosYear(numArtigos, year);
                return artigos;
            }
            else if (id == "topqtd" && param2 == "year" && isNumeric && isNumeric2)
            {
                IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.GetTopQtdArtigosYear(numArtigos, year);
                return artigos;
            }
            else if (id == "topcompras" && param2 == "year" && isNumeric && isNumeric2)
            {
                IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.GetTopArtigosCompradosYear(numArtigos, year);
                return artigos;
            }
            else if (id == "topstock" && param2 == "order" && isNumeric && (param3 == "asc" || param3 == "desc"))
            {
                IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.GetStockArtigos(numArtigos, param3);
                return artigos;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/artigos/top/5/year/2013");
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

