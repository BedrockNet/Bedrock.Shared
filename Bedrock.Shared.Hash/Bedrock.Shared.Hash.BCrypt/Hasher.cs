using Bedrock.Shared.Configuration;
using Bedrock.Shared.Hash.Interface;

using BCryptNet = BCrypt.Net;

namespace Bedrock.Shared.Hash.BCrypt
{
    public class Hasher : HasherBase, IHasher
    {
		#region Constructors
		public Hasher(BedrockConfiguration bedrockConfiguration) : base(bedrockConfiguration) { }
		#endregion

		#region IHasher Methods
		public override string GenerateSalt()
        {
            return BCryptNet.BCrypt.GenerateSalt();
        }

        public override string GenerateSalt(int? workFactor)
        {
            if (!workFactor.HasValue)
                workFactor = BedrockConfiguration.Hash.WorkFactor;

            return BCryptNet.BCrypt.GenerateSalt(workFactor.Value);
        }

        public override string HashPassword(string input)
        {
            return BCryptNet.BCrypt.HashPassword(input);
        }

        public override string HashPassword(string input, int? workFactor)
        {
            if (!workFactor.HasValue)
                workFactor = BedrockConfiguration.Hash.WorkFactor;

            return BCryptNet.BCrypt.HashPassword(input, workFactor.Value);
        }

        public override string HashPassword(string input, string salt)
        {
            return BCryptNet.BCrypt.HashPassword(input, salt);
        }

        public override string HashString(string source)
        {
            return BCryptNet.BCrypt.HashString(source);
        }

        public override string HashString(string source, int? workFactor)
        {
            if (!workFactor.HasValue)
                workFactor = BedrockConfiguration.Hash.WorkFactor;

            return BCryptNet.BCrypt.HashString(source, workFactor.Value);
        }

        public override bool Verify(string text, string hash)
        {
            return BCryptNet.BCrypt.Verify(text, hash);
        }
        #endregion
    }
}
