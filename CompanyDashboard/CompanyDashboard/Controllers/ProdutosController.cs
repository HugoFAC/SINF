using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyDashboard.Controllers
{
    public class ProdutosController : Controller
    {
        //
        // GET: /Produtos/

        public ActionResult id(string id)
        {
            ViewBag.ID = id;
            return View();
        }

    }
}
