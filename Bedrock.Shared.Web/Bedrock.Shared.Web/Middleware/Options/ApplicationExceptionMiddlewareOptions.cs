namespace Bedrock.Shared.Web.Middleware.Options
{
    public class ApplicationExceptionMiddlewareOptions
    {
        public ApplicationExceptionMiddlewareOptions(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        #region Properties
        public string ErrorMessage { get; set; }
        #endregion
    }
}
