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

                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, Moeda, NumContrib as NumContribuinte, Fac_Mor AS campo_exemplo FROM  CLIENTES");

                
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

        #endregion Fornecedor
        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {
            
            GcpBEArtigo objArtigo = new GcpBEArtigo();
            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(CompanyDashboard.Properties.Settings.Default.Company.Trim(), CompanyDashboard.Properties.Settings.Default.User.Trim(), CompanyDashboard.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
                    myArt.CodArtigo = objArtigo.get_Artigo();
                    myArt.DescArtigo = objArtigo.get_Descricao();

                    return myArt;
                }
                
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

                objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("artigo");
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
                objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecCompras WHERE Tipodoc != 'COT' AND Tipodoc != 'PCO'");
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

        public static List<Model.DocCompra> Compras_List(string id, int month)
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
                DateTime sixMonthsBack = today.AddMonths(0 - month);
                string newdate = sixMonthsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");
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
        public static List<Model.DocCompra> Compras_List_month(string id, string month1, string month2)
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
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, Tipodoc, TotalMerc, Serie From CabecCompras WHERE DataDoc between '" + month1 + "' and'" + month2 + "'");
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

        public static List<Model.DocCompra> Compras_List_year(string id, string month1, string month2)
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
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, Tipodoc, TotalMerc, Serie From CabecCompras WHERE DataDoc between '" + month1 + "' and'" + month2 + "'");
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

        public static List<Model.DocCompra> Compras_List_week(string id, string month1, string month2)
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
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, Tipodoc, TotalMerc, Serie From CabecCompras WHERE DataDoc between '" + month1 + "' and'" + month2 + "'");
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
                objListCab = PriEngine.Engine.Consulta("SELECT SUM(TotalMerc) as total From CabecDoc WHERE Tipodoc = 'FA'");
                while (!objListCab.NoFim())
                {
                    totalV += objListCab.Valor("total");
                    objListCab.Seguinte();
                }
            }
            return totalV;
        }

        public static List<Model.DocVenda> Vendas_List(string id, int month)
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
                DateTime sixMonthsBack = today.AddMonths(0 - month);
                string newdate = sixMonthsBack.ToString("yyyy-MM-dd HH:mm:ss.fff");
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

        public static List<Model.DocVenda> Vendas_List_month(string id, string month1, string month2)
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
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Filial, Tipodoc, Data, NumDoc, TotalMerc, Serie From CabecDoc WHERE Data between '" + month1 +"' and'" + month2 + "'" );
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

        public static List<Model.DocVenda> Vendas_List_week(string id, string month1, string month2)
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
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Filial, Tipodoc, Data, NumDoc, TotalMerc, Serie From CabecDoc WHERE Data between '" + month1 + "' and'" + month2 + "'");
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
        public static List<Model.DocVenda> Vendas_List_year(string id, string month1, string month2)
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
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Filial, Tipodoc, Data, NumDoc, TotalMerc, Serie From CabecDoc WHERE Data between '" + month1 + "' and'" + month2 + "'");
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
    }
}