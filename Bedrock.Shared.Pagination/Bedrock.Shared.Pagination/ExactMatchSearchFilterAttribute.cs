using System;

namespace Bedrock.Shared.Pagination
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ExactMatchSearchFilterAttribute : Attribute { }
}
