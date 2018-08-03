using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class ItemVenda
    {
        public virtual int Id { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual Venda Venda { get; set; }
        public virtual int IdVenda { get; set; }
        public virtual int IdProduto { get; set; }
        //public virtual Produto Produto { get; set; }
    }
}