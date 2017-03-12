using System;
using System.Linq;
using System.Reflection;
using Autofac;
using System.IO;
using System.Collections.Generic;
using Demo.Core.Tasks;

namespace Demo.Core.Infrastructure
{
    [Obsolete("This work is performed in NopEngine", true)]
    public class NopStarter
    {
        private readonly object _locker = new object();
        private bool _configured;
        private IContainer _container;

        public IContainer BuildContainer()
        {
            lock (_locker)
            {
                if (_configured)
                    return _container;

                var builder = new ContainerBuilder();

                //type finder
                var typeFinder = new TypeFinder();
                builder.Register(c => typeFinder);
                
                //find IDependencyRegistar implementations
                var drTypes = typeFinder.FindClassesOfType<IDependencyRegistar>();
                foreach (var t in drTypes)
                {
                    dynamic dependencyRegistar = Activator.CreateInstance(t);
                    dependencyRegistar.Register(builder, typeFinder);
                }

                //event
                OnContainerBuilding(new ContainerBuilderEventArgs(builder));
                _container = builder.Build();
                //event
                OnContainerBuildingComplete(new ContainerBuilderEventArgs(builder));

                _configured = true;
                return _container;
            }
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        public void ExecuteStartUpTasks()
        {
            var startUpTaskTypes = _container.Resolve<ITypeFinder>().FindClassesOfType<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
            {
                var startUpTask = ((IStartupTask)Activator.CreateInstance(startUpTaskType));
                startUpTask.Execute();
            }
        }

        protected void OnContainerBuilding(ContainerBuilderEventArgs args)
        {
            if (ContainerBuilding != null)
            {
                ContainerBuilding(this, args);
            }
        }

        protected void OnContainerBuildingComplete(ContainerBuilderEventArgs args)
        {
            if (ContainerBuildingComplete != null)
            {
                ContainerBuildingComplete(this, args);
            }
        }

        public event EventHandler<ContainerBuilderEventArgs> ContainerBuilding;

        public event EventHandler<ContainerBuilderEventArgs> ContainerBuildingComplete;

    }
}
