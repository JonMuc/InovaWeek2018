using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class RenderUser
    {
        public virtual Usuario Usuario { get; set; }
        public virtual List<Postagem> ListaPostagem { get; set; }
    }
}