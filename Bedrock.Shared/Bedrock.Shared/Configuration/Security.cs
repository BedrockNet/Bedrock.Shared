using System;
using System.Collections.Generic;

using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Configuration
{
	public class Security
    {
        #region Public Properties
        public SecurityApplication Application { get; set; }
        #endregion
    }

    public class SecurityApplication
    {
        #region Public Properties
        public Authorization Authorization { get; set; }

        public ClaimCollection ClaimCollection { get; set; }

        public AzureAdB2C AzureAdB2C { get; set; }

        public SwaggerAdB2C SwaggerAdB2C { get; set; }
        #endregion
    }

    public class Authorization
    {
        #region Public Properties
        public int Timeout { get; set; }
        #endregion
    }

    public class ClaimCollection
    {
        private List<string> _applications;

        #region Public Properties
        public List<string> Applications
        {
            get
            {
                _applications = _applications ?? new List<string>();
                return _applications;
            }
            set { _applications = value; }
        }

        public string ConnectionKey { get; set; }

        public bool IsCacheEnabled { get; set; }

        public CacheType CacheType { get; set; }

        public int CacheExpiry { get; set; }
        #endregion
    }

    public class AzureAdB2C
    {
        public string InstanceUrl { get; set; }

        public string Tenant { get; set; }

        public string ApplicationId { get; set; }

        public string AuthorizeUri { get; set; }

        public string TokenUri { get; set; }

        public string ClientId { get; set; }

        public string Policy { get; set; }

        public string ScopeRead { get; set; }

        public string ScopeWrite { get; set; }

        public string ScopeImpersonation { get; set; }

        public string ApiVersion { get; set; }
    }

    public class SwaggerAdB2C
    {
        #region Public Properties
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        #endregion
    }
}
