using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Bedrock.Shared.Data.Validation.Interface;
using Bedrock.Shared.Entity.Interface;
using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Session.Interface;
using Bedrock.Shared.Utility;

using Bedrock.Shared.Web.Controller;
using Bedrock.Shared.Web.Exception;
using Bedrock.Shared.Web.Utility;

using Http = Microsoft.AspNetCore.Http;

namespace Bedrock.Shared.Web.Api.Controller
{
    public class BedrockApiController : ControllerBase
    {
        #region Constructors
        protected BedrockApiController() { }

        protected BedrockApiController(Http.IHttpContextAccessor httpContextAccessor, ISession session, params ISessionAware[] sessionAwareDependencies) : base(httpContextAccessor, session, sessionAwareDependencies) { }
        #endregion

        #region Public Properties
        #endregion

        #region Protected Methods (Ensure)
        protected void EnsureModel<T>(T model)
        {
            var message = StringHelper.Current.Lookup(StringError.ContractMalformed);

            if (model == null)
                throw new HttpException(HttpStatusCode.BadRequest, message);

            else if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest, message, GetErrorsFromModelState());
        }

        protected void EnsureResult<T>(T result)
        {
            var collection = result as IEnumerable;
            var message = StringHelper.Current.Lookup(StringError.ResultIsEmpty);

            if (result == null || (collection != null && !collection.GetEnumerator().MoveNext()))
                throw new HttpException(HttpStatusCode.BadRequest, message);
        }

        protected void EnsureValidity(IValidationState validationState, string message)
        {
            if (!validationState.IsValid)
            {
                var errors = validationState.AllResultDescriptions.Select(rd => rd);
                throw new HttpException(HttpStatusCode.BadRequest, message, errors);
            }
        }
        #endregion

        #region Protected Methods
        protected string GetCreatedLocation(IBedrockIdEntity<int> entity, string routeName = null)
        {
            EnsureDefaultRouteName(ref routeName);
            return GetCreatedLocation<int>(entity, routeName);
        }

        protected string GetCreatedLocation<T>(IBedrockIdEntity<T> entity, string routeName = null)
            where T : struct, IComparable
        {
            EnsureDefaultRouteName(ref routeName);
            return Url.Link(routeName, new { id = entity.Id });
        }

        protected string GetDefaultRouteName()
        {
            string returnValue = null;
            EnsureDefaultRouteName(ref returnValue);
            return returnValue;
        }
        #endregion

        #region Private Methods
        private void EnsureDefaultRouteName(ref string routeName)
        {
            routeName = !string.IsNullOrEmpty(routeName) ?
                            routeName :
                            StringHelper.Current.Lookup(StringApplication.DefaultRouteName);
        }

        private IEnumerable<ModelStateError> GetErrorsFromModelState()
        {
            return ModelState
                    .Where(s => s.Value.Errors.Count > 0)
                    .Select(s => new ModelStateError
                    {
                        Key = s.Key,
                        Value = s.Value.Errors.First().ErrorMessage
                    });
        }
        #endregion
    }
}