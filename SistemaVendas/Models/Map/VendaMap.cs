using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class VendaMap : ClassMapping<Venda>
    {
        public VendaMap()
        {
            Id<int>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<DateTime>(x => x.DataVenda);
            Property<float>(x => x.ValorTotal);
            Property<int>(x => x.IdCliente);
            Property<int>(x => x.Referencia);
            Property<int>(x => x.IdVendedor);
            //OneToOne<Cliente>(x => x.Cliente, map =>
            //{
            //    map.PropertyReference(typeof(Cliente).GetProperty("ClienteId"));
            //    map.Cascade(Cascade.All);
            //});
            //OneToOne<Vendedor>(x => x.Vendedor, map =>
            //{
            //    map.PropertyReference(typeof(Vendedor).GetProperty("VendedorId"));
            //    map.Cascade(Cascade.All);
            //});
            //ManyToOne<Cliente>(x => x.Cliente, map =>
            //{
            //    map.Column("IdCliente");
            //    map.ForeignKey("Venda_Cliente");
            //    map.Cascade(Cascade.All);
            //});
            Table("Venda");
        }
    }
}

