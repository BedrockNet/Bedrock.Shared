namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IPropertyDescriptor
    {
        #region Properties
        string Name { get; }
        #endregion

        #region Methods
        object GetValue(object target);
        #endregion
    }
}
