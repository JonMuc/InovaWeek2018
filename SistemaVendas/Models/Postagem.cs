using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Postagem
    {
        public virtual int Id { get; set; }
        public virtual int ID_Usuario { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual int Nota { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual int NumAvaliacoes { get; set; }
        public virtual Usuario Pessoa { get; set; }
        public virtual int ID_Resposta { get; set; }
        public virtual List<Venda> Compras { get; set; }
    }
}