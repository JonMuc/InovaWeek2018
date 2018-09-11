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
    public class DashboardController : BaseController
    {
        public DashboardController(ISession session) : base(session)
        {
        }

        public ActionResult Index()
        {
            var lista = _session.Query<Postagem>().OrderByDescending(x => x.Data).Take(10).ToList();
            foreach (Postagem item in lista)
            {
                item.Usuario = _session.Query<Usuario>().Where(x => x.Id == item.ID_Usuario).FirstOrDefault();
            }
            var postagem = new Postagem();
            postagem.ListaPostagem = lista;
            return View(postagem);
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

        public ActionResult GerarListaComentario()
        {
            var result = new JsonResult();
            var usuario = (Usuario)Session["Usuario"];
            var lista = _session.Query<Postagem>().Where(x => !x.IsResposta).OrderByDescending(x => x.Data).Take(10).ToList();
            foreach (Postagem item in lista)
            {
                item.Usuario = _session.Query<Usuario>().Where(x => x.Id == item.ID_Usuario).FirstOrDefault();
                item.DataString = item.Data.ToLongDateString();
                item.QuantidadeResposta = _session.Query<Postagem>().Where(x => item.Id == x.ID_Resposta).Count();
                item.ListaResposta = _session.Query<Postagem>().Where(x => item.Id == x.ID_Resposta).ToList();
                item.Avaliei = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id && x.ID_Usuario == usuario.Id).Any();
                if (item.Avaliei)
                {
                    item.NotaUsuario = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id && x.ID_Usuario == usuario.Id).FirstOrDefault().Nota;
                }
            }
            result.Data = lista;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SairSessao()
        {
            Session.Clear();
            Response.BufferOutput = false;
            Response.Redirect("/Home/Index");
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RealizarAvaliacao(long idPostagem, int nota)
        {
            var result = new JsonResult();
            var usuario = (Usuario)Session["Usuario"];
            var avaliacao = new Avaliacao();
            if (_session.Query<Avaliacao>().Where(x => x.ID_Postagem == idPostagem && x.ID_Usuario == usuario.Id).Any() && _session.Query<Avaliacao>().Where(x => x.ID_Postagem == idPostagem && x.ID_Usuario == usuario.Id).FirstOrDefault().Nota == nota)
            {
                return null;
            }
            if (_session.Query<Avaliacao>().Where(x => x.ID_Postagem == idPostagem && x.ID_Usuario == usuario.Id).Any() && _session.Query<Avaliacao>().Where(x => x.ID_Postagem == idPostagem && x.ID_Usuario == usuario.Id).FirstOrDefault().Nota != nota)
            {
                var delete = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == idPostagem && x.ID_Usuario == usuario.Id).FirstOrDefault();
                delete.ID_Usuario = 999999;
                delete.ID_Postagem = 999999;
                _session.Save(delete);
            }
            avaliacao.ID_Postagem = idPostagem;
            avaliacao.ID_Usuario = usuario.Id;
            avaliacao.Nota = nota;
            _session.Save(avaliacao);
            var postagem = _session.Query<Postagem>().Where(x => x.Id == idPostagem).FirstOrDefault();
            postagem.Nota =+ nota;
            postagem.NumAvaliacoes++;
            _session.Save(postagem);
            var usuarioPostagem = _session.Query<Usuario>().Where(x => x.Id == postagem.ID_Usuario).FirstOrDefault();
            usuarioPostagem.NotaAvaliacao += nota;
            usuarioPostagem.QuantidadeAvaliacao++;
            _session.Save(usuarioPostagem);
            result.Data = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarSubComentarios(long idPostagem) {
            var result = new JsonResult();
            var usuario = (Usuario)Session["Usuario"];
            var postagem = _session.Query<Postagem>().Where(x => x.Id == idPostagem).FirstOrDefault();
            postagem.Usuario = _session.Query<Usuario>().Where(x => x.Id == postagem.ID_Usuario).FirstOrDefault();
            postagem.DataString = postagem.Data.ToLongDateString();
            postagem.QuantidadeResposta = _session.Query<Postagem>().Where(x => postagem.Id == x.ID_Resposta).Count();
            postagem.ListaResposta = _session.Query<Postagem>().Where(x => postagem.Id == x.ID_Resposta).OrderByDescending(x => x.Data).ToList();
            foreach (Postagem item in postagem.ListaResposta)
            {
                item.Usuario = _session.Query<Usuario>().Where(x => x.Id == item.ID_Usuario).FirstOrDefault();
                item.DataString = item.Data.ToLongDateString();
                item.QuantidadeResposta = _session.Query<Postagem>().Where(x => item.Id == x.ID_Resposta).Count();
                item.ListaResposta = _session.Query<Postagem>().Where(x => item.Id == x.ID_Resposta).ToList();
                item.Avaliei = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id && x.ID_Usuario == usuario.Id).Any();
                if (item.Avaliei)
                {
                    item.NotaUsuario = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id && x.ID_Usuario == usuario.Id).FirstOrDefault().Nota;
                }
            }
            result.Data = postagem;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EnviarComentario(string comentario, long idPostagem)
        {
            var result = new JsonResult();
            var usuario = (Usuario)Session["Usuario"];
            var postagem = new Postagem();
            postagem.Conteudo = comentario;
            postagem.Data = DateTime.Now;
            postagem.ID_Usuario = usuario.Id;
            postagem.ID_Resposta = idPostagem;
            postagem.IsResposta = true;
            _session.Save(postagem);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarUsuarioNome(string nome)
        {
            var result = new JsonResult();
            var listaUser = _session.Query<Usuario>().Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();
            result.Data = listaUser;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarUsuario()
        {
            var result = new JsonResult();
            var usuario = (Usuario)Session["Usuario"];
            result.Data = usuario;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Comentar(string comentario)
        {
            var result = new JsonResult();
            var usuario = (Usuario) Session["Usuario"];
            var novoComentario = new Postagem();
            novoComentario.Conteudo = comentario;
            novoComentario.Data = DateTime.Now;
            novoComentario.ID_Usuario = usuario.Id;
            _session.Save(novoComentario);
            result.Data = true;
            return Json(result, JsonRequestBehavior.AllowGet);
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



            //var usuario = _session.Query<Usuario>().Where(x => x.Login.ToLower() == login.ToLower() && x.Senha == senha).FirstOrDefault();
            if (novoUsuario != null)
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