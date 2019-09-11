using System;

namespace Bedrock.Shared.Extension
{
    public static class LongExtension
    {
        #region Public Methods
        public static DateTime ConvertToDateTimeFromSeconds(this long epochTimeStampInSeconds)
        {
            var returnValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            returnValue = returnValue.AddSeconds(epochTimeStampInSeconds).ToLocalTime();
            return returnValue;
        }

        public static DateTime ConvertToDateTimeFromMilliseconds(this long epochTimeStampInMilliseconds)
        {
            var returnValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            returnValue = returnValue.AddMilliseconds(epochTimeStampInMilliseconds).ToLocalTime();
            return returnValue;
        }
        #endregion
    }
}
