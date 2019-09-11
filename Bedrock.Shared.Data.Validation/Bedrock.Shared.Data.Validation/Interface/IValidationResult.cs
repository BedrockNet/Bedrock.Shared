using Bedrock.Shared.Data.Validation.Enumeration;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationResult
    {
        #region Properties
        IPropertyDescriptor Property { get; set; }

        ValidationResultSeverity Severity { get; set; }

        string Description { get; set; }

        ValidationResultType ResultType { get; set; }
        #endregion
    }
}
