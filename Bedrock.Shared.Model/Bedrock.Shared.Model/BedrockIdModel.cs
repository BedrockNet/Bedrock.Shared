using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Model
{
    public class BedrockIdModel<DM, M, K> : MapperModelIdBase<DM, M, K>, IBedrockIdEntity<K>, IBedrockEntity
        where DM : class, new()
        where M : BedrockIdModel<DM, M, K>, new()
        where K : IComparable
    { }
}
