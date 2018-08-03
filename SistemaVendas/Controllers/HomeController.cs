using NHibernate;
using SistemaVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVendas.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ISession session) : base(session)
        {
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}
        

        public ActionResult Index()
        {
            //var pessoa = new Pessoa();
            //pessoa.Nome = "Joao";
            //pessoa.DataNascimento = new DateTime();
            //_session.Save(pessoa);
            //var x = _session.Query<Pessoa>().FirstOrDefault();
            //var cliente = new Cliente();
            //cliente.Pessoa = x;
            //cliente.Telefone = "999999999";
            //cliente.Endereco = "Vila";
            //cliente.CPF = "1521632165";
            //_session.Save(cliente);
            //var vendedor = new Vendedor();
            //var pessoa1 = new Pessoa();
            //pessoa1.Nome = "JoaoVend";
            //pessoa1.DataNascimento = new DateTime();
            //_session.Save(pessoa1);
            //vendedor.Login = "guisfe";
            //vendedor.Senha = "senha";
            //vendedor.Senha = "geen";
            //vendedor.Salario = 13;
            //vendedor.Pessoa = pessoa1;
            //_session.Save(vendedor);
            //var repoCli = _session.Query<Cliente>().FirstOrDefault();
            //var repoVend = _session.Query<Vendedor>().FirstOrDefault();
            //var produto = new Produto();
            //produto.Descricao = "CARRO aberto";
            //produto.Nome = "fiat CARRO";
            //produto.Valor = 111111;
            //_session.Save(produto);
            //var venda = new Venda();
            //venda.IdCliente = repoCli.Id;
            //venda.ValorTotal = 222222;
            //venda.IdVendedor = repoVend.Id;
            //venda.DataVenda = new DateTime();
            //_session.Save(venda);
            //var itemVenda = new ItemVenda();
            //itemVenda.Quantidade = 1;
            //itemVenda.IdProduto = produto.Id;
            //itemVenda.IdVenda = venda.Id;
            //_session.Save(itemVenda);
            //////  _session.Flush();
            //var p = _session.Query<Cliente>().FirstOrDefault();
            //var a = _session.Query<Venda>().AsQueryable();
            //p.Compras = a.Where(z => z.IdCliente == p.Id).ToList();
            return View();
        }

        public JsonResult GetUser(long id)
        {
            var x = new JsonResult();
            x.Data = _session.Get<Pessoa>(id);
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logar(string login, string senha)
        {
            var result = new JsonResult();
            var vendedor = _session.Query<Vendedor>().Where(x => x.Login.ToLower() == login.ToLower() && x.Senha == senha).FirstOrDefault();
            if (vendedor != null)
            {
                result.Data = true;
                result.ContentType = "/PainelGeral/Index";
            }
            else
            {
                result.Data = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsers()
        {
            var x = new JsonResult();
            x.Data = _session.Query<Pessoa>();
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}