using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class ItemCompraMap : ClassMapping<ItemCompra>
    {
        public ItemCompraMap()
        {
            Id<int>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<int>(x => x.Quantidade);
            OneToOne(x => x.Compra, map =>
            {
                map.PropertyReference(typeof(Compra).GetProperty("CompraId"));
                map.Cascade(Cascade.All);
            });
            OneToOne(x => x.Produto, map =>
            {
                map.PropertyReference(typeof(Produto).GetProperty("ProdutoId"));
                map.Cascade(Cascade.All);
            });
        }
    }
}

