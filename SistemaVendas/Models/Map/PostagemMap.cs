using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class PostagemMap : ClassMapping<Postagem>
    {

        public PostagemMap()
        {
            Id<int>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<string>(x => x.Conteudo);
            Property<DateTime>(x => x.Data);
            Property<int>(x => x.ID_Resposta);
            Property<int>(x => x.ID_Usuario);
            Property<int>(x => x.Nota);
            Property<int>(x => x.NumAvaliacoes);

            //OneToOne(x => x.Pessoa, map =>
            //{
            //    map.PropertyReference(typeof(Pessoa).GetProperty("PessoaId"));
            //    map.Cascade(Cascade.All);
            //});
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
            Table("Postagem");
        }
    }
}

