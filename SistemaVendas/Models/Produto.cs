using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Produto
    {
        public virtual int Id { get; set; }
        public virtual float Valor { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        //public virtual ItemCompra ItemCompra { get; set; }
    }
}