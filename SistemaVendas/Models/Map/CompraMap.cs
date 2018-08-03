using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class CompraMap : ClassMapping<Compra>
    {
        public CompraMap()
        {
            Id<int>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<DateTime>(x => x.DataCompra);
            Property<float>(x => x.ValorTotal);
            OneToOne(x => x.Fornecedor, map =>
            {
                map.PropertyReference(typeof(Fornecedor).GetProperty("Fornecedor"));
                map.Cascade(Cascade.All);
            });
            OneToOne(x => x.ItemCompra, map =>
            {
                map.PropertyReference(typeof(ItemCompra).GetProperty("ItemCompra"));
                map.Cascade(Cascade.All);
            });
            Table("Compra");
        }
    }
}

