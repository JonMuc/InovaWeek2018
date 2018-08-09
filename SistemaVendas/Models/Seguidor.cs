using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Seguidor
    {
        public virtual int Id { get; set; }
        public virtual int ID_Usuario { get; set; }
        public virtual int ID_Seguindo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario Seguindo { get; set; }
    }
}