namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationRule
    {
        #region Methods
        IValidationResult Execute(object target);
        #endregion
    }
}
