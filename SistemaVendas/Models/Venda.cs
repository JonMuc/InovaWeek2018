using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Venda
    {
        public virtual int Id { get; set; }
        public virtual DateTime DataVenda { get; set; }
        public virtual float ValorTotal { get; set; }
        //public virtual IEnumerable<Cliente> Cliente { get; set; }
        public virtual int IdCliente { get; set; }
        public virtual int IdVendedor { get; set; }
        public virtual string url { get; set; }

        public virtual int Referencia { get; set; }
        //public virtual Vendedor Vendedor { get; set; }
        public virtual List<ItemVenda> ItensVenda { get; set; }
        //public virtual ItemVenda ItemVenda { get; set; }
    }
}