﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Bedrock.Shared.Security.Identity
{
    public static class Principal
    {
        #region Properties
        public static ClaimsPrincipal Anonymous
        {
            get { return new ClaimsPrincipal(Identity.Anonymous); }
        }
        #endregion

        #region Public Methods
        public static ClaimsPrincipal Create(string authenticationType, params Claim[] claims)
        {
            return new ClaimsPrincipal(Identity.Create(authenticationType, claims));
        }

        public static IEnumerable<Claim> CreateRoles(string[] roleNames)
        {
            if (roleNames == null || roleNames.Count() == 0)
                return new Claim[] { };

            return new List<Claim>(from r in roleNames select new Claim(ClaimTypes.Role, r)).ToArray();
        }

        public static ClaimsPrincipal CreateFromCertificate(X509Certificate2 certificate, string authenticationType = "X.509", bool includeAllClaims = false)
        {
            return new ClaimsPrincipal(Identity.CreateFromCertificate(certificate, authenticationType, includeAllClaims));
        }
        #endregion
    }
}
