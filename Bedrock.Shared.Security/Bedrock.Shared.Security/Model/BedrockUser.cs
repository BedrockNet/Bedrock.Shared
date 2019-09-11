using System;
using System.Linq;
using System.Security.Claims;

using Bedrock.Shared.Enumeration;
using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.Model
{
    public class BedrockUser : IBedrockUser
    {
        #region Public Properies
        public static IBedrockUser Default => new BedrockUser
        {
            UserId = -1,
            DisplayName = UserType.System.ToString(),
            UserType = UserType.System,
            GlobalKey = Guid.Parse("62224259-2995-47C2-86AC-2F611D989A19")
        };
        #endregion

        #region IBedrockUser Properties
        public int UserId { get; set; }

        public string DisplayName { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string JobTitle { get; set; }

        public string[] Emails { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public UserType UserType { get; set; }

        public Guid GlobalKey { get; set; }
        #endregion

        #region Public Methods
        public static IBedrockUser CreateFromPrincipal(ClaimsPrincipal claimsPrincipal, IClaimType claimType)
        {
            var globalKey = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.Subject)?.Value;

            return new BedrockUser
            {
                DisplayName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.DisplayName)?.Value,
                GivenName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.GivenName)?.Value,
                Surname = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.Surname)?.Value,
                JobTitle = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.JobTitle)?.Value,
                Emails = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.Emails)?.Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                StreetAddress = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.StreetAddress)?.Value,
                City = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.City)?.Value,
                State = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.State)?.Value,
                PostalCode = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.PostalCode)?.Value,
                Country = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType.Country)?.Value,
                UserType = UserType.User,
                GlobalKey = !string.IsNullOrWhiteSpace(globalKey) ? new Guid(globalKey) : Guid.Empty
            };
        }
        #endregion
    }
}
