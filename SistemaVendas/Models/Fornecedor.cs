using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Fornecedor
    {
        public virtual int Id { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual string RazaoSocial { get; set; }
    }
}