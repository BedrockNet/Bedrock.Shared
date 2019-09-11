using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Bedrock.Shared.Extension;

using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Security.Interface;

using Bedrock.Shared.Service.Interface;
using Bedrock.Shared.Session.Interface;

using CommonServiceLocator;

namespace Bedrock.Shared.Session.Implementation
{
    public class BedrockSession : ISession
    {
		#region Member Fields
		private IDictionary<Type, IUnitOfWork> _unitsOfWork;
        #endregion

        #region Protected Properties
        protected IDictionary<Type, IUnitOfWork> UnitsOfWork
        {
            get
            {
                _unitsOfWork = _unitsOfWork ?? new Dictionary<Type, IUnitOfWork>();
                return _unitsOfWork;
            }
        }
        #endregion

        #region ISession Properties
        public IBedrockUser User { get; private set; }

        public ClaimsPrincipal Principal { get; private set; }
        #endregion

        #region ISession Methods
        public void SetUser(IBedrockUser user)
        {
			User = user ?? throw new ArgumentNullException(nameof(user));
            Principal = new BedrockPrincipal(User, new BedrockIdentity(User));
        }

        public void SetUser(IBedrockUser user, ClaimsPrincipal principal)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Principal = principal;
        }

        public IUnitOfWork GetUnitOfWork<T>() where T : IUnitOfWork
        {
            var type = typeof(T);

            if (UnitsOfWork.ContainsKey(type))
                return UnitsOfWork[type];

            var unitOfWork = ServiceLocator.Current.GetInstance<T>();
            unitOfWork.Session = this;

            UnitsOfWork[type] = unitOfWork;

            return unitOfWork;
        }

        public void Complete()
        {
            // TODO:  Catch/Wrap/Re-throw DbUpdateConcurrencyException
            UnitsOfWork.Values.Each(u => u.SaveChanges());
        }

        public void Complete<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class
        {
            // TODO:  Catch/Wrap/Re-throw DbUpdateConcurrencyException
            UnitsOfWork.Values.Each(u => u.SaveChanges(response));
        }

        public Task<int>[] CompleteAsync()
        {
            var tasks = new List<Task<int>>();
            UnitsOfWork.Values.Each(u => tasks.Add(u.SaveChangesAsync()));
            return tasks.ToArray();
        }

        public Task<int>[] CompleteAsync<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class
        {
            var tasks = new List<Task<int>>();
            UnitsOfWork.Values.Each(u => tasks.Add(u.SaveChangesAsync(response)));
            return tasks.ToArray();
        }

        public void Dispose()
        {
            UnitsOfWork.Values.Each(u => u.Dispose());
        }

        public void Reset()
        {
            Dispose();
            UnitsOfWork.Clear();
        }
        #endregion
    }
}
