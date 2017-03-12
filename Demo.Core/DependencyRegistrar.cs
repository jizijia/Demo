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

namespace Demo.Core
{
    public class DependencyRegistrar : IDependencyRegistrar
    {

        //public virtual void Register(Autofac.ContainerBuilder builder, ITypeFinder typeFinder)
        public void Register(Autofac.ContainerBuilder builder, ITypeFinder typeFinder, TBSConfig config)
        {
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>(ContractConst.CACHE_STATIC).SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>(ContractConst.CACHE_PER_REQUEST);
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
