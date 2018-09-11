using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class AvaliacaoMap : ClassMapping<Avaliacao>
    {

        public AvaliacaoMap()
        {
            Id<long>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<long>(x => x.ID_Postagem);
            Property<long>(x => x.ID_Usuario);
            Property<long>(x => x.Nota);
            Table("Avaliacao");
        }
    }
}

