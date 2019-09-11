using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Extension;

using Bedrock.Shared.Security.Extension;
using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.ResourceAuthorization
{
    public abstract class ResourceAuthorizationManagerBase : IResourceAuthorizationManager
    {
        #region Constructors
        public ResourceAuthorizationManagerBase(IClaimType claimType, IClaimCollectorFactory claimCollectorFactory, BedrockConfiguration bedrockConfiguration)
        {
            ClaimType = claimType;
            ClaimCollectorFactory = claimCollectorFactory;
            BedrockConfiguration = bedrockConfiguration;
        }
        #endregion

        #region Public Properties
        public string UsernameClaimType { get; set; }

        public string SubjectClaimType { get; set; }
        #endregion

        #region Protected Properties
        protected IClaimType ClaimType { get; set; }

        protected IClaimCollectorFactory ClaimCollectorFactory { get; set; }

        protected BedrockConfiguration BedrockConfiguration { get; set; }
        #endregion

        #region Public Methods
        public virtual Task<bool> CheckAccessAsync(IResourceAuthorizationContext context)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Protected Methods
        protected virtual Task<bool> Ok()
        {
            return Task.FromResult(true);
        }

        protected virtual Task<bool> Nok()
        {
            return Task.FromResult(false);
        }

        protected virtual Task<bool> Eval(bool val)
        {
            return Task.FromResult(val);
        }

        protected virtual void AddPermissionClaims(IResourceAuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated)
                return;

            var userClaims = new List<Claim>();
            var claimsApplications = BedrockConfiguration.Security.Application.ClaimCollection.Applications;

            var usernameClaimType = !string.IsNullOrWhiteSpace(UsernameClaimType) ? UsernameClaimType : ClaimType.DisplayName;
            var username = context.Principal.Claims.GetClaimByTypeFirstOrDefault(usernameClaimType)?.Value;

            var subjectClaimType = !string.IsNullOrWhiteSpace(SubjectClaimType) ? SubjectClaimType : ClaimType.Subject;
            var subject = context.Principal.Claims.GetClaimByTypeFirstOrDefault(subjectClaimType)?.Value;

            claimsApplications.Each(ua =>
            {
                var collector = ClaimCollectorFactory.CreateInstanceCollector(ua);
                var pass = ClaimCollectorFactory.CreateInstancePass(ua, collector, username, subject);

                pass
                    .Collector
                    .Collect(pass)
                    .Each(c => userClaims.Add(c));
            });

            userClaims.Each(c => context.Principal.Identities.First().AddClaim(c));
        }
        #endregion
    }
}
