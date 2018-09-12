using HtmlAgilityPack;
using NHibernate;
using SistemaVendas.Models;
using SistemaVendas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebCrawler;

namespace SistemaVendas.Controllers
{
    public class CadastroController : BaseController
    {
        public CadastroController(ISession session) : base(session)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logar(string login, string senha)
        {
            var result = new JsonResult();
            var usuario = _session.Query<Usuario>().Where(x => x.Login.ToLower() == login.ToLower() && x.Senha == senha).FirstOrDefault();
            if (usuario != null)
            {
                Session.Add("Usuario", usuario);
                result.Data = true;
                result.ContentType = "/PainelGeral/Index";
            }
            else
            {
                result.Data = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DirecionarAposCadastro()
        {
            Response.BufferOutput = false;
            Response.Redirect("/Dashboard/Index");
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Salvar(string usuario, string senha, string nome, DateTime? data, string email, long curso, List<long> area)
        {
            var result = new JsonResult();
            var novoUsuario = new Usuario();
            novoUsuario.Nome = nome;
            novoUsuario.Login = usuario;
            novoUsuario.Senha = senha;
            novoUsuario.DataNascimento = data.Value;
            novoUsuario.Email = email;
            novoUsuario.Curso = curso;
            novoUsuario.AreasInteresse = area.Sum();
            _session.Save(novoUsuario);
            result.Data = true;
            result.ContentType = "/Dashboard/Index";
            result.Data = false;
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