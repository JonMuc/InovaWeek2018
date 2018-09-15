using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class UsuarioMap : ClassMapping<Usuario>
    {

        public UsuarioMap()
        {
            Id<long>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<long>(x => x.AreasInteresse);
            Property<long>(x => x.Curso);
            Property<long>(x => x.QuantidadeAvaliacao);
            Property<long>(x => x.NotaAvaliacao);
            Property<string>(x => x.Login);
            Property<string>(x => x.Senha);
            Property<string>(x => x.Email);
            Property<string>(x => x.Nome);
            Property<string>(x => x.Foto);
            Property<string>(x => x.Sobrenome);
            Property<DateTime>(x => x.DataNascimento);
            //OneToOne(x => x.Compras, map =>
            //{
            //    map.PropertyReference(typeof(Venda).GetProperty("VendaId"));
            //    map.Cascade(Cascade.All);
            //});
            //ManyToOne(x => x.Compras, map =>
            //{
            //    map.Column("VendaId");
            //    map.Cascade(Cascade.All);
            //});
            //Bag(x => x.Compras, map => map.Key(km => km.Column("Id")));
            //ManyToOne(o => o.Compras,
            //      o =>
            //      {
            //          o.Column("UserId");
            //          o.Unique(true);
            //          o.ForeignKey("Users_UserDetails_FK1");
            //      });
            //Bag(x => x.Compras, map => map.Key(km => km.Column("OrderId")));
            Table("Usuario");



        }
    }
}

