using System;
using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Security.Interface
{
    public interface IBedrockUser
    {
        #region Propeties
        int UserId { get; set; }

        string DisplayName { get; set; }

        string GivenName { get; set; }

        string Surname { get; set; }

        string JobTitle { get; set; }

        string[] Emails { get; set; }

        string StreetAddress { get; set; }

        string City { get; set; }

        string State { get; set; }

        string PostalCode { get; set; }

        string Country { get; set; }

        UserType UserType { get; set; }

        Guid GlobalKey { get; set; }
        #endregion
    }
}
