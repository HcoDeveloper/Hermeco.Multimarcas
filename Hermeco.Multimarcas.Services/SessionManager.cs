using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using System.Linq;
using Hermeco.Multimarcas.Services.Entities;
using System.Configuration;

namespace Hermeco.Multimarcas.Services
{
    public static class SessionManager
    {
        public static ISession OpenSession(String connectionString)
        {
            ISessionFactory sessionFactory = Fluently.Configure().Database(
                MsSqlConfiguration.MsSql2012.ConnectionString(
                connectionString).ShowSql())
                .Mappings( m => m.FluentMappings.AddFromAssemblyOf<ClienteOfertaEntity>() )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<OfertaEntity>())
                .ExposeConfiguration( cfg => new SchemaExport(cfg).Create(false,false))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
