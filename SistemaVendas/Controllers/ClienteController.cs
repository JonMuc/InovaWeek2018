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

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUser(long id)
        {
            var x = new JsonResult();
            x.Data = _session.Get<Pessoa>(id);
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