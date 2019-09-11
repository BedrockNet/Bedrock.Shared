namespace Bedrock.Shared.Security.Interface
{
    public interface IClaimType
    {
        string DisplayName { get; }

        string GivenName { get; }

        string Surname { get; }

        string JobTitle { get; }

        string Emails { get; }

        string StreetAddress { get; }

        string City { get; }

        string State { get; }

        string PostalCode { get; }

        string Country { get; }

        string Subject { get; }

        string ScopeClaimType { get; }
    }
}
