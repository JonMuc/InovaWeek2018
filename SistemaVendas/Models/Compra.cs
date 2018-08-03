using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Compra
    {
        public virtual int Id { get; set; }
        public virtual DateTime DataCompra { get; set; }
        public virtual float ValorTotal { get; set; }
        public virtual ItemCompra ItemCompra { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
    }
}