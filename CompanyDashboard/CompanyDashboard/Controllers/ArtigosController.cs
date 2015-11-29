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

        //Precisamos de fazer novos webservices conforme o prof disse
        public Object Get(string id, string param, string param2, string param3, string param4)
        {
            //Adicionei mais parâmetros porque acho que vão ser precisos
            return 0;
        }
    }
}

