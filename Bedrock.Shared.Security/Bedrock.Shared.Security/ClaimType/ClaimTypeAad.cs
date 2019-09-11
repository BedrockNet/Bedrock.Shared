using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.Constant
{
    public class ClaimTypeAad : IClaimType
    {
        public string DisplayName => "name";

        public string GivenName => "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";

        public string Surname => "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

        public string JobTitle => "jobTitle";

        public string Emails => "emails";

        public string StreetAddress => "streetAddress";

        public string City => "city";

        public string State => "state";

        public string PostalCode => "postalCode";

        public string Country => "country";

        public string Subject => "http://schemas.microsoft.com/identity/claims/objectidentifier";

        public string ScopeClaimType => "http://schemas.microsoft.com/identity/claims/scope";
    }
}
