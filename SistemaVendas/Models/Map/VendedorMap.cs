using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class VendedorMap : ClassMapping<Vendedor>
    {
        public VendedorMap()
        {
            Id<int>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<string>(x => x.Setor);
            Property<float>(x => x.Salario);
            Property<int>(x => x.IdPessoa);
            OneToOne(x => x.Pessoa, map =>
            {
                map.PropertyReference(typeof(Pessoa).GetProperty("PessoaId"));
                map.Cascade(Cascade.All);
            });
            //ManyToOne(x => x.Vendas, map =>
            //{
            //    map.Column("VendaId");
            //    map.Cascade(Cascade.All);
            //});
            //Bag(x => x.Vendas, map => map.Key(km => km.Column("VendadId")));
            //OneToOne(x => x.Vendas, map =>
            //{
            //    map.PropertyReference(typeof(Venda).GetProperty("VendaId"));
            //    map.Cascade(Cascade.All);
            //});
            Property<string>(x => x.Login);
            Property<string>(x => x.Senha);
            Table("Vendedor");
        }
    }
}

