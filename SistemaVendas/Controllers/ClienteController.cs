using NHibernate;
using SistemaVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVendas.Controllers
{
    public class ClienteController : BaseController
    {
        public ClienteController(ISession session) : base(session)
        {
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}


        public ActionResult Index()
        {
            //var a = new Pessoa();
            //a.Nome = "Joao";
            //a.DataNascimento = new DateTime();
            //_session.Save(a);
            //var x = _session.Query<Pessoa>().FirstOrDefault();
            //var c = new Cliente();
            //c.Pessoa = x;
            //c.Telefone = "999999999";
            //c.Endereco = "Vila";
            //c.CPF = "1521632165";
            //_session.Save(c);
            //var vend = new Venda();
            //vend.Cliente = c;
            //vend.ValorTotal = 123;
            //_session.Save(vend);
            //var p = _session.Query<Cliente>().FirstOrDefault();
            return View();
        }

        public JsonResult GetUser(long id)
        {
            var x = new JsonResult();
            x.Data = _session.Get<Pessoa>(id);
            return Json(x, JsonRequestBehavior.AllowGet);
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