﻿using System;
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
        //
        // GET: /DocVenda/

        public IEnumerable<Lib_Primavera.Model.DocVenda> Get()
        {
            return Lib_Primavera.PriIntegration.Vendas_List();
        }


        // GET api/DocVenda/5    
        public object Get(string id)
        {
            if (id == "total")
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
                        throw new HttpResponseException(
                                Request.CreateResponse(HttpStatusCode.NotFound));

                    }
                    else
                    {
                        return docvenda;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }

        }


        // GET api/DocVenda/month/6   
        public IEnumerable<Lib_Primavera.Model.DocVenda> Get(string id, string param)
        {
            int n;
            bool isNumeric = int.TryParse(param, out n);
            if (isNumeric && id == "month")
            {
                return Lib_Primavera.PriIntegration.Vendas_List(id, n);
            }
            else
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));
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


        public HttpResponseMessage Put(int id, Lib_Primavera.Model.DocVenda dv)
        {

            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();

            try
            {
                erro = Lib_Primavera.PriIntegration.UpdDocVenda(dv);
                if (erro.Erro == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
                }
            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }



        public HttpResponseMessage Delete(string id)
        {


            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();

            try
            {

                erro = Lib_Primavera.PriIntegration.DelDocVenda(id);

                if (erro.Erro == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
                }

            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);

            }

        }
    }
}
