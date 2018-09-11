using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Postagem
    {
        public virtual long Id { get; set; }
        public virtual long ID_Usuario { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual long Nota { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual long NumAvaliacoes { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual long ID_Resposta { get; set; }
        public virtual bool IsResposta { get; set; }

        public virtual string DataString { get; set; }

        //estatico
        public virtual List<Postagem> ListaPostagem { get; set; }
        public virtual List<Postagem> ListaResposta { get; set; }
        public virtual long QuantidadeResposta { get; set; }
        public virtual bool Avaliei { get; set; }
        public virtual long NotaUsuario { get; set; }

    }
}