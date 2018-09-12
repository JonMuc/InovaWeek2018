using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Avaliacao
    {
        public virtual long Id { get; set; }
        public virtual long ID_Postagem { get; set; }
        public virtual Postagem Postagem { get; set; }
        public virtual long ID_Usuario { get; set; }
        public virtual long Nota { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}