using System;
using System.Linq.Expressions;

namespace Bedrock.Shared.Utility
{
    /// <summary>
    /// Efficient way for newing up instance of type T
    /// Example:  var stringHelper = New<StringHelper>.Instance;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class New<T> where T : new()
    {
		#region Properties
		public static readonly Func<T> Instance = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
		#endregion
	}
}
