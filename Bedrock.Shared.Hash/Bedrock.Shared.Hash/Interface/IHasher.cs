using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Hash.Interface
{
    public interface IHasher
    {
        #region Methods
        string GenerateSalt();

        string GenerateSalt(int? workFactor);

        string HashPassword(string input);

        string HashPassword(string input, int? workFactor);

        string HashPassword(string input, string salt);

        string HashString(string source);

        string HashString(string source, int? workFactor);

        bool Verify(string text, string hash);

        string HashAndShift(string input, string salt);

        string HashAndShift(string input, string salt, ShiftDirection shiftDirection, int shiftFactor, int shiftOffset, bool killSalt);

        bool VerifyHashAndShifted(string input, string salt, string hash);

        bool VerifyHashAndShifted(string input, string salt, string hash, ShiftDirection shiftDirection, int shiftFactor, int shiftOffset, bool killSalt);
        #endregion
    }
}
