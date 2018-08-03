using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Vendedor
    {
        public virtual int Id { get; set; }
        public virtual string Setor { get; set; }
        public virtual float Salario { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual List<Venda> Vendas { get; set; }
        public virtual string Login { get; set; }
        public virtual string Senha { get; set; }
        public virtual int QuantidadeVendas { get; set; }
        public virtual float ValorTotalVendas { get; set; }

    }
}