using System;
using System.Collections.Generic;

using Bedrock.Shared.Extension;
using Bedrock.Shared.Session.Interface;

using Autofac;
using CommonServiceLocator;

namespace Bedrock.Shared.Ioc.Autofac
{
    public class SessionServiceLocator : AutofacServiceLocator
    {
        #region Constructors
        public SessionServiceLocator(IContainer container) : base(container) { }
        #endregion

        #region ServiceLocatorImplBase Methods
        protected override object DoGetInstance(Type serviceType, string key)
        {
            var instance = base.DoGetInstance(serviceType, key);
            Enlist(instance);

            return instance;
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            var instances = base.DoGetAllInstances(serviceType);
            instances.Each(i => Enlist(i));

            return instances;
        }
        #endregion

        #region Private Methods
        private void Enlist(object instance)
        {
            var sessionAware = instance as ISessionAware;

            if (sessionAware != null)
            {
                var session = ServiceLocator.Current.GetInstance<ISession>();

                if (session != null)
                    sessionAware.Enlist(session);
            }
        }
        #endregion
    }
}
