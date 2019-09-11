using Bedrock.Shared.Data.Validation.Enumeration;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationRequest
    {
        #region Properties
        bool IsFields { get; set; }

        bool IsRules { get; set; }

        bool IsRecursiveFields { get; set; }

        bool IsRecursiveCollectionFields { get; set; }

        bool IsHaltOnErrorFields { get; set; }

        bool IsRecursiveRules { get; set; }

        bool IsRecursiveCollectionRules { get; set; }

        bool IsHaltOnErrorRules { get; set; }

        bool ThrowExceptionOnError { get; set; }

        ValidationRequestType RequestType { get; }
        #endregion
    }
}
