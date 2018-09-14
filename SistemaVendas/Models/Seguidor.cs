using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaVendas.Models
{
    public class Seguidor
    {
        public virtual long Id { get; set; }
        public virtual long ID_Usuario { get; set; }
        public virtual long ID_Seguindo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario Seguindo { get; set; }
    }
}