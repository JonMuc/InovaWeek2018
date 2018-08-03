using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Cliente
    {
        public virtual int Id { get; set; }
        public virtual string CPF { get; set; }
        public virtual string Telefone { get; set; }
        public virtual int IdPessoa { get; set; }
        public virtual string Endereco { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual List<Venda> Compras { get; set; }
    }
}