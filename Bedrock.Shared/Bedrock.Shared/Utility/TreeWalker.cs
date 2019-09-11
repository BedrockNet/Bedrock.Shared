using System;
using System.Collections.Generic;

using Bedrock.Shared.Extension;

namespace Bedrock.Shared.Utility
{
    public class TreeWalker<T> : Singleton<TreeWalker<T>> where T : class
    {
        #region Public Methods
        public void Walk(T entity, Action<T> action)
        {
            var visitationHelper = ObjectVisitationHelper.CreateInstance();
            WalkInternal(entity, action, visitationHelper);
        }

        public void Walk(IEnumerable<T> entities, Action<T> action)
        {
            entities.Each((e) => Walk(e, action));
        }
        #endregion

        #region Private Methods
        private void WalkInternal(T entity, Action<T> action, ObjectVisitationHelper visitationHelper)
        {
            if (!visitationHelper.TryVisit(entity))
                return;

            action(entity);

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                if (propertyInfo.GetIndexParameters().Length > 0)
                    continue;

                var isEntitySingular = typeof(T).IsAssignableFrom(propertyInfo.PropertyType);
                var isEntityCollection = typeof(IEnumerable<T>).IsAssignableFrom(propertyInfo.PropertyType);

                if (isEntitySingular)
                {
                    var propertyEntity = propertyInfo.GetValue(entity, null) as T;

					if(propertyEntity != null)
						WalkInternal(propertyEntity, action, visitationHelper);
                }
                else if (isEntityCollection)
                {
                    var propertyEntities = propertyInfo.GetValue(entity, null) as IEnumerable<T>;

					if (propertyEntities != null)
						propertyEntities.Each((e) => WalkInternal(e, action, visitationHelper));
                }
            }
        }
        #endregion
    }
}
