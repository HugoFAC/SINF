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
    public class ClientesController : ApiController
    {
        // GET: api/clientes/
        public IEnumerable<Lib_Primavera.Model.Cliente> Get()
        {
                return Lib_Primavera.PriIntegration.ListaClientes();
        }


        // GET api/clientes/5    
        public Object Get(string id)
        {
            Lib_Primavera.Model.Cliente cliente = Lib_Primavera.PriIntegration.GetCliente(id);
            if (cliente == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "O cliente com o id " + id + " não existe!");
            }
            else
            {
                return cliente;
            }
        }

        // GET api/clientes/top/5
        public Object Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if (id == "top" && isNumeric)
            {
                IEnumerable<Lib_Primavera.Model.Cliente> clientes = Lib_Primavera.PriIntegration.GetTopClientes(n);
                return clientes;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "O pedido foi mal efectuado!\nExemplo: api/clientes/top/5");
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
