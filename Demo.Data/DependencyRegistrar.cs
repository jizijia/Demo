using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Demo.Core.Caching;
using Demo.Core.Configuration;
using Demo.Core.Infrastructure;
using Demo.Core.Infrastructure.DependencyManagement;
using Demo.Core.Data;

namespace Demo.Data
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(Autofac.ContainerBuilder builder, ITypeFinder typeFinder, TBSConfig config)
        {
            #region Data Layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();
            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();
            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                dataProvider.InitConnectionFactory();
                builder.Register<IDbContext>(c => new TBSObjectContext(dataProviderSettings.DataConnectionString)).InstancePerLifetimeScope();
            }
            else
            {
                builder.Register<IDbContext>(c => new TBSObjectContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerLifetimeScope();
            }

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            //builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            #endregion
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
