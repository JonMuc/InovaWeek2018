using NHibernate;
using SistemaVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var vendedor = _session.Query<Vendedor>().OrderByDescending(x => x.Id).FirstOrDefault();
            var cliente = _session.Query<Cliente>().Where(x => x.Id == idCliente).FirstOrDefault();
            //List<Produto> listProduto = null;
            //foreach (int i in ListaProdutos)
            //{
            //    if (i != 0)
            //    {
            //        listProduto.Add(pro);
            //    }
            //}
            var listProduto = _session.Query<Produto>().Where(x => ListaProdutos.Contains(x.Id)).ToList();
            var cont = 0;
            foreach (Produto i in listProduto)
            {
                total += (float)i.Valor * ListaQuantidade[cont];
                cont++;
            }
            var venda = new Venda();
            venda.IdCliente = cliente.Id;
            venda.IdVendedor = vendedor.Id;
            venda.ValorTotal = (float)total;
            venda.DataVenda = new DateTime().ToLocalTime();
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
            _session.Save(venda);
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
            var result = new JsonResult();
            var venda = _session.Query<Venda>().Where(x => x.Id == idVenda).FirstOrDefault();
            venda.IdVendedor = 0;
            venda.IdCliente = 0;
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

        public ActionResult BuscarVendedor(string usuario, string nome)
        {
            var result = new JsonResult();
            var total = 0.0;
            var resp = _session.Query<Vendedor>().Where(x => x.Login.ToLower() == usuario.ToLower()).FirstOrDefault();
            var pessoa = _session.Query<Pessoa>().Where(x => x.Nome.ToLower() == nome.ToLower()).FirstOrDefault();
            resp.Pessoa = pessoa;
            var quantiVenda = _session.Query<Venda>().Where(x => x.IdVendedor == resp.Id).ToList();
            resp.QuantidadeVendas = quantiVenda.Count();
            foreach (Venda i in quantiVenda)
            {
                total += i.ValorTotal;
            }
            resp.ValorTotalVendas = (float)total;
            result.Data = resp;
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
                itemCompra.Quantidade = quantidade == null ? 0 : quantidade.Value;
                itemCompra.Compra = compra;
                _session.Save(itemCompra);
                produto.Descricao = descricao;
               // produto.ItemCompra = itemCompra;
                produto.Nome = nome;
                produto.Valor = valorUnitario == null ? 0 : valorUnitario.Value;
                _session.Save(produto);
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