using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Avaliacao
    {
        public virtual int Id { get; set; }
        public virtual int ID_Postagem { get; set; }
        public virtual Postagem Postagem { get; set; }
        public virtual int ID_Usuario { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}