using System;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NonValidated : Attribute { }
}
