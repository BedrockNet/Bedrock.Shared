using System;
using System.Text;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Enumeration;
using Bedrock.Shared.Hash.Interface;

namespace Bedrock.Shared.Hash
{
    public abstract class HasherBase : IHasher
    {
		#region Constructors
		public HasherBase(BedrockConfiguration bedrockConfiguration)
		{
			BedrockConfiguration = bedrockConfiguration;
		}
		#endregion

		#region Protected Properties
		protected BedrockConfiguration BedrockConfiguration { get; set; }
		#endregion

		#region IHasher Methods
		public abstract string GenerateSalt();

        public abstract string GenerateSalt(int? workFactor = null);

        public abstract string HashPassword(string input);

        public abstract string HashPassword(string input, int? workFactor);

        public abstract string HashPassword(string input, string salt);

        public abstract string HashString(string source);

        public abstract string HashString(string source, int? workFactor);

        public abstract bool Verify(string text, string hash);

        public virtual string HashAndShift(string input, string salt)
        {
            var shiftDirection = BedrockConfiguration.Hash.ShiftDirection;
            var shiftFactor = BedrockConfiguration.Hash.ShiftFactor;
            var shiftOffset = BedrockConfiguration.Hash.ShiftOffset;
            var killSalt = BedrockConfiguration.Hash.IsKillSalt;

            return HashAndShift(input, salt, shiftDirection, shiftFactor, shiftOffset, killSalt);
        }

        public virtual string HashAndShift(string input, string salt, ShiftDirection shiftDirection, int shiftFactor, int shiftOffset, bool killSalt)
        {
            byte[] shiftedBytes;
            var hashedInput = HashPassword(input, salt);

            if (killSalt)
                hashedInput = hashedInput.TrimStart(salt.ToCharArray());

            var inputBytes = Encoding.UTF8.GetBytes(hashedInput);
            var byteToShift = GetByteToShift(inputBytes, shiftOffset, shiftDirection);

            switch (shiftDirection)
            {
                case ShiftDirection.Right:
                {
                    shiftedBytes = ShiftRight(inputBytes, byteToShift, shiftFactor);
                    break;
                }

                default:
                {
                    shiftedBytes = ShiftLeft(inputBytes, byteToShift, shiftFactor);
                    break;
                }
            }

            return Encoding.UTF8.GetString(shiftedBytes);
        }

        public virtual bool VerifyHashAndShifted(string input, string salt, string hash)
        {
            var shiftDirection = BedrockConfiguration.Hash.ShiftDirection;
            var shiftFactor = BedrockConfiguration.Hash.ShiftFactor;
            var shiftOffset = BedrockConfiguration.Hash.ShiftOffset;
            var killSalt = BedrockConfiguration.Hash.IsKillSalt;

            return VerifyHashAndShifted(input, salt, hash, shiftDirection, shiftFactor, shiftOffset, killSalt);
        }

        public virtual bool VerifyHashAndShifted(string input, string salt, string hash, ShiftDirection shiftDirection, int shiftFactor, int shiftOffset, bool killSalt)
        {
            return HashAndShift(input, salt, shiftDirection, shiftFactor, shiftOffset, killSalt) == hash;
        }
        #endregion

        #region Private Methods
        private int GetByteToShift(byte[] inputBytes, int shiftOffset, ShiftDirection shiftDirection)
        {
            var byteToShift = 0;
            var bytesLength = inputBytes.Length;

            shiftOffset = Math.Abs(shiftOffset);

            if (shiftOffset > bytesLength)
            {
                if (shiftDirection == ShiftDirection.Left)
                    byteToShift = bytesLength - 1;
            }
            else
            {
                if (shiftDirection == ShiftDirection.Left)
                    byteToShift = shiftOffset - 1;
                else
                    byteToShift = bytesLength - shiftOffset;
            }

            return byteToShift;
        }

        private byte[] ShiftLeft(byte[] bytes, int byteToShift, int shiftFactor)
        {
            bytes[byteToShift] = (byte)(bytes[byteToShift] << shiftFactor);
            return bytes;
        }

        private byte[] ShiftRight(byte[] bytes, int byteToShift, int shiftFactor)
        {
            bytes[byteToShift] = (byte)(bytes[byteToShift] >> shiftFactor);
            return bytes;
        }
        #endregion
    }
}
