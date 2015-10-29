using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyDashboard.Lib_Primavera.Model
{
    public class DocVenda
    {

        public string id
        {
            get;
            set;
        }

        public string Entidade
        {
            get;
            set;
        }

        public string Filial
        {
            get;
            set;
        }

        public int NumDoc
        {
            get;
            set;
        }

        public DateTime Data
        {
            get;
            set;
        }

        public double TotalMerc
        {
            get;
            set;
        }

        public string Serie
        {
            get;
            set;
        }

        public string Tipodoc
        {
            get;
            set;
        }
        public string TipoEntidade
        {
            get;
            set;
        }
        public List<Model.LinhaDocVenda> LinhasDoc

        {
            get;
            set;
        }
 

    }
}