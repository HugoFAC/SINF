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
        public double PVP1
        {
            get;
            set;
        }
        public double PVP2
        {
            get;
            set;
        }
        public double PVP3
        {
            get;
            set;
        }
        public double PVP4
        {
            get;
            set;
        }
        public double PVP5
        {
            get;
            set;
        }
        public double PVP6
        {
            get;
            set;
        }
        public string CodBarras
        {
            get;
            set;
        }
        public double Iva
        {
            get;
            set;
        }
        public DateTime DataUltEntrada
        {
            get;
            set;
        }
        public DateTime DataUltSaida
        {
            get;
            set;
        }
    }
}