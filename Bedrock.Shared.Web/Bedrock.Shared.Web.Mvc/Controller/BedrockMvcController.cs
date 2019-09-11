using Bedrock.Shared.Session.Interface;
using Bedrock.Shared.Web.Controller;

using Http = Microsoft.AspNetCore.Http;

namespace Bedrock.Shared.Web.Mvc.Controller
{
    public class BedrockMvcController : ControllerBase
    {
        #region Constructors
        protected BedrockMvcController() { }

        protected BedrockMvcController(Http.IHttpContextAccessor httpContextAccessor, ISession session, params ISessionAware[] sessionAwareDependencies) : base(httpContextAccessor, session, sessionAwareDependencies) { }
        #endregion

        #region Public Properties
        #endregion
    }
}
