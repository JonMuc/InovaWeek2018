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
            Id<long>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<string>(x => x.Conteudo, map => {
                map.Length(20000);     
            });
            Property<DateTime>(x => x.Data);
            Property<bool>(x => x.IsResposta);
            Property<long>(x => x.ID_Resposta);
            Property<long>(x => x.ID_Usuario);
            Property<long>(x => x.Nota);
            Property<long>(x => x.NumAvaliacoes);

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

