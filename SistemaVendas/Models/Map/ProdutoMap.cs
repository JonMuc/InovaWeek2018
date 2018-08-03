using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class ProdutoMap : ClassMapping<Produto>
    {
        public ProdutoMap()
        {
            Id<int>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<float>(x => x.Valor);
            Property<string>(x => x.Nome);
            Property<string>(x => x.Descricao);
            //OneToOne(x => x.ItemCompra, map =>
            //{
            //    map.PropertyReference(typeof(ItemCompra).GetProperty("ItemCompraId"));
            //    map.Cascade(Cascade.All);
            //});
            Table("Produto");
        }
    }
}

