using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaVendas.Controllers
{
    public class BaseController : Controller
    {
        public ISession _session;
        public BaseController(ISession session)
        {
            _session = session;
        }

        public void asd()
        {

        }
        protected override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            //DependencyResolver.Current.GetService<ISession>().BeginTransaction(IsolationLevel.ReadCommitted);
            _session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        protected override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            //ITransaction currentTransaction = DependencyResolver.Current.GetService<ISession>().Transaction;
            ITransaction currentTransaction = _session.Transaction;

            try
            {
                if (currentTransaction.IsActive)
                    if (actionExecutedContext.Exception != null)
                        currentTransaction.Rollback();
                    else
                        currentTransaction.Commit();
            }
            finally
            {
                currentTransaction.Dispose();
            }
        }
    }
}