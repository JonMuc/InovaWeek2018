using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Usuario
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sobrenome { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual string Login { get; set; }
        public virtual string Senha { get; set; }
        public virtual int Curso { get; set; }
        public virtual long AreasInteresse { get; set; }
        public virtual long NotaAvaliacao { get; set; }
        public virtual long QuantidadeAvaliacao { get; set; }
    }
}