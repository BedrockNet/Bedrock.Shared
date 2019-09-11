using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;
using CommonServiceLocator;

namespace Bedrock.Shared.Ioc.Autofac
{
    public class AutofacServiceLocator : ServiceLocatorImplBase
    {
        #region Constructors
        public AutofacServiceLocator(IContainer container)
        {
            if(container == null)
                throw new ArgumentNullException(nameof(container));

            Container = container;
        }
        #endregion

        #region Properties
        protected IContainer Container { get; set; }
        #endregion

        #region IDisposable Methods
        public void Dispose() { }
        #endregion

        #region ServiceLocatorImplBase Methods
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                var returnValue = default(object);

                if (serviceType.GetTypeInfo().IsAbstract || serviceType.GetTypeInfo().IsInterface)
                    Container.TryResolve(serviceType, out returnValue);
                else
                    returnValue = Container.Resolve(serviceType);

                return returnValue;
            }

            return Container.ResolveKeyed(key, serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return Container.ResolveAll(serviceType).Cast<object>();
        }
        #endregion
    }
}
