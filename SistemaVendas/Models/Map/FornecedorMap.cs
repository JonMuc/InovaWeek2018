using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SistemaVendas.Models.Map
{
    class FornecedorMap : ClassMapping<Fornecedor>
    {
        public FornecedorMap()
        {
            Id<int>(x => x.Id, map => {
                map.Generator(Generators.Increment);
            });
            Property<string>(x => x.CNPJ);
            Property<string>(x => x.RazaoSocial);
            Table("Fornecedor");
        }
    }
}

