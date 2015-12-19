using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Interop.ErpBS800;
using Interop.StdPlatBS800;
using Interop.StdBE800;
using Interop.GcpBE800;
using ADODB;
using Interop.IGcpBS800;
using System.Globalization;
//using Interop.StdBESql800;
//using Interop.StdBSSql800;

namespace CompanyDashboard.Lib_Primavera
{
    public class PriIntegration
    {
        # region Login

        public static Boolean Login(Model.Login login)
        {
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), login.Username.Trim(), login.Password.Trim()) == true)
            {
                return true;
            }
            else
                return false;
        }

        #endregion Login;
        # region Cliente

        public static List<Model.Cliente> ListaClientes()
        {
            
            
            StdBELista objList;

            List<Model.Cliente> listClientes = new List<Model.Cliente>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, Moeda, NumContrib as NumContribuinte, Fac_Mor AS campo_exemplo FROM  CLIENTES order by Nome");

                
                while (!objList.NoFim())
                {
                    listClientes.Add(new Model.Cliente
                    {
                        CodCliente = objList.Valor("Cliente"),
                        NomeCliente = objList.Valor("Nome"),
                        Moeda = objList.Valor("Moeda"),
                        NumContribuinte = objList.Valor("NumContribuinte"),
                        Morada = objList.Valor("campo_exemplo")
                    });
                    objList.Seguinte();

                }

                return listClientes;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
        {
            

            GcpBECliente objCli = new GcpBECliente();


            Model.Cliente myCli = new Model.Cliente();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
                    myCli.CodCliente = objCli.get_Cliente();
                    myCli.NomeCliente = objCli.get_Nome();
                    myCli.Moeda = objCli.get_Moeda();
                    myCli.NumContribuinte = objCli.get_NumContribuinte();
                    myCli.Morada = objCli.get_Morada();
                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static List<Model.Cliente> GetTopClientes(int numClientes)
        {

            StdBELista objList;

            Model.Cliente art = new Model.Cliente();
            List<Model.Cliente> listArts = new List<Model.Cliente>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Consulta("SELECT TOP " + numArtigos + " descricao, total FROM Artigo JOIN (SELECT Artigo.Artigo, SUM(PrecoLiquido) AS total FROM linhasDoc JOIN artigo ON linhasDoc.Artigo = Artigo.Artigo JOIN cabecDoc ON linhasDoc.IdCabecDoc = cabecDoc.Id WHERE cabecDoc.tipoDoc != 'ECL' AND cabecDoc.tipoDoc != 'GR' AND linhasDoc.Artigo != 'NULL' GROUP BY Artigo.Artigo) t1 ON Artigo.Artigo = t1.Artigo ORDER BY total DESC");
                objList = PriEngine.Engine.Consulta("select TOP " + numClientes + " Entidade, cli.Nome, sum(TotalMerc-TotalDesc) as total from cabecDoc as c, Clientes as cli WHERE c.tipoDoc != 'ECL' and c.tipoDoc != 'GR' AND c.Entidade = cli.Cliente group by entidade,cli.Nome order by total desc");
                while (!objList.NoFim())
                {
                    art = new Model.Cliente();
                    art.CodCliente = objList.Valor("Entidade");
                    art.NomeCliente = objList.Valor("Nome");
                    art.TotalGasto = objList.Valor("total");

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }
        }

        public static List<Model.Cliente> GetTopClientesYear(int numClientes, int year)
        {
            StdBELista objList;

            Model.Cliente art = new Model.Cliente();
            List<Model.Cliente> listArts = new List<Model.Cliente>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if(year == 0)
                    objList = PriEngine.Engine.Consulta("select TOP " + numClientes + " Entidade, cli.Nome, sum(TotalMerc-TotalDesc) as total from cabecDoc as c, Clientes as cli WHERE (c.tipoDoc = 'FA' OR c.tipoDoc = 'NC' OR c.tipoDoc = 'VD') AND c.Entidade = cli.Cliente group by entidade,cli.Nome order by total desc");
                else
                    objList = PriEngine.Engine.Consulta("select TOP " + numClientes + " Entidade, cli.Nome, sum(TotalMerc-TotalDesc) as total from cabecDoc as c, Clientes as cli WHERE (c.tipoDoc = 'FA' OR c.tipoDoc = 'NC' OR c.tipoDoc = 'VD') AND c.Entidade = cli.Cliente AND DATEPART(yyyy, c.Data) = '" + year + "' group by entidade,cli.Nome order by total desc");

                while (!objList.NoFim())
                {
                    art = new Model.Cliente();
                    art.CodCliente = objList.Valor("Entidade");
                    art.NomeCliente = objList.Valor("Nome");
                    art.TotalGasto = objList.Valor("total");

                    listArts.Add(art);
                    objList.Seguinte();
                }
                return listArts;
            }
            else
            {
                return null;
            }
        }

        public static Lib_Primavera.Model.RespostaErro UpdCliente(Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
           

            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.CodCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.CodCliente);
                        objCli.set_EmModoEdicao(true);

                        objCli.set_Nome(cliente.NomeCliente);
                        objCli.set_NumContribuinte(cliente.NumContribuinte);
                        objCli.set_Moeda(cliente.Moeda);
                        objCli.set_Morada(cliente.Morada);

                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }


        public static Lib_Primavera.Model.RespostaErro DelCliente(string codCliente)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Clientes.Remove(codCliente);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }



        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            

            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {

                    myCli.set_Cliente(cli.CodCliente);
                    myCli.set_Nome(cli.NomeCliente);
                    myCli.set_NumContribuinte(cli.NumContribuinte);
                    myCli.set_Moeda(cli.Moeda);
                    myCli.set_Morada(cli.Morada);

                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }

       

        #endregion Cliente;   // -----------------------------  END   CLIENTE    -----------------------

        #region Fornecedor

        public static List<Model.Fornecedor> ListaFornecedores()
        {


            StdBELista objList;

            List<Model.Fornecedor> listFornecedores = new List<Model.Fornecedor>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT Fornecedor, Nome, Moeda, NumContrib as NumContribuinte, EnderecoWeb AS website FROM  Fornecedores");


                while (!objList.NoFim())
                {
                    listFornecedores.Add(new Model.Fornecedor
                    {
                        CodFornecedor = objList.Valor("Fornecedor"),
                        NomeFornecedor = objList.Valor("Nome"),
                        Moeda = objList.Valor("Moeda"),
                        NumContribuinte = objList.Valor("NumContribuinte"),
                        EnderecoWeb = objList.Valor("website")
                    });
                    objList.Seguinte();

                }

                return listFornecedores;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.Fornecedor GetFornecedor(string codFornecedor)
        {


            GcpBEFornecedor objFor = new GcpBEFornecedor();


            Model.Fornecedor myFor = new Model.Fornecedor();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Fornecedores.Existe(codFornecedor) == true)
                {
                    objFor = PriEngine.Engine.Comercial.Fornecedores.Edita(codFornecedor);
                    myFor.CodFornecedor = objFor.get_Fornecedor();
                    myFor.NomeFornecedor = objFor.get_Nome();
                    myFor.Moeda = objFor.get_Moeda();
                    myFor.NumContribuinte = objFor.get_NumContribuinte();
                    myFor.EnderecoWeb = objFor.get_EnderecoWeb();
                    return myFor;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static List<Model.Fornecedor> GetTopFornecedoresYear(int numFornecedores, int year)
        {
            StdBELista objList;

            Model.Fornecedor art = new Model.Fornecedor();
            List<Model.Fornecedor> listArts = new List<Model.Fornecedor>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Consulta("SELECT TOP " + numArtigos + " descricao, total FROM Artigo JOIN (SELECT Artigo.Artigo, SUM(PrecoLiquido) AS total FROM linhasDoc JOIN artigo ON linhasDoc.Artigo = Artigo.Artigo JOIN cabecDoc ON linhasDoc.IdCabecDoc = cabecDoc.Id WHERE cabecDoc.tipoDoc != 'ECL' AND cabecDoc.tipoDoc != 'GR' AND linhasDoc.Artigo != 'NULL' GROUP BY Artigo.Artigo) t1 ON Artigo.Artigo = t1.Artigo ORDER BY total DESC");
                if(year == 0)
                    objList = PriEngine.Engine.Consulta("select TOP " + numFornecedores + " Nome, sum(TotalMerc) as total from cabecCompras group by Nome order by total asc");
                else
                    objList = PriEngine.Engine.Consulta("select TOP " + numFornecedores + " Nome, sum(TotalMerc) as total from cabecCompras WHERE DATEPART(yyyy, DataDoc) = '" + year + "' group by Nome order by total asc");
                
                while (!objList.NoFim())
                {
                    art = new Model.Fornecedor();
                    art.NomeFornecedor = objList.Valor("Nome");
                    art.TotalMerc = Math.Abs(objList.Valor("total"));
                    
                    listArts.Add(art);
                    objList.Seguinte();
                }
                return listArts;
            }
            else
            {
                return null;
            }
        }

        #endregion Fornecedor
        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {

            StdBELista objArtigo;
            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigo = PriEngine.Engine.Consulta("select Artigo.Artigo as id, Artigo.Descricao as descricao, Artigo.CodBarras, Artigo.Iva, Artigo.PCMedio, Artigo.DataUltEntrada, Artigo.DataUltSaida, Artigo.STKactual as stock, Familias.Descricao as categoria, ArtigoMoeda.PVP1, ArtigoMoeda.PVP2, ArtigoMoeda.PVP3, ArtigoMoeda.PVP4, ArtigoMoeda.PVP5, ArtigoMoeda.PVP6 from Artigo JOIN Familias on Artigo.Familia = Familias.Familia JOIN ArtigoMoeda on ArtigoMoeda.Artigo = Artigo.Artigo where Artigo.Artigo = '"+ codArtigo +"'");
                    myArt.CodArtigo = objArtigo.Valor("id");
                    myArt.DescArtigo = objArtigo.Valor("descricao");
                    myArt.Categoria = objArtigo.Valor("categoria");
                    myArt.Stock = (int)objArtigo.Valor("stock");
                    myArt.Preco = objArtigo.Valor("PCMedio");
                    myArt.PVP1 = objArtigo.Valor("PVP1");
                    myArt.PVP2 = objArtigo.Valor("PVP2");
                    myArt.PVP3 = objArtigo.Valor("PVP3");
                    myArt.PVP4 = objArtigo.Valor("PVP4");
                    myArt.PVP5 = objArtigo.Valor("PVP5");
                    myArt.PVP6 = objArtigo.Valor("PVP6");
                    myArt.CodBarras = objArtigo.Valor("CodBarras");
                    myArt.Iva = Convert.ToDouble(objArtigo.Valor("Iva"));
                    myArt.DataUltEntrada = Convert.ToDateTime(objArtigo.Valor("DataUltEntrada"));
                    myArt.DataUltSaida = Convert.ToDateTime(objArtigo.Valor("DataUltSaida"));
                    return myArt;
                }
                
            }
            else
            {
                return null;
            }

        }

        public static List<Model.Artigo> GetTopArtigos(int numArtigos)
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("select top " + numArtigos + " a.Artigo ,a.descricao, SUM(PrecoLiquido) as total from linhasDoc as l, artigo as a, cabecDoc as c WHERE l.Artigo = a.Artigo AND l.IdCabecDoc = c.Id AND (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') and l.Artigo != 'NULL' group by a.artigo,a.descricao order by total DESC");
                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("Artigo");
                    art.TotalVendas = objList.Valor("total");
                    art.DescArtigo = objList.Valor("descricao");

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }
        }

        public static List<Model.Artigo> GetTopArtigosYear(int numArtigos, int year)
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if(year == 0)
                    objList = PriEngine.Engine.Consulta("select top " + numArtigos + " a.Artigo ,a.descricao, SUM(PrecoLiquido) as total, SUM(l.quantidade) AS qtd, MIN(a.STKActual) as stock from linhasDoc as l, artigo as a, cabecDoc as c WHERE l.Artigo = a.Artigo AND l.IdCabecDoc = c.Id AND (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') and l.Artigo != 'NULL' group by a.artigo,a.descricao order by total DESC");
                else
                    objList = PriEngine.Engine.Consulta("select top " + numArtigos + " a.Artigo ,a.descricao, SUM(PrecoLiquido) as total, SUM(l.quantidade) AS qtd, MIN(a.STKActual) as stock from linhasDoc as l, artigo as a, cabecDoc as c WHERE l.Artigo = a.Artigo AND l.IdCabecDoc = c.Id AND (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') and l.Artigo != 'NULL' AND DATEPART(yyyy, c.Data) = '" + year + "' group by a.artigo,a.descricao order by total DESC");
                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("Artigo");
                    art.TotalVendas = objList.Valor("total");
                    art.DescArtigo = objList.Valor("descricao");
                    art.Quantidade = (int)objList.Valor("qtd");
                    art.Stock = (int)objList.Valor("stock");
                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }
        }

        public static List<Model.Artigo> GetTopQtdArtigosYear(int numArtigos, int year)
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if(year == 0)
                    objList = PriEngine.Engine.Consulta("select top " + numArtigos + " a.Artigo ,a.descricao, SUM(PrecoLiquido) as total, SUM(l.quantidade) AS qtd, MIN(a.STKActual) as stock from linhasDoc as l, artigo as a, cabecDoc as c WHERE l.Artigo = a.Artigo AND l.IdCabecDoc = c.Id AND (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') and l.Artigo != 'NULL' group by a.artigo,a.descricao order by qtd DESC");
                else
                    objList = PriEngine.Engine.Consulta("select top " + numArtigos + " a.Artigo ,a.descricao, SUM(PrecoLiquido) as total, SUM(l.quantidade) AS qtd, MIN(a.STKActual) as stock from linhasDoc as l, artigo as a, cabecDoc as c WHERE l.Artigo = a.Artigo AND l.IdCabecDoc = c.Id AND (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') and l.Artigo != 'NULL' AND DATEPART(yyyy, c.Data) = '" + year + "' group by a.artigo,a.descricao order by qtd DESC");
                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("Artigo");
                    art.TotalVendas = objList.Valor("total");
                    art.DescArtigo = objList.Valor("descricao");
                    art.Quantidade = (int)objList.Valor("qtd");
                    art.Stock = (int)objList.Valor("stock");
                    if (art.Stock < 0)
                    {
                        art.Stock = 0;
                    }
                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }
        }

        public static List<Model.Artigo> GetTopArtigosCompradosYear(int numArtigos, int year)
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if(year == 0)
                    objList = PriEngine.Engine.Consulta("select top " + numArtigos + " a.Artigo ,a.descricao, SUM(PrecoLiquido) as total, SUM(l.quantidade) AS qtd, MIN(a.STKActual) as stock from LinhasCompras as l, artigo as a, CabecCompras as c WHERE l.Artigo = a.Artigo AND l.IdCabecCompras = c.Id AND (Tipodoc = 'VFA' OR TipoDoc = 'VNC' OR TipoDoc = 'VVD') and l.Artigo != 'NULL' group by a.artigo,a.descricao order by qtd asc");
                else
                    objList = PriEngine.Engine.Consulta("select top " + numArtigos + " a.Artigo ,a.descricao, SUM(PrecoLiquido) as total, SUM(l.quantidade) AS qtd, MIN(a.STKActual) as stock from LinhasCompras as l, artigo as a, CabecCompras as c WHERE l.Artigo = a.Artigo AND l.IdCabecCompras = c.Id AND (Tipodoc = 'VFA' OR TipoDoc = 'VNC' OR TipoDoc = 'VVD') and l.Artigo != 'NULL' AND DATEPART(yyyy, c.DataDoc) = '" + year + "' group by a.artigo,a.descricao order by qtd asc");
                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("Artigo");
                    art.TotalVendas = objList.Valor("total");
                    art.DescArtigo = objList.Valor("descricao");
                    art.Quantidade = (int)Math.Abs(objList.Valor("qtd"));
                    art.Stock = (int)objList.Valor("stock");
                    if (art.Stock < 0)
                    {
                        art.Stock = 0;
                    }
                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }
        }

        public static List<Model.Artigo> GetCatArtigosYear(int year)
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if(year == 0)
                    objList = PriEngine.Engine.Consulta("SELECT SUM(quantidade) AS qtd, Artigo.Familia, Familias.Descricao FROM Artigo FULL OUTER JOIN LinhasDoc ON LinhASDoc.Artigo = Artigo.Artigo FULL OUTER JOIN cabecdoc ON IdCabecDoc = cabecdoc.id JOIN Familias ON Artigo.Familia = Familias.Familia WHERE Artigo.Artigo IS NOT NULL AND (cabecdoc.Tipodoc = 'FA' OR cabecdoc.TipoDoc = 'NC' OR cabecdoc.TipoDoc = 'VD') GROUP BY Artigo.Familia, Familias.Descricao ORDER BY qtd DESC");
                else
                    objList = PriEngine.Engine.Consulta("SELECT SUM(quantidade) AS qtd, Artigo.Familia, Familias.Descricao FROM Artigo FULL OUTER JOIN LinhasDoc ON LinhASDoc.Artigo = Artigo.Artigo FULL OUTER JOIN cabecdoc ON IdCabecDoc = cabecdoc.id JOIN Familias ON Artigo.Familia = Familias.Familia WHERE Artigo.Artigo IS NOT NULL AND (cabecdoc.Tipodoc = 'FA' OR cabecdoc.TipoDoc = 'NC' OR cabecdoc.TipoDoc = 'VD') AND DATEPART(yyyy, cabecdoc.Data) = '" + year + "' GROUP BY Artigo.Familia, Familias.Descricao ORDER BY qtd DESC");
                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.Categoria = objList.Valor("Descricao");
                    art.TotalVendas = objList.Valor("qtd");
                    if (art.TotalVendas < 0)
                    {
                        objList.Seguinte();
                    }
                    else
                    {
                        listArts.Add(art);
                        objList.Seguinte();
                    }
                }

                return listArts;

            }
            else
            {
                return null;

            }
        }

        public static List<Model.Artigo> GetStockArtigos(int numArtigos, string order)
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("SELECT TOP " + numArtigos + " Artigo, Descricao, STKActual FROM Artigo ORDER BY STKActual " + order);
                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.DescArtigo = objList.Valor("Descricao");
                    art.CodArtigo = objList.Valor("Artigo");
                    art.Stock = (int)objList.Valor("STKActual");
                    listArts.Add(art);
                    objList.Seguinte();
                    }

                return listArts;

            }
            else
            {
                return null;

            }
        }

        public static List<Model.Artigo> ListaArtigos()
        {
                        
            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("select artigo.artigo as id, artigo.descricao, artigo.stkactual as stock, familias.descricao as categoria, artigo.PCMedio as preco from artigo full outer join familias on artigo.Familia = Familias.Familia where artigo IS NOT NULL");

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.Preco = objList.Valor("preco");
                    art.Categoria = objList.Valor("categoria");
                    art.Stock = (int)objList.Valor("stock");
                    art.CodArtigo = objList.Valor("id");
                    art.DescArtigo = objList.Valor("descricao");

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }

        }

        public static Lib_Primavera.Model.RespostaErro InsereArtigoObj(Model.Artigo art)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBEArtigo myArt = new GcpBEArtigo();
            try
            {
                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {

                    myArt.set_Artigo(art.CodArtigo);
                    myArt.set_Descricao(art.DescArtigo);

                    PriEngine.Engine.Comercial.Artigos.Actualiza(myArt);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }

        public static Lib_Primavera.Model.RespostaErro UpdArtigo(Lib_Primavera.Model.Artigo artigo)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBEArtigo objArt = new GcpBEArtigo();

            try
            {

                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Artigos.Existe(artigo.CodArtigo) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        objArt = PriEngine.Engine.Comercial.Artigos.Edita(artigo.CodArtigo);
                        objArt.set_EmModoEdicao(true);

                        objArt.set_Descricao(artigo.DescArtigo);

                        PriEngine.Engine.Comercial.Artigos.Actualiza(objArt);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }
        public static Lib_Primavera.Model.RespostaErro DelArtigo(string CodArtigo)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEArtigo objArt = new GcpBEArtigo();


            try
            {

                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Artigos.Existe(CodArtigo) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Artigos.Remove(CodArtigo);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        #endregion Artigo

   

        #region DocCompra
        

        public static List<Model.DocCompra> Compras_List()
        {
                
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, Tipodoc, TotalMerc, Serie From CabecCompras");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.Tipodoc = objListCab.Valor("Tipodoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecCompras, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido, Armazem, Lote from LinhasCompras where IdCabecCompras='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecCompras");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindc.Armazem = objListLin.Valor("Armazem");
                        lindc.Lote = objListLin.Valor("Lote");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;
                    
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }
                
        public static Model.RespostaErro Compra_New(Model.DocCompra dc)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            

            GcpBEDocumentoCompra myCompra = new GcpBEDocumentoCompra();
            GcpBELinhaDocumentoCompra myLin = new GcpBELinhaDocumentoCompra();
            GcpBELinhasDocumentoCompra myLinhas = new GcpBELinhasDocumentoCompra();

            PreencheRelacaoCompras rl = new PreencheRelacaoCompras();
            List<Model.LinhaDocCompra> lstlindc = new List<Model.LinhaDocCompra>();

            try
            {
                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    myCompra.set_DataDoc(dc.Data);
                    myCompra.set_Entidade(dc.Entidade);
                    myCompra.set_NumDocExterno(dc.NumDocExterno);
                    myCompra.set_Serie(dc.Serie);
                    myCompra.set_Tipodoc(dc.Tipodoc);
                    // Linhas do documento para a lista de linhas
                    lstlindc = dc.LinhasDoc;
                    PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(myCompra, rl);
                    foreach (Model.LinhaDocCompra lin in lstlindc)
                    {
                        PriEngine.Engine.Comercial.Compras.AdicionaLinha(myCompra, lin.CodArtigo, lin.Quantidade, lin.Armazem, "", lin.PrecoUnitario, lin.Desconto);
                    }

                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Compras.Actualiza(myCompra, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static float Compras_Total()
        {

            StdBELista objListCab;
            Model.DocCompra dv = new Model.DocCompra();
            float totalC = 0;
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecCompras WHERE (Tipodoc = 'VFA' OR Tipodoc = 'VFP' OR TipoDoc = 'VFR' OR TipoDoc = 'VNC' OR TipoDoc = 'VGR' OR TipoDoc = 'VVD')");
                while (!objListCab.NoFim())
                {
                    totalC += objListCab.Valor("total");
                    objListCab.Seguinte();
                }
            }
            return totalC;
        }

        public static float ComprasTotalAbsYear(int year)
        {

            StdBELista objListCab;
            Model.DocCompra dv = new Model.DocCompra();
            float totalC = 0;
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if(year == 0)
                    objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecCompras WHERE (Tipodoc = 'VFA' OR Tipodoc = 'VFP' OR TipoDoc = 'VFR' OR TipoDoc = 'VNC' OR TipoDoc = 'VGR' OR TipoDoc = 'VVD')");
                else
                    objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecCompras WHERE (Tipodoc = 'VFA' OR Tipodoc = 'VFP' OR TipoDoc = 'VFR' OR TipoDoc = 'VNC' OR TipoDoc = 'VGR' OR TipoDoc = 'VVD') AND DATEPART(yyyy, DataDoc) = '" + year + "'"); 
                
                while (!objListCab.NoFim())
                {
                    totalC += objListCab.Valor("total");
                    objListCab.Seguinte();
                }
            }
            return totalC;
        }

        public static float Compras_Total_per(String start, String finish)
        {

            StdBELista objListCab;
            Model.DocCompra dv = new Model.DocCompra();
            float totalC = 0;
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecCompras WHERE (Tipodoc = 'VFA' OR Tipodoc = 'VFP' OR TipoDoc = 'VFR' OR TipoDoc = 'VNC' OR TipoDoc = 'VGR' OR TipoDoc = 'VVD') AND DataDoc between '" + start + "' and'" + finish + "'");
                while (!objListCab.NoFim())
                {
                    totalC += objListCab.Valor("total");
                    objListCab.Seguinte();
                }
            }
            return totalC;
        }

        public static Model.DocCompra Compra_Get(string numdoc)
        {
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {


                string st = "SELECT id, Entidade, NumDocExterno, Tipodoc, DataDoc, NumDoc, TotalMerc, Serie From CabecCompras where NumDoc='" + numdoc + "'";
                objListCab = PriEngine.Engine.Consulta(st);
                dc = new Model.DocCompra();
                dc.id = objListCab.Valor("id");
                dc.Entidade = objListCab.Valor("Entidade");
                dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                dc.NumDoc = objListCab.Valor("NumDoc");
                dc.Data = objListCab.Valor("DataDoc");
                dc.Tipodoc = objListCab.Valor("Tipodoc");
                dc.TotalMerc = objListCab.Valor("TotalMerc");
                dc.Serie = objListCab.Valor("Serie");
                objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dc.id + "' order By NumLinha");
                listlindc = new List<Model.LinhaDocCompra>();

                while (!objListLin.NoFim())
                {
                    lindc = new Model.LinhaDocCompra();
                    lindc.IdCabecDoc = objListLin.Valor("idCabecDoc");
                    lindc.CodArtigo = objListLin.Valor("Artigo");
                    lindc.DescArtigo = objListLin.Valor("Descricao");
                    lindc.Quantidade = objListLin.Valor("Quantidade");
                    lindc.Unidade = objListLin.Valor("Unidade");
                    lindc.Desconto = objListLin.Valor("Desconto1");
                    lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                    lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                    lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");
                    listlindc.Add(lindc);
                    objListLin.Seguinte();
                }

                dc.LinhasDoc = listlindc;
                return dc;
            }
            return null;
        }

        public static List<Model.DocCompra> Compras_List_Months(int month)
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                DateTime today = DateTime.Now;
                DateTime monthsBack = today.AddMonths(0 - month);
                string newdate = monthsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, Tipodoc, TotalMerc, Serie From CabecCompras WHERE DataDoc >= '" + newdate + "'");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Tipodoc = objListCab.Valor("Tipodoc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }

        public static List<Model.DocCompra> Compras_List_Years(int years)
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                DateTime today = DateTime.Now;
                DateTime yearsBack = today.AddYears(0 - years);
                string newdate = yearsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, Tipodoc, TotalMerc, Serie From CabecCompras WHERE DataDoc >= '" + newdate + "'");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Tipodoc = objListCab.Valor("Tipodoc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }

        public static Object Compras_List_Years2(int years)
        {

            StdBELista objListCab;
            Model.Month dc = new Model.Month();
            List<Model.Month> listdc = new List<Model.Month>();
/*
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                DateTime today = DateTime.Now;
                DateTime yearsBack = today.AddYears(0 - years);
                string newdate = yearsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");
                objListCab = PriEngine.Engine.Consulta("select CAST(YEAR(DataVencimento) AS VARCHAR(4)) + '-' + CAST(MONTH(DataVencimento) AS VARCHAR(2)) as data, sum(TotalMerc-TotalDesc) as total, sum(TotalDesc) as ds from cabecCompras Where (Tipodoc = 'VFA' OR Tipodoc = 'VFP' OR TipoDoc = 'VFR' OR TipoDoc = 'VNC' OR TipoDoc = 'VGR' OR TipoDoc = 'VVD') and DataVencimento >= '" + newdate + "' group by CAST(YEAR(DataVencimento) AS VARCHAR(4)), CAST(MONTH(DataVencimento) AS VARCHAR(2)) order by CAST(YEAR(DataVencimento) AS VARCHAR(4)), right(100+CAST(MONTH(DataVencimento) AS VARCHAR(2)),2) asc");
                while (!objListCab.NoFim())
                {
                    DateTime retrievedData = Convert.ToDateTime(objListCab.Valor("data"));

                    dc = new Model.Month();
                    dc.Num = retrievedData.Month;
                    dc.Total = objListCab.Valor("total");
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }*/
            return listdc;
        }
        
        public static Object Compras_List_Year(int year)
        {

            StdBELista objListCab;
            Model.Month dc = new Model.Month();
            List<Model.Month> listdc = new List<Model.Month>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if (year == 0){
                    for (int y = 2013; y < 2016; y++)
                    {
                        for (int i = 1; i < 13; i++)
                        {
                            dc = new Model.Month();
                            if (i < 10)
                            {
                                dc.Num = String.Format(y + "-0" + i);
                            }
                            else
                            {
                                dc.Num = String.Format(y + "-" + i);
                            }

                            objListCab = PriEngine.Engine.Consulta("SELECT TotalMerc as total From CabecCompras WHERE (Tipodoc = 'VFA' OR TipoDoc = 'VNC' OR TipoDoc = 'VVD') AND DATEPART(yyyy, DataDoc) = '" + y + "' AND DATEPART(mm, DataDoc) = '" + i + "'");

                            while (!objListCab.NoFim())
                            {
                                dc.Total += objListCab.Valor("total");
                                objListCab.Seguinte();
                            }

                            listdc.Add(dc);
                        }
                    }
                }
                else{
                    for (int i = 1; i < 13; i++)
                    {
                        dc = new Model.Month();
                        if (i < 10)
                        {
                            dc.Num = String.Format(year + "-0" + i);
                        }
                        else
                        {
                            dc.Num = String.Format(year + "-" + i);
                        }

                        objListCab = PriEngine.Engine.Consulta("SELECT TotalMerc as total From CabecCompras WHERE (Tipodoc = 'VFA' OR TipoDoc = 'VNC' OR TipoDoc = 'VVD') AND DATEPART(yyyy, DataDoc) = '" + year + "' AND DATEPART(mm, DataDoc) = '" + i + "'");
                        while (!objListCab.NoFim())
                        {
                            dc.Total += objListCab.Valor("total");
                            objListCab.Seguinte();
                        }

                        listdc.Add(dc);
                    }
                }
            }
            return listdc;
        }

        public static List<Model.DocCompra> Compras_List_period(string start, string finish)
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                /*DateTime today = DateTime.Now;
                DateTime sixMonthsBack = today.AddMonths(0 - month);
                string newdate = sixMonthsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");*/
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, Tipodoc, TotalMerc, Serie From CabecCompras WHERE DataDoc between '" + start + "' and'" + finish + "'");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Tipodoc = objListCab.Valor("Tipodoc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }

       
        #endregion DocCompra


        #region DocsVenda

        public static Model.RespostaErro Vendas_New(Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();
             
            GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();
             
            PreencheRelacaoVendas rl = new PreencheRelacaoVendas();
            List<Model.LinhaDocVenda> lstlindv = new List<Model.LinhaDocVenda>();
            
            try
            {
                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    myEnc.set_DataDoc(dv.Data);
                    myEnc.set_Entidade(dv.Entidade);
                    myEnc.set_Filial(dv.Filial);
                    myEnc.set_Serie(dv.Serie);
                    myEnc.set_Tipodoc(dv.Tipodoc);
                    // Linhas do documento para a lista de linhas
                    lstlindv = dv.LinhasDoc;
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    foreach (Model.LinhaDocVenda lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodArtigo, lin.Quantidade, "", "", lin.PrecoUnitario, lin.Desconto);
                    }


                   // PriEngine.Engine.Comercial.Compras.TransformaDocumento(

                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static List<Model.DocVenda> Vendas_List()
        {
            
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Filial, Tipodoc, Data, NumDoc, TotalMerc, Serie From CabecDoc");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.Filial = objListCab.Valor("Filial");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.Tipodoc = objListCab.Valor("Tipodoc");
                    dv.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }

        public static float Vendas_Total()
        {

            StdBELista objListCab;
            Model.DocVenda dv = new Model.DocVenda();
            float totalV = 0;
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecDoc WHERE (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD')");
                while (!objListCab.NoFim())
                {
                    totalV += objListCab.Valor("total");
                    objListCab.Seguinte();
                }
            }
            return totalV;
        }

        public static float VendasTotalAbsYear(int year)
        {

            StdBELista objListCab;
            Model.DocCompra dv = new Model.DocCompra();
            float totalC = 0;
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if(year == 0)
                    objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecDoc WHERE (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD')");
                else
                    objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecDoc WHERE (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') AND DATEPART(yyyy, Data) = '" + year + "'");
                while (!objListCab.NoFim())
                {
                    totalC += objListCab.Valor("total");
                    objListCab.Seguinte();
                }
            }
            return totalC;
        }

        public static float Vendas_Total_per(string start, string finish)
        {

            StdBELista objListCab;
            Model.DocVenda dv = new Model.DocVenda();
            float totalV = 0;
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecDoc WHERE (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') AND Data between '" + start + "' and'" + finish + "'");
                while (!objListCab.NoFim())
                {
                    totalV += objListCab.Valor("total");
                    objListCab.Seguinte();
                }
            }
            return totalV;
        }

        public static List<Model.DocVenda> Vendas_List(int month)
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                DateTime today = DateTime.Now;
                DateTime monthsBack = today.AddMonths(0 - month);
                string newdate = monthsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Filial, Tipodoc, Data, NumDoc, TotalMerc, Serie From CabecDoc WHERE Data >= '" + newdate + "'");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.Filial = objListCab.Valor("Filial");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.Tipodoc = objListCab.Valor("Tipodoc");
                    dv.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }

        public static List<Model.DocVenda> Vendas_List_Years(int years)
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                DateTime today = DateTime.Now;
                DateTime yearsBack = today.AddYears(0 - years);
                string newdate = yearsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Filial, Tipodoc, Data, NumDoc, TotalMerc, Serie From CabecDoc WHERE Data >= '" + newdate + "'");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.Filial = objListCab.Valor("Filial");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.Tipodoc = objListCab.Valor("Tipodoc");
                    dv.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }

        public static Object Vendas_List_Years2(int years)
        {

            StdBELista objListCab;
            Model.Month dc = new Model.Month();
            List<Model.Month> listdc = new List<Model.Month>();

            /*if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                DateTime today = DateTime.Now;
                DateTime yearsBack = today.AddYears(0 - years);
                string newdate = yearsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");
                objListCab = PriEngine.Engine.Consulta("select CAST(YEAR(DataVencimento) AS VARCHAR(4)) + '-' + CAST(MONTH(DataVencimento) AS VARCHAR(2)) as data, sum(TotalMerc-TotalDesc) as total, sum(TotalDesc) as ds from cabecDoc Where (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') and DataVencimento >= '" + newdate + "' group by CAST(YEAR(DataVencimento) AS VARCHAR(4)), CAST(MONTH(DataVencimento) AS VARCHAR(2)) order by CAST(YEAR(DataVencimento) AS VARCHAR(4)), right(100+CAST(MONTH(DataVencimento) AS VARCHAR(2)),2) asc");
                while (!objListCab.NoFim())
                {
                    DateTime retrievedData = Convert.ToDateTime(objListCab.Valor("data"));

                    dc = new Model.Month();
                    dc.Num = retrievedData.Month;
                    dc.Total = objListCab.Valor("total");
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }*/
            return listdc;
        }

        public static Object Vendas_List_Year(int year)
        {

            StdBELista objListCab;
            Model.Month dv = new Model.Month();
            List<Model.Month> listdv = new List<Model.Month>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if (year == 0)
                {
                    for (int y = 2013; y < 2016; y++)
                    {
                        for (int i = 1; i < 13; i++)
                        {
                            dv = new Model.Month();

                            if (i < 10)
                            {
                                dv.Num = String.Format(y + "-0" + i);
                            }
                            else
                            {
                                dv.Num = String.Format(y + "-" + i);
                            }

                            objListCab = PriEngine.Engine.Consulta("SELECT TotalMerc as total From CabecDoc WHERE (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') AND DATEPART(yyyy, Data) = '" + y + "' AND DATEPART(mm, Data) = '" + i + "'");
                            while (!objListCab.NoFim())
                            {
                                dv.Total += objListCab.Valor("total");
                                objListCab.Seguinte();
                            }
                            listdv.Add(dv);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < 13; i++)
                    {
                        dv = new Model.Month();

                        if (i < 10)
                        {
                            dv.Num = String.Format(year + "-0" + i);
                        }
                        else
                        {
                            dv.Num = String.Format(year + "-" + i);
                        }

                        objListCab = PriEngine.Engine.Consulta("SELECT TotalMerc as total From CabecDoc WHERE (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') AND DATEPART(yyyy, Data) = '" + year + "' AND DATEPART(mm, Data) = '" + i + "'");
                        while (!objListCab.NoFim())
                        {
                            dv.Total += objListCab.Valor("total");
                            objListCab.Seguinte();
                        }
                        listdv.Add(dv);
                    }
                }
            }
            return listdv;
        }

        public static List<Model.DocVenda> Vendas_List_period(string start, string finish)
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                /*DateTime today = DateTime.Now;
                DateTime previousMonth = today.AddMonths(0 - month);
                string beforedate = previousMonth.ToString("yyyy-MM-dd HH:mm:ss.fff");
                DateTime afterMonth = today.AddMonths(0 - (month + 1));
                string afterdate = afterMonth.ToString("yyyy-MM-dd HH:mm:ss.fff");*/
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Filial, Tipodoc, Data, NumDoc, TotalMerc, Serie From CabecDoc WHERE Data between '" + start +"' and'" + finish + "'" );
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.Filial = objListCab.Valor("Filial");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.Tipodoc = objListCab.Valor("Tipodoc");
                    dv.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }

        public static Model.DocVenda Venda_Get(string numdoc)
        {
            
            
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {


                string st = "SELECT id, Entidade, Filial, Tipodoc, Data, NumDoc, TotalMerc, Serie From CabecDoc where NumDoc='" + numdoc + "'";
                objListCab = PriEngine.Engine.Consulta(st);
                dv = new Model.DocVenda();
                dv.id = objListCab.Valor("id");
                dv.Filial = objListCab.Valor("Filial");
                dv.Entidade = objListCab.Valor("Entidade");
                dv.NumDoc = objListCab.Valor("NumDoc");
                dv.Data = objListCab.Valor("Data");
                dv.Tipodoc = objListCab.Valor("Tipodoc");
                dv.TotalMerc = objListCab.Valor("TotalMerc");
                dv.Serie = objListCab.Valor("Serie");
                objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                listlindv = new List<Model.LinhaDocVenda>();

                while (!objListLin.NoFim())
                {
                    lindv = new Model.LinhaDocVenda();
                    lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                    lindv.CodArtigo = objListLin.Valor("Artigo");
                    lindv.DescArtigo = objListLin.Valor("Descricao");
                    lindv.Quantidade = objListLin.Valor("Quantidade");
                    lindv.Unidade = objListLin.Valor("Unidade");
                    lindv.Desconto = objListLin.Valor("Desconto1");
                    lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                    lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                    lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                    listlindv.Add(lindv);
                    objListLin.Seguinte();
                }

                dv.LinhasDoc = listlindv;
                return dv;
            }
            return null;
        }

        public static Lib_Primavera.Model.RespostaErro UpdDocVenda(Lib_Primavera.Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBEDocumentoVenda objDocV = new GcpBEDocumentoVenda();

            try
            {

                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Vendas.Existe(dv.Filial, dv.Tipodoc, dv.Serie, dv.NumDoc) == false)
                    {
                        erro.Erro = 1;
                        System.Diagnostics.Debug.WriteLine("FODADSSE  " + dv.Filial + "  " + dv.Tipodoc + "  " + dv.Serie + "  " + dv.NumDoc);
                        erro.Descricao = "O documento de venda não existe";
                        return erro;
                    }
                    else
                    {

                        objDocV = PriEngine.Engine.Comercial.Vendas.Edita(dv.Filial, dv.Tipodoc, dv.Serie, dv.NumDoc);

                        objDocV.set_EmModoEdicao(true);

                        objDocV.set_Entidade(dv.Entidade);
                        objDocV.set_TotalMerc(dv.TotalMerc);
                        objDocV.set_DataDoc(dv.Data);
                        objDocV.set_ID(dv.id);

                        List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();
                        listlindv = dv.LinhasDoc;
                        GcpBELinhasDocumentoVenda linhas = new GcpBELinhasDocumentoVenda();
                        for(int i = 0; i < listlindv.Count; i++){
                            GcpBELinhaDocumentoVenda linha = new GcpBELinhaDocumentoVenda(listlindv.ElementAt(i));
                            linhas.Insere(linha);
                        }

                        objDocV.set_Linhas(linhas);


                        PriEngine.Engine.Comercial.Vendas.Actualiza(objDocV);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        public static Lib_Primavera.Model.RespostaErro DelDocVenda(string id)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Vendas.ExisteID(id) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O documento de venda não existe";
                        return erro;
                    }
                    else
                    {
                        Lib_Primavera.Model.DocVenda tempdoc = Venda_Get(id);
                        PriEngine.Engine.Comercial.Vendas.Remove(tempdoc.Filial, tempdoc.Tipodoc, tempdoc.Serie, tempdoc.NumDoc);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        #endregion DocsVenda

        public static IEnumerable<Model.Lucro> GetLucro(int year)
        {
            //NumberStyles style;
            //CultureInfo culture;
            //style = NumberStyles.AllowDecimalPoint;
            //culture = CultureInfo.CreateSpecificCulture("fr-FR");
            StdBELista objListVendas;
            StdBELista objListCompras;
            List<Model.Lucro> listlucro = new List<Model.Lucro>();
            double totalV = 0;
            double totalC = 0;
            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {
                if(year == 0)
                {
                    for (int y = 2013; y < 2016; y++)
                    {
                        for (int i = 1; i < 13; i++)
                        {
                            totalC = 0;
                            totalV = 0;
                            Model.Lucro dl = new Model.Lucro();

                            if (i < 10)
                            {
                                dl.month = String.Format(y + "-0" + i);
                            }
                            else
                            {
                                dl.month = String.Format(y + "-" + i);
                            }

                            objListVendas = PriEngine.Engine.Consulta("SELECT TotalMerc as total From CabecDoc WHERE (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') AND DATEPART(yyyy, Data) = '" + y + "' AND DATEPART(mm, Data) = '" + i + "'");
                            while (!objListVendas.NoFim())
                            {
                                if (objListVendas.Valor("total") > 0)
                                    totalV += objListVendas.Valor("total");
                                objListVendas.Seguinte();
                            }
                            dl.vendas = Math.Round(totalV, 2);
                            objListCompras = PriEngine.Engine.Consulta("SELECT TotalMerc as total From CabecCompras WHERE (Tipodoc = 'VFA' OR TipoDoc = 'VNC' OR TipoDoc = 'VVD') AND DATEPART(yyyy, DataDoc) = '" + y + "' AND DATEPART(mm, DataDoc) = '" + i + "'");
                            while (!objListCompras.NoFim())
                            {
                                totalC += objListCompras.Valor("total");
                                objListCompras.Seguinte();
                            }
                            dl.compras = Math.Round(totalC, 2);
                            if (dl.compras > 0)
                                dl.lucro = Math.Round(dl.vendas - dl.compras, 2);
                            else
                                dl.lucro = dl.vendas + dl.compras;
                            listlucro.Add(dl);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < 13; i++)
                    {
                        totalC = 0;
                        totalV = 0;
                        Model.Lucro dl = new Model.Lucro();

                        if (i < 10)
                        {
                            dl.month = String.Format(year + "-0" + i);
                        }
                        else
                        {
                            dl.month = String.Format(year + "-" + i);
                        }

                        objListVendas = PriEngine.Engine.Consulta("SELECT TotalMerc as total From CabecDoc WHERE (Tipodoc = 'FA' OR TipoDoc = 'NC' OR TipoDoc = 'VD') AND DATEPART(yyyy, Data) = '" + year + "' AND DATEPART(mm, Data) = '" + i + "'");
                        while (!objListVendas.NoFim())
                        {
                            if (objListVendas.Valor("total") > 0)
                                totalV += objListVendas.Valor("total");
                            objListVendas.Seguinte();
                        }
                        dl.vendas = Math.Round(totalV, 2);
                        objListCompras = PriEngine.Engine.Consulta("SELECT TotalMerc as total From CabecCompras WHERE (Tipodoc = 'VFA' OR TipoDoc = 'VNC' OR TipoDoc = 'VVD') AND DATEPART(yyyy, DataDoc) = '" + year + "' AND DATEPART(mm, DataDoc) = '" + i + "'");
                        while (!objListCompras.NoFim())
                        {
                            totalC += objListCompras.Valor("total");
                            objListCompras.Seguinte();
                        }
                        dl.compras = Math.Round(totalC, 2);
                        if (dl.compras > 0)
                            dl.lucro = Math.Round(dl.vendas - dl.compras, 2);
                        else
                            dl.lucro = dl.vendas + dl.compras;
                        listlucro.Add(dl);
                    }
                }
            }
            return listlucro;
        }

        public static float GetContaTotal(String tipo, int year){
            

                StdBELista objListCab;
                Model.Conta dv = new Model.Conta();
                float totalC = 0;
                if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if(tipo.Equals("receber")){
                        if (year == 0)
                            objListCab = PriEngine.Engine.Consulta("SELECT Ano,Mes01CR-Mes02CR-Mes03CR-Mes00CR-Mes01CR-Mes02CR-Mes03CR-Mes04CR-Mes05CR-Mes06CR-Mes07CR-Mes08CR-Mes09CR-Mes10CR-Mes11CR-Mes12CR+Mes01DB+Mes02DB+Mes03DB+Mes04DB+Mes05DB+Mes06DB+Mes07DB+Mes08DB+Mes09DB+Mes10DB+Mes11DB+Mes12DB as valor FROM AcumuladosContas where conta = '21'");
                        else
                            objListCab = PriEngine.Engine.Consulta("SELECT Ano,Mes01CR-Mes02CR-Mes03CR-Mes00CR-Mes01CR-Mes02CR-Mes03CR-Mes04CR-Mes05CR-Mes06CR-Mes07CR-Mes08CR-Mes09CR-Mes10CR-Mes11CR-Mes12CR+Mes01DB+Mes02DB+Mes03DB+Mes04DB+Mes05DB+Mes06DB+Mes07DB+Mes08DB+Mes09DB+Mes10DB+Mes11DB+Mes12DB as valor FROM AcumuladosContas where conta = '21' AND ano = '" + year + "'");

                        while (!objListCab.NoFim())
                        {
                            totalC += (float)objListCab.Valor("valor");
                            objListCab.Seguinte();
                        }
                    }
                    else {
                        if (year == 0)
                            objListCab = PriEngine.Engine.Consulta("SELECT Ano,Mes01CR-Mes02CR-Mes03CR-Mes00CR-Mes01CR-Mes02CR-Mes03CR-Mes04CR-Mes05CR-Mes06CR-Mes07CR-Mes08CR-Mes09CR-Mes10CR-Mes11CR-Mes12CR+Mes01DB+Mes02DB+Mes03DB+Mes04DB+Mes05DB+Mes06DB+Mes07DB+Mes08DB+Mes09DB+Mes10DB+Mes11DB+Mes12DB as valor FROM AcumuladosContas where conta = '22'");
                        else
                            objListCab = PriEngine.Engine.Consulta("SELECT Ano,Mes01CR-Mes02CR-Mes03CR-Mes00CR-Mes01CR-Mes02CR-Mes03CR-Mes04CR-Mes05CR-Mes06CR-Mes07CR-Mes08CR-Mes09CR-Mes10CR-Mes11CR-Mes12CR+Mes01DB+Mes02DB+Mes03DB+Mes04DB+Mes05DB+Mes06DB+Mes07DB+Mes08DB+Mes09DB+Mes10DB+Mes11DB+Mes12DB as valor FROM AcumuladosContas where conta = '22' AND ano = '" + year + "'");

                        while (!objListCab.NoFim())
                        {
                            totalC += (float)objListCab.Valor("valor");
                            objListCab.Seguinte();
                        }
                    }
                }
                return totalC; 
        }
    }
}