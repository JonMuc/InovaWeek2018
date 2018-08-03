using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Repository
{
    public class RepositoryCrud<T> : Banco<T> where T : class
    {
        public void Alterar(T entidade)
        {
            throw new NotImplementedException();
        }

        public IList<T> Consultar()
        {
            throw new NotImplementedException();
        }

        public void Excluir(T entidade)
        {
            throw new NotImplementedException();
        }

        public void Inserir(T entidade)
        {
            throw new NotImplementedException();
        }

        public T RetornarPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}