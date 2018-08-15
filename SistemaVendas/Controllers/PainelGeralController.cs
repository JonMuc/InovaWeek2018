using NHibernate;
using SistemaVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using SistemaVendas.Repository;

namespace SistemaVendas.Controllers
{
    public class PainelGeralController : BaseController
    {
        // GET: PainelGeral

        public PainelGeralController(ISession session) : base(session)
        {
        }

        public ActionResult Index()
        {
            var a = Session;
            return View();
        }

        public ActionResult RelatorioVendedor()
        {
            return View();
        }
        public ActionResult RealizarCompra()
        {
            return View();
        }

        public ActionResult EstornarVenda()
        {
            return View();
        }

        public ActionResult RealizarVenda()
        {
            return View();
        }

        public ActionResult CadastrarCliente()
        {
            return View();
        }
        
        public ActionResult CadastrarVendedor()
        {
            return View();
        }
        public ActionResult UrlCadastrarVendedor()
        {
            var result = new JsonResult();
            result.ContentType = "/PainelGeral/Index";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinalizarVenda(int? idCliente, int[] ListaProdutos, int[] ListaQuantidade)
        {
            var result = new JsonResult();
            var total = 0.0;
            var pagSeguro = new PagSeguro();
            var item = new ItemVenda();
            var prod = new ItemVenda();
            var vendedor = _session.Query<Vendedor>().OrderByDescending(x => x.Id).FirstOrDefault();
            var cliente = _session.Query<Cliente>().Where(x => x.Id == idCliente).FirstOrDefault();
            var listProduto = _session.Query<Produto>().Where(x => ListaProdutos.Contains(x.Id)).ToList();
            var cont = 0;
            var itens = "";
            foreach (Produto i in listProduto)
            {
                total += (float)i.Valor * ListaQuantidade[cont];
                itens = itens + i.Nome + " ";
                cont++;
            }
            DateTime data = DateTime.Now;
            var venda = new Venda();
            venda.IdCliente = cliente.Id;
            venda.IdVendedor = vendedor.Id;
            venda.ValorTotal = (float)total;
            venda.DataVenda = data;
            cont = 0;
            foreach (Produto i in listProduto)
            {
                var itemVenda = new ItemVenda();
                itemVenda.Quantidade = ListaQuantidade[cont];
                itemVenda.IdProduto = i.Id;
                itemVenda.IdVenda = venda.Id;
                _session.Save(itemVenda);
                cont++;
            }
            int refe;
            try
            {
                 refe = _session.Query<Venda>().OrderByDescending(x => x.Referencia).FirstOrDefault().Referencia + 1;
                 venda.Referencia = refe;
            }
            catch
            {
                venda.Referencia = 1;
            }
            _session.Save(venda);
            var url = pagSeguro.CheckOut(cliente.Pessoa.Nome, itens, venda.ValorTotal.ToString(), cont.ToString(), cliente.Telefone, venda.Referencia.ToString());
            venda.url = url;
            result.Data = venda;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarCliente(string cpf)
        {
            var result = new JsonResult();
            var resp = _session.Query<Cliente>().Where(x => x.CPF.ToLower() == cpf.ToLower()).FirstOrDefault();
            result.Data = resp;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ConfirmarEstorno(int idVenda)
        {
            var pagSeguro = new PagSeguro();
            var result = new JsonResult();
            var venda = _session.Query<Venda>().Where(x => x.Id == idVenda).FirstOrDefault();
            venda.IdVendedor = 0;
            venda.IdCliente = 0;
            var code = pagSeguro.ConsultarTransacao(venda.Referencia);
            pagSeguro.Estornar(code);
            _session.Save(venda);
            result.Data = venda;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuscarEstorno(string cpf)
        {
            var result = new JsonResult();
            var resp = _session.Query<Cliente>().Where(x => x.CPF.ToLower() == cpf.ToLower()).FirstOrDefault();
            var vendas = _session.Query<Venda>().Where(x => x.IdCliente == resp.Id).OrderByDescending(x => x.Id).ToList();
            resp.Compras = vendas;
            result.Data = resp;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarVendedor(string nome)
        {
            var result = new JsonResult();
            var total = 0.0;
            var pessoa = _session.Query<Pessoa>().Where(x => x.Nome.ToLower() == nome.ToLower()).FirstOrDefault();
            if (pessoa != null)
            {
                var resp = _session.Query<Vendedor>().Where(x => x.IdPessoa == pessoa.Id).FirstOrDefault();
                resp.Pessoa = pessoa;
                var quantiVenda = _session.Query<Venda>().Where(x => x.IdVendedor == resp.Id).ToList();
                resp.QuantidadeVendas = quantiVenda.Count();
                foreach (Venda i in quantiVenda)
                {
                    total += i.ValorTotal;
                }
                resp.ValorTotalVendas = (float)total;
                result.Data = resp;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult BuscarProduto(string nomeProduto)
        {
            var result = new JsonResult();
            var resp = nomeProduto == null ? null : _session.Query<Produto>().Where(x => x.Nome.ToLower() == nomeProduto.ToLower()).FirstOrDefault();
            result.Data = resp;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SalvarCompra(string cnpj, string razaoSocial, DateTime dataCompra, float? valorCompra, int? quantidade, string nome, string descricao, float? valorUnitario)
        {
            var fornecedor = new Fornecedor();
            var compra = new Compra();
            var itemCompra = new ItemCompra();
            var produto = new Produto();
            var result = new JsonResult();
            try
            {
                fornecedor.CNPJ = cnpj;
                fornecedor.RazaoSocial = razaoSocial;
                _session.Save(fornecedor);
                compra.DataCompra = dataCompra;
                compra.Fornecedor = fornecedor;
                compra.ValorTotal = valorCompra == null ? 0 : valorCompra.Value;
                _session.Save(compra);
                produto.Descricao = descricao;
                produto.Nome = nome;
                produto.Valor = valorUnitario == null ? 0 : valorUnitario.Value;
                _session.Save(produto);
                itemCompra.Quantidade = quantidade == null ? 0 : quantidade.Value;
                itemCompra.Compra = compra;
                itemCompra.Produto = produto;
                _session.Save(itemCompra);
                result.Data = true;
            }
            catch
            {
                result.Data = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult BuscarVendedorId(int id)
        {
            var result = new JsonResult();
            var vendedor = _session.Query<Vendedor>().Where(x => x.Id == id).FirstOrDefault();
            vendedor.Pessoa = _session.Query<Pessoa>().Where(x => x.Id == vendedor.IdPessoa).FirstOrDefault();
            result.Data = vendedor;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarClienteId(int id)
        {
            var result = new JsonResult();
            var cliente = _session.Query<Cliente>().Where(x => x.Id == id).FirstOrDefault();
            cliente.Pessoa = _session.Query<Pessoa>().Where(x => x.Id == cliente.IdPessoa).FirstOrDefault();
            result.Data = cliente;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarVendedor()
        {
            var result = new JsonResult();
            var lista = _session.Query<Vendedor>().ToList();
            foreach (Vendedor elemento in lista)
            {
                elemento.Pessoa = _session.Query<Pessoa>().Where(x => x.Id == elemento.IdPessoa).FirstOrDefault();
            }
            result.Data = lista;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarClientes()
        {
            var result = new JsonResult();
            var lista = _session.Query<Cliente>().ToList();
            foreach (Cliente elemento in lista)
            {
                elemento.Pessoa = _session.Query<Pessoa>().Where(x => x.Id == elemento.IdPessoa).FirstOrDefault();
            }
            result.Data = lista;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GerarListaEstorno()
        {
            var result = new JsonResult();
            var lista = _session.Query<Venda>().Where(x => x.IdCliente != 0 && x.IdVendedor != 0).ToList();
            result.Data = lista;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AtualizarVendedor(int id, string nome, string senha, string setor, string usuario, float? salario, DateTime data)
        {
            var vendedor = _session.Query<Vendedor>().Where(x => x.Id == id).FirstOrDefault();
            vendedor.Pessoa = _session.Query<Pessoa>().Where(x => x.Id == vendedor.IdPessoa).FirstOrDefault();
            var result = new JsonResult();
            try
            {
                vendedor.Pessoa.Nome = nome;
                vendedor.Pessoa.DataNascimento = data;
                _session.Save(vendedor.Pessoa);
                _session.Flush();
                vendedor.Login = usuario;
                vendedor.Senha = senha;
                vendedor.Setor = setor;
                vendedor.Salario = salario == null ? 0 : salario.Value;
                _session.Save(vendedor);
                result.Data = true;
            }
            catch
            {
                result.Data = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalvarVendedor(string nome, string senha, string setor, string usuario, float? salario, DateTime data )
        {
            var pessoa = new Pessoa();
            var vendedor = new Vendedor();
            var result = new JsonResult();
            try
            {
                pessoa.Nome = nome;
                pessoa.DataNascimento = data;
                _session.Save(pessoa);
                _session.Flush();
                vendedor.Login = usuario;
                vendedor.Senha = senha;
                vendedor.Pessoa = pessoa;
                vendedor.IdPessoa = pessoa.Id;
                vendedor.Setor = setor;
                vendedor.Salario = salario == null ? 0 : salario.Value;
                _session.Save(vendedor);
                result.Data = true;
            }
            catch
            {
                result.Data = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AtualizarCliente(int id, string nome, string cpf, string telefone, string endereco, DateTime data)
        {
            var cliente = _session.Query<Cliente>().Where(x => x.Id == id).FirstOrDefault();
            cliente.Pessoa = _session.Query<Pessoa>().Where(x => x.Id == cliente.IdPessoa).FirstOrDefault();
            var result = new JsonResult();
            try
            {
                cliente.Pessoa.Nome = nome;
                cliente.Pessoa.DataNascimento = data;
                _session.Save(cliente.Pessoa);
                cliente.CPF = cpf;
                cliente.Endereco = endereco;
                cliente.Telefone = telefone;
                _session.Save(cliente);
                result.Data = true;
            }
            catch
            {
                result.Data = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalvarCliente(string nome, string cpf, string telefone, string endereco, DateTime data)
        {
            var pessoa = new Pessoa();
            var cliente = new Cliente();
            var result = new JsonResult();
            try
            {
                pessoa.Nome = nome;
                pessoa.DataNascimento = data;
                _session.Save(pessoa);
                cliente.CPF = cpf;
                cliente.IdPessoa = pessoa.Id;
                cliente.Endereco = endereco;
                cliente.Pessoa = pessoa;
                cliente.Telefone = telefone;
                _session.Save(cliente);
                result.Data = true;
            }
            catch
            {
                result.Data = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}