using HtmlAgilityPack;
using NHibernate;
using SistemaVendas.Models;
using SistemaVendas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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
            var usuario = (Usuario)Session["Usuario"];
            foreach (Postagem item in lista)
            {
                item.Usuario = _session.Query<Usuario>().Where(x => x.Id == item.ID_Usuario).FirstOrDefault();
            }
            var postagem = new Postagem();
            postagem.ListaPostagem = lista;
            postagem.Usuario = _session.Query<Usuario>().Where(x => x.Id == usuario.Id).FirstOrDefault();
            postagem.Usuario.QuantidadeSeguindo = _session.Query<Seguidor>().Where(x => x.ID_Usuario == usuario.Id).Count();
            postagem.Usuario.QuantidadeSeguidores = _session.Query<Seguidor>().Where(x => x.ID_Seguindo == usuario.Id).Count();
            postagem.Usuario.Idade = DateTime.Now.Year - postagem.Usuario.DataNascimento.Year;
            var nota = _session.Query<Postagem>().Where(x => x.ID_Usuario == usuario.Id).ToList();
            postagem.Usuario.Pontos = nota != null ? nota.Select(x => x.Nota).Sum() : 0;
            postagem.Usuario.NotaAvaliacao = postagem.Usuario.QuantidadeAvaliacao == 0 ? 0 : postagem.Usuario.Pontos / postagem.Usuario.QuantidadeAvaliacao;
            postagem.Usuario.QuantidadePublicacoes = _session.Query<Postagem>().Where(x => x.ID_Usuario == usuario.Id).Count();
            postagem.Usuario.QuantidadeResposta = _session.Query<Postagem>().Where(x => x.ID_Usuario == usuario.Id && x.IsResposta).Count();
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
                item.DataString = item.Data.ToLocalTime().ToString();
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

        public ActionResult SalvarFoto(HttpPostedFileBase file)
        {
            var usuario = (Usuario)Session["Usuario"];
            var userEdit = _session.Query<Usuario>().Where(x => x.Id == usuario.Id).FirstOrDefault();
            if (file != null)
            {
                if (usuario.Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/" + userEdit.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/" + userEdit.Foto));
                    }
                }
                String[] strName = file.FileName.Split('.');
                String strExt = strName[strName.Count() - 1];
                string pathSave = String.Format("{0}{1}.{2}", Server.MapPath("~/Imagens/"), userEdit.Id, strExt);
                String pathBase = String.Format("/Imagens/{0}.{1}", userEdit.Id, strExt);
                file.SaveAs(pathSave);
                userEdit.Foto = pathBase;
                _session.Save(userEdit);
            }
            return null;
        }

        public ActionResult RenderUser(long IdUser)
        {
            var usuario = (Usuario)Session["Usuario"];
            var res = new RenderUser();
            res.Usuario = _session.Query<Usuario>().Where(x => x.Id == IdUser).FirstOrDefault();
            res.Usuario.Seguindo = _session.Query<Seguidor>().Where(x => x.ID_Usuario == usuario.Id && x.ID_Seguindo == IdUser).Any();
            res.Usuario.QuantidadeSeguindo = _session.Query<Seguidor>().Where(x => x.ID_Usuario == usuario.Id).Count();
            res.Usuario.QuantidadeSeguidores = _session.Query<Seguidor>().Where(x => x.ID_Seguindo == usuario.Id).Count();
            res.Usuario.Idade = DateTime.Now.Year - res.Usuario.DataNascimento.Year;
            var nota = _session.Query<Postagem>().Where(x => x.ID_Usuario == usuario.Id).ToList();
            res.Usuario.Pontos = nota != null ? nota.Select(x => x.Nota).Sum() : 0;
            res.Usuario.NotaAvaliacao = res.Usuario.QuantidadeAvaliacao == 0 ? 0 : res.Usuario.Pontos / res.Usuario.QuantidadeAvaliacao;
            res.Usuario.QuantidadePublicacoes = _session.Query<Postagem>().Where(x => x.ID_Usuario == usuario.Id).Count();
            res.Usuario.QuantidadeResposta = _session.Query<Postagem>().Where(x => x.ID_Usuario == usuario.Id && x.IsResposta).Count();
            var lista = _session.Query<Postagem>().Where(x => x.ID_Usuario == IdUser && !x.IsResposta).OrderByDescending(x => x.Data).Take(5).ToList();
            foreach (Postagem item in lista)
            {
                item.Usuario = _session.Query<Usuario>().Where(x => x.Id == item.ID_Usuario).FirstOrDefault();
                item.DataString = item.Data.ToLocalTime().ToString();
                item.QuantidadeResposta = _session.Query<Postagem>().Where(x => item.Id == x.ID_Resposta).Count();
                item.ListaResposta = _session.Query<Postagem>().Where(x => item.Id == x.ID_Resposta).ToList();
                item.Avaliei = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id && x.ID_Usuario == usuario.Id).Any();
                if (item.Avaliei)
                {
                    item.NotaUsuario = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id && x.ID_Usuario == usuario.Id).FirstOrDefault().Nota;
                }
            }
            res.ListaPostagem = lista;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public void Desseguir(long IdUser)
        {
            var usuario = (Usuario)Session["Usuario"];
            var userDesseguir = _session.Query<Seguidor>().Where(x => x.ID_Usuario == usuario.Id && x.ID_Seguindo == IdUser).FirstOrDefault();
            userDesseguir.ID_Seguindo = 9999999;
            userDesseguir.ID_Usuario = 9999999;
            _session.Save(userDesseguir);
        }

        public void Seguir(long idUser)
        {
            var usuario = (Usuario)Session["Usuario"];
            var seguidor = new Seguidor();
            seguidor.ID_Usuario = usuario.Id;
            seguidor.ID_Seguindo = idUser;
            _session.Save(seguidor);
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
            postagem.Nota = postagem.Nota + nota;
            //postagem.ID_Usuario = usuario.Id;//
            postagem.NumAvaliacoes = postagem.NumAvaliacoes + 1;
            _session.Save(postagem);
            var usuarioPostagem = _session.Query<Usuario>().Where(x => x.Id == postagem.ID_Usuario).FirstOrDefault();
            usuarioPostagem.NotaAvaliacao = usuario.NotaAvaliacao + nota;
            usuarioPostagem.QuantidadeAvaliacao = usuarioPostagem.QuantidadeAvaliacao + 1;
            _session.Save(usuarioPostagem);
            result.Data = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarSubComentarios(long idPostagem) {
            var result = new JsonResult();
            var usuario = (Usuario)Session["Usuario"];
            var postagem = _session.Query<Postagem>().Where(x => x.Id == idPostagem).FirstOrDefault();
            postagem.Usuario = _session.Query<Usuario>().Where(x => x.Id == postagem.ID_Usuario).FirstOrDefault();
            postagem.DataString = postagem.Data.ToLocalTime().ToString();
            postagem.QuantidadeResposta = _session.Query<Postagem>().Where(x => postagem.Id == x.ID_Resposta).Count();
            postagem.ListaResposta = _session.Query<Postagem>().Where(x => postagem.Id == x.ID_Resposta).OrderByDescending(x => x.Data).ToList();
            foreach (Postagem item in postagem.ListaResposta)
            {
                item.Usuario = _session.Query<Usuario>().Where(x => x.Id == item.ID_Usuario).FirstOrDefault();
                item.DataString = item.Data.ToLocalTime().ToString();
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

        public ActionResult BuscarUltimasPotagens()
        {
            var result = new JsonResult();
            var usuario = (Usuario)Session["Usuario"];
            var IdsPostagens = _session.Query<Postagem>().Where(x => x.ID_Usuario == usuario.Id).Select(x => x.Id);
            var postagem = _session.Query<Postagem>().Where(x => x.ID_Usuario != usuario.Id).Where(x => IdsPostagens.Contains(x.ID_Resposta)).OrderByDescending(x => x.Data).Take(10);
            var IdsAvaliacao = _session.Query<Avaliacao>().Where(x => IdsPostagens.Contains(x.ID_Postagem)).Where(x => x.ID_Usuario != usuario.Id).Select(x => x.ID_Postagem);
            var listaAvaliado = _session.Query<Postagem>().Where(x => IdsAvaliacao.Contains(x.Id));
            var aux = postagem.AsEnumerable().Union(listaAvaliado);
            foreach (Postagem item in aux)
            {
                item.Usuario = _session.Query<Usuario>().Where(x => x.Id == item.ID_Usuario).FirstOrDefault();
                item.DataString = item.Data.ToLocalTime().ToString();
                item.QuantidadeResposta = _session.Query<Postagem>().Where(x => item.Id == x.ID_Resposta).Count();
                item.ListaResposta = _session.Query<Postagem>().Where(x => item.Id == x.ID_Resposta).ToList();
                item.Avaliei = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id && x.ID_Usuario != usuario.Id).Any();
                if (item.Avaliei)
                {
                    var buscar = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id).Where(x => x.ID_Usuario != usuario.Id).Select(x => x.ID_Usuario);
                    item.Usuario = _session.Query<Usuario>().Where(x => buscar.Contains(x.Id)).FirstOrDefault();
                    item.NotaUsuario = _session.Query<Avaliacao>().Where(x => x.ID_Postagem == item.Id && buscar.Contains(x.ID_Usuario)).FirstOrDefault().Nota;
                }
            }
            result.Data = aux;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarUsuarioNome(string nome)
        {
            var result = new JsonResult();
            var listaUser = _session.Query<Usuario>().Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();
            foreach (Usuario lista in listaUser) {
                var nota = _session.Query<Postagem>().Where(x => x.ID_Usuario == lista.Id).ToList();
                lista.Pontos = nota != null ? nota.Select(x => x.Nota).Sum() : 0;
                lista.NotaAvaliacao = lista.QuantidadeAvaliacao == 0 ? 0 : lista.Pontos / lista.QuantidadeAvaliacao;
            }
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