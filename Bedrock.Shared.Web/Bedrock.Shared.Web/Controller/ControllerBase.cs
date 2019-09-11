using System.Linq;
using System.Reflection;

using Bedrock.Shared.Extension;
using Bedrock.Shared.Log.Interface;
using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Session.Interface;

using Http = Microsoft.AspNetCore.Http;
using Mvc = Microsoft.AspNetCore.Mvc;

namespace Bedrock.Shared.Web.Controller
{
    public class ControllerBase : Mvc.Controller
    {
        #region Constructors
        protected ControllerBase() { }

        protected ControllerBase(Http.IHttpContextAccessor httpContextAccessor, ISession session, params ISessionAware[] sessionAwareDependencies)
        {
            HttpContextAccessor = httpContextAccessor;
            CurrentSession = session;
            SetSessionUser();

            sessionAwareDependencies.Each(sad => sad.Enlist(session));
            SetServiceProperties(sessionAwareDependencies);
        }
        #endregion

        #region Public Properties
        public Http.IHttpContextAccessor HttpContextAccessor { get; private set; }

        public ISession CurrentSession { get; private set; }

        public ILogger Logger { get; set; }
        #endregion

        #region Protected Methods
        protected void SetSessionUser()
        {
            var principal = HttpContextAccessor.HttpContext.User as BedrockPrincipal;

            if(principal != null && principal.BedrockUser != null)
                CurrentSession.SetUser(principal.BedrockUser, principal);
        }

        protected void SetServiceProperties(ISessionAware[] dependencies)
        {
            if (dependencies == null || !dependencies.Any())
                return;

            var flags = BindingFlags.Instance | BindingFlags.NonPublic;

            GetType()
                .GetProperties(flags)
                .Where(p => typeof(ISessionAware).IsAssignableFrom(p.PropertyType))
                .Each(p =>
                {
                    var serviceValue = dependencies
                                        .FirstOrDefault(d => p.PropertyType.IsAssignableFrom(d.GetType()));

                    if (serviceValue != null)
                        p.SetValue(this, serviceValue);
                });
        }
        #endregion
    }
}
