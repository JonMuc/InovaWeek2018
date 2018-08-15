using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using SistemaVendas.Models;
using SistemaVendas.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SistemaVendas.Repository
{
    public class FluentySession
    {
        private static string ConnectionString = "Data Source=DESKTOP-7L5H0ES; user Id=sa; Password=123456; Initial Catalog=SistemaInova;Integrated Security=True";
        //private static string ConnectionString = "Data Source=sistemavendasbd.database.windows.net;Initial Catalog=sistemavendasBD;Integrated Security=False;User ID=sistemavendasBD;Password=Sistemavenda123;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // private static string ConnectionString = "Data Source=sistemavendasbd.database.windows.net;Initial Catalog=sistemavendasbd;Integrated Security=False;User ID=sistemabd;Password=Sistema@bd;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private static ISessionFactory session;
        private static Configuration NHConfig;

        public static Configuration ConfigNHibernate()
        {
            var configure = new Configuration();
            configure.SessionFactoryName("BuildIt");
            configure.DataBaseIntegration(db =>
            {
                db.Dialect<MsSqlAzure2008Dialect>();
                db.Driver<SqlClientDriver>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.IsolationLevel = System.Data.IsolationLevel.ReadCommitted;
                db.ConnectionString = ConnectionString;
                db.Timeout = 10;
            });
            return configure;
        }



        public static ISessionFactory Setup()
        {
            NHConfig = ConfigNHibernate();
            HbmMapping mapping = GetMappings();
            NHConfig.AddDeserializedMapping(mapping, "SistemaInova");
            SchemaMetadataUpdater.QuoteTableAndColumns(NHConfig, new MsSqlAzure2008Dialect());
            session = NHConfig.BuildSessionFactory();

            //CreateDataBase();
            if (!ValidateSchema())
            {
              //  throw new Exception("dis db is fucked up boi");
            }
            return session;
        }
        
        public static bool ValidateSchema()
        {
            try
            {
                SchemaValidator schemaValidator = new SchemaValidator(NHConfig);
                schemaValidator.Validate();
                return true;
            }
            catch (HibernateException e)
            {
                return false;
            }
        }

        public static HbmMapping GetMappings()
        {
            ModelMapper mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            //mapper.AddMapping<PessoaMap>();
            //mapper.AddMapping<ClienteMap>();
            mapper.AddMapping<UsuarioMap>();
            mapper.AddMapping<AvaliacaoMap>();
            mapper.AddMapping<SeguidorMap>();
            mapper.AddMapping<PostagemMap>();
            //mapper.AddMapping<CompraMap>();
            //mapper.AddMapping<FornecedorMap>();
            //mapper.AddMapping<ItemCompraMap>();
            //mapper.AddMapping<ItemVendaMap>();
            //mapper.AddMapping<ProdutoMap>();
            //mapper.AddMapping<VendaMap>();
            //mapper.AddMapping<VendedorMap>();
          
            // HbmMapping mapping = mapper.CompileMappingFor(new[] { typeof(Pessoa), typeof(Cliente) });

            HbmMapping mapping = mapper.CompileMappingFor(new[] { typeof(Avaliacao), typeof(Usuario), typeof(Seguidor), typeof(Postagem) });
            return mapping;
        }


        public static void CreateDataBase()
        {
            new SchemaExport(NHConfig).Drop(true, true);
            new SchemaExport(NHConfig).Create(false, true);
        }

    }
}