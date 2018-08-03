using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class ItemCompra
    {
        public virtual int Id { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual Compra Compra { get; set; }
        public virtual Produto Produto { get; set; }
    }
}