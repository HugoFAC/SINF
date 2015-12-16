using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyDashboard.Lib_Primavera.Model
{
    public class Artigo
    {
        public string CodArtigo
        {
            get;
            set;
        }

        public string DescArtigo
        {
            get;
            set;
        }

        public double TotalVendas
        {
            get;
            set;
        }

        public string Categoria
        {
            get;
            set;
        }

        public int Quantidade
        {
            get;
            set;
        }
        public int Stock
        {
            get;
            set;
        }
        public double Preco
        {
            get;
            set;
        }
    }
}