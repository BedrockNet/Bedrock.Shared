using System;

namespace Bedrock.Shared.Utility
{
    public class NoopDisposable : IDisposable
    {
		#region IDisposable Members
		public void Dispose() { }
		#endregion
	}
}
