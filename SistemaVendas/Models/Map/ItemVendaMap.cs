using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class ItemVendaMap : ClassMapping<ItemVenda>
    {
        public ItemVendaMap()
        {
            Id<int>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<int>(x => x.Quantidade);
            Property<int>(x => x.IdProduto);
            Property<int>(x => x.IdVenda);
            //OneToOne(x => x.Produto, map =>
            //{
            //    map.PropertyReference(typeof(Produto).GetProperty("ProdutoId"));
            //    map.Cascade(Cascade.All);
            //});
            //OneToOne(x => x.Venda, map =>
            //{
            //    map.PropertyReference(typeof(Venda).GetProperty("VendaId"));
            //    map.Cascade(Cascade.All);
            //});
            Table("ItemVenda");
        }
    }
}

