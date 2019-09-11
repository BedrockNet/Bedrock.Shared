using System.ComponentModel.DataAnnotations;
using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Extension
{
    public static class EntityExtension
    {
        #region Public Methods
        public static void AddValidationResult(this IValidatableEntity entity, string message, params string[] memberNames)
        {
            entity.ValidationResults.Add(new ValidationResult(message, memberNames));
        }
        #endregion
    }
}
