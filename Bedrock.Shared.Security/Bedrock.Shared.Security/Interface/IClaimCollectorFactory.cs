namespace Bedrock.Shared.Security.Interface
{
    public interface IClaimCollectorFactory
    {
        #region Methods
        IClaimCollector CreateInstanceCollector(string application);

        ICollectPass CreateInstancePass(string application, IClaimCollector collector, string userId, string subjectId);
        #endregion
    }
}
