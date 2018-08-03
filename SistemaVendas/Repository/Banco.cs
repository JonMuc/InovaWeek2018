using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVendas.Repository
{
    public interface Banco<T>
    {
        void Inserir(T entidade);
        void Alterar(T entidade);
        void Excluir(T entidade);
        T RetornarPorId(int id);
        IList<T> Consultar();
    }
}
