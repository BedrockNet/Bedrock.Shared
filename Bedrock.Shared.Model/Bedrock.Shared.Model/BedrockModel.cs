using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Model
{
    public class BedrockModel<DM, M> : MapperModelBase<DM, M>, IBedrockEntity
        where DM : class, new()
        where M : BedrockModel<DM, M>, new()
    { }
}
