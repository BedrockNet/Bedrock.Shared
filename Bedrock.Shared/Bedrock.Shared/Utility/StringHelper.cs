using System;
using Bedrock.Shared.Enumeration.StringHelper;

namespace Bedrock.Shared.Utility
{
    public class StringHelper : Singleton<StringHelper>
    {
        #region Public Methods
        public string Lookup<T>(T key, params string[] args) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            var returnValue = string.Empty;
            var lookup = (object)key;

            TypeSwitch.On<T>
            (
                TypeSwitch.Case<StringApplication>(() =>
                {
                    returnValue = Lookup((StringApplication)lookup, args);
                }),
                TypeSwitch.Case<StringHtml>(() =>
                {
                    returnValue = Lookup((StringHtml)lookup, args);
                }),
                TypeSwitch.Case<StringMediaType>(() =>
                {
                    returnValue = Lookup((StringMediaType)lookup, args);
                }),
                TypeSwitch.Case<StringCharacter>(() =>
                {
                    returnValue = Lookup((StringCharacter)lookup, args);
                }),
                TypeSwitch.Case<StringAbbreviation>(() =>
                {
                    returnValue = Lookup((StringAbbreviation)lookup, args);
                }),
                TypeSwitch.Case<StringInformation>(() =>
                {
                    returnValue = Lookup((StringInformation)lookup, args);
                }),
                TypeSwitch.Case<StringError>(() =>
                {
                    returnValue = Lookup((StringError)lookup, args);
                }),
                TypeSwitch.Case<StringCacheKey>(() =>
                {
                    returnValue = Lookup((StringCacheKey)lookup, args);
                }),
                TypeSwitch.Case<StringSecurity>(() =>
                {
                    returnValue = Lookup((StringSecurity)lookup, args);
                }),
                TypeSwitch.Case<StringClaimType>(() =>
                {
                    returnValue = Lookup((StringClaimType)lookup, args);
                })
            );

            return returnValue;
        }
        #endregion

        #region Private Methods
        private string Lookup(StringApplication key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringApplication.EmailStripCharacters:
                    {
                        returnValue = "<(.|)*?>";
                        break;
                    }

                case StringApplication.Controller:
                    {
                        returnValue = "controller";
                        break;
                    }

                case StringApplication.Action:
                    {
                        returnValue = "action";
                        break;
                    }

                case StringApplication.SessionUser:
                    {
                        returnValue = "SessionUser";
                        break;
                    }

                case StringApplication.SessionVariableForwardedFor:
                    {
                        returnValue = "HTTP_X_FORWARDED_FOR";
                        break;
                    }

                case StringApplication.SessionVariableRemoteAddress:
                    {
                        returnValue = "REMOTE_ADDR";
                        break;
                    }

                case StringApplication.EntityFrameworkProxyNamespace:
                    {
                        returnValue = "System.Data.Entity.DynamicProxies";
                        break;
                    }

                case StringApplication.ServiceSuffix:
                    {
                        returnValue = "Service";
                        break;
                    }

                case StringApplication.DefaultRouteName:
                    {
                        returnValue = "DefaultApi";
                        break;
                    }

                case StringApplication.TransactionKey:
                    {
                        returnValue = "Transaction-for-TransactionAttribute";
                        break;
                    }

                case StringApplication.ValidationResultFormat:
                    {
                        returnValue = "{1}. {0}{2}";
                        break;
                    }

                case StringApplication.FieldValidationResults:
                    {
                        returnValue = string.Format("Field validation results:  {0}", args[0]);
                        break;
                    }

                case StringApplication.RuleValidationResults:
                    {
                        returnValue = string.Format("Rule validation results:  {0}", args[0]);
                        break;
                    }

                case StringApplication.EntityFrameworkTableAlias:
                    {
                        returnValue = @"(?<tableAlias>AS \[Extent\d+\](?! WITH \(NOLOCK\)))";
                        break;
                    }

                case StringApplication.EntityFrameworkTableAliasNoLock:
                    {
                        returnValue = "${tableAlias} WITH (NOLOCK)";
                        break;
                    }

                case StringApplication.MSLoggedByKey:
                    {
                        returnValue = "MS_LoggedBy";
                        break;
                    }

                case StringApplication.Error:
                    {
                        returnValue = "error";
                        break;
                    }

                case StringApplication.Everyone:
                    {
                        returnValue = "Everyone";
                        break;
                    }

                case StringApplication.Private:
                    {
                        returnValue = "private";
                        break;
                    }

                case StringApplication.TimerKey:
                    {
                        returnValue = "Timer";
                        break;
                    }

                case StringApplication.ViewPathFormat:
                    {
                        returnValue = "~/Views/{1}/{0}.cshtml";
                        break;
                    }

                case StringApplication.ViewPathFormatShared:
                    {
                        returnValue = "~/Views/Shared/{0}.cshtml";
                        break;
                    }

                case StringApplication.ExceptionMessage:
                    {
                        returnValue = "Exception Message";
                        break;
                    }

                case StringApplication.MethodName:
                    {
                        returnValue = "Method Name";
                        break;
                    }

                case StringApplication.NameFormat:
                    {
                        returnValue = string.Format("{0}, {1}", args);
                        break;
                    }

                case StringApplication.True:
                    {
                        returnValue = "true";
                        break;
                    }

                case StringApplication.False:
                    {
                        returnValue = "false";
                        break;
                    }

                case StringApplication.All:
                    {
                        returnValue = "All";
                        break;
                    }

                case StringApplication.MSHttpContext:
                    {
                        returnValue = "MS_HttpContext";
                        break;
                    }

                case StringApplication.CurrencyUSDollars:
                    {
                        returnValue = "usd";
                        break;
                    }

                case StringApplication.DefaultDateSqlServer:
                    {
                        returnValue = "1/1/1900 12:00:00 AM";
                        break;
                    }
                case StringApplication.ReasonPhrase:
                    {
                        returnValue = "Reason Phrase";
                        break;
                    }
            }

            return returnValue;
        }

        private string Lookup(StringHtml key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringHtml.BreakTag:
                    {
                        returnValue = "<br />";
                        break;
                    }

                case StringHtml.ForwardSlashes:
                    {
                        returnValue = "//";
                        break;
                    }

                case StringHtml.ImageElementName:
                    {
                        returnValue = "img";
                        break;
                    }

                case StringHtml.SourcePropertyName:
                    {
                        returnValue = "src";
                        break;
                    }

                case StringHtml.LinkElementName:
                    {
                        returnValue = "a";
                        break;
                    }

                case StringHtml.LinkReferenceProperty:
                    {
                        returnValue = "href";
                        break;
                    }
            }

            return returnValue;
        }

        private string Lookup(StringMediaType key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringMediaType.ApplicationJson:
                    {
                        returnValue = "application/json";
                        break;
                    }

                case StringMediaType.TextJson:
                    {
                        returnValue = "text/json";
                        break;
                    }

                case StringMediaType.ApplicationXml:
                    {
                        returnValue = "application/xml";
                        break;
                    }

                case StringMediaType.TextXml:
                    {
                        returnValue = "text/xml";
                        break;
                    }

                case StringMediaType.ApplicationEncoded:
                    {
                        returnValue = "application/x-www-form-urlencoded";
                        break;
                    }

                case StringMediaType.ApplicationJsonResponse:
                    {
                        returnValue = "application/json; charset=utf-8";
                        break;
                    }

                case StringMediaType.TextPlain:
                    {
                        returnValue = "text/plain";
                        break;
                    }

                case StringMediaType.TextHTML:
                    {
                        returnValue = "text/html";
                        break;
                    }

                case StringMediaType.ImageJpeg:
                    {
                        returnValue = "image/jpeg";
                        break;
                    }
            }

            return returnValue;
        }

        private string Lookup(StringCharacter key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringCharacter.Underscore:
                    {
                        returnValue = "_";
                        break;
                    }

                case StringCharacter.Space:
                    {
                        returnValue = " ";
                        break;
                    }

                case StringCharacter.Semicolon:
                    {
                        returnValue = ";";
                        break;
                    }

                case StringCharacter.Comma:
                    {
                        returnValue = ",";
                        break;
                    }

                case StringCharacter.Colon:
                    {
                        returnValue = ":";
                        break;
                    }

                case StringCharacter.SemicolonWithSpace:
                    {
                        returnValue = "; ";
                        break;
                    }

                case StringCharacter.Hyphen:
                    {
                        returnValue = "-";
                        break;
                    }

                case StringCharacter.Ampersand:
                    {
                        returnValue = "@";
                        break;
                    }

                case StringCharacter.HashTag:
                    {
                        returnValue = "#";
                        break;
                    }

                case StringCharacter.ForwardSlash:
                    {
                        returnValue = "/";
                        break;
                    }

                case StringCharacter.BackwardSlash:
                    {
                        returnValue = @"\";
                        break;
                    }

                case StringCharacter.Pipe:
                    {
                        returnValue = "|";
                        break;
                    }

                case StringCharacter.Money:
                    {
                        returnValue = "$";
                        break;
                    }

                case StringCharacter.Yes:
                    {
                        returnValue = "Y";
                        break;
                    }

                case StringCharacter.No:
                    {
                        returnValue = "N";
                        break;
                    }
            }

            return returnValue;
        }

        private string Lookup(StringAbbreviation key, params string[] args)
        {
            var returnValue = string.Empty;
            return returnValue;
        }

        private string Lookup(StringInformation key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringInformation.ProcessError:
                    {
                        returnValue = "Process error";
                        break;
                    }

                case StringInformation.ProcessComplete:
                    {
                        returnValue = "Process complete";
                        break;
                    }

                case StringInformation.ProcessStarted:
                    {
                        returnValue = "Process started";
                        break;
                    }
            }

            return returnValue;
        }

        private string Lookup(StringError key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringError.UnhandledException:
                    {
                        returnValue = "An unhandled exception has occurred";
                        break;
                    }

                case StringError.GenericException:
                    {
                        returnValue = string.Format("Something went wrong. Please contact the service desk.");
                        break;
                    }

                case StringError.InvalidFieldsOrRules:
                    {
                        returnValue = "Fields or Rules not valid";
                        break;
                    }

                case StringError.InvalidScheme:
                    {
                        returnValue = "Invalid Scheme. Expected HTTPS.";
                        break;
                    }

                case StringError.InvalidCertificate:
                    {
                        returnValue = "Invalid Certificate. Access Denied.";
                        break;
                    }

                case StringError.InvalidData:
                    {
                        returnValue = string.Format("Data invalid for {0} operation", args);
                        break;
                    }

                case StringError.EntityNotFound:
                    {
                        returnValue = string.Format("{0} not found by filter", args);
                        break;
                    }

                case StringError.DocumentNotFound:
                    {
                        returnValue = string.Format("{0} not found", args);
                        break;
                    }

                case StringError.InvalidDataFilter:
                    {
                        returnValue = "Invalid Data For Method";
                        break;
                    }

                case StringError.EntityNotFoundFilter:
                    {
                        returnValue = "Entity Not Found";
                        break;
                    }

                case StringError.KeyNotFoundFilter:
                    {
                        returnValue = "Ket Not Found In Collection";
                        break;
                    }

                case StringError.NotImplementedFilter:
                    {
                        returnValue = "Method Not Implemented";
                        break;
                    }

                case StringError.SecurityFilter:
                    {
                        returnValue = "Security Violation";
                        break;
                    }

                case StringError.SqlFilter:
                    {
                        returnValue = "SQL Error";
                        break;
                    }

                case StringError.IncompleteTransaction:
                    {
                        returnValue = "Unable to complete transaction.";
                        break;
                    }
                case StringError.ContractMalformed:
                    {
                        returnValue = "Contract is Malformed";
                        break;
                    }
                case StringError.ResultIsEmpty:
                    {
                        returnValue = "Result is Empty";
                        break;
                    }
                case StringError.RuleEvaluationFailed:
                    {
                        returnValue = "Rule Evaluation Failed";
                        break;
                    }
            }

            return returnValue;
        }

        private string Lookup(StringCacheKey key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringCacheKey.UserPermissions:
                    {
                        returnValue = string.Format("bedrock:{1}:permissions:{0}", args);
                        break;
                    }
            }

            return returnValue;
        }

        private string Lookup(StringSecurity key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringSecurity.NoAuthorizationManagerSet:
                    {
                        returnValue = "No AuthorizationManager set";
                        break;
                    }
                case StringSecurity.ResourceType:
                    {
                        returnValue = "ResourceType";
                        break;
                    }
            }

            return returnValue;
        }

        private string Lookup(StringClaimType key, params string[] args)
        {
            var returnValue = string.Empty;

            switch (key)
            {
                case StringClaimType.Subject:
                    {
                        returnValue = "sub";
                        break;
                    }
                case StringClaimType.Name:
                    {
                        returnValue = "name";
                        break;
                    }
                case StringClaimType.GivenName:
                    {
                        returnValue = "given_name";
                        break;
                    }
                case StringClaimType.FamilyName:
                    {
                        returnValue = "family_name";
                        break;
                    }
                case StringClaimType.MiddleName:
                    {
                        returnValue = "middle_name";
                        break;
                    }
                case StringClaimType.NickName:
                    {
                        returnValue = "nickname";
                        break;
                    }
                case StringClaimType.PreferredUserName:
                    {
                        returnValue = "preferred_username";
                        break;
                    }
                case StringClaimType.Profile:
                    {
                        returnValue = "profile";
                        break;
                    }
                case StringClaimType.Picture:
                    {
                        returnValue = "picture";
                        break;
                    }
                case StringClaimType.WebSite:
                    {
                        returnValue = "website";
                        break;
                    }
                case StringClaimType.Email:
                    {
                        returnValue = "email";
                        break;
                    }
                case StringClaimType.EmailVerified:
                    {
                        returnValue = "email_verified";
                        break;
                    }
                case StringClaimType.Gender:
                    {
                        returnValue = "gender";
                        break;
                    }
                case StringClaimType.BirthDate:
                    {
                        returnValue = "birthdate";
                        break;
                    }
                case StringClaimType.ZoneInfo:
                    {
                        returnValue = "zoneinfo";
                        break;
                    }
                case StringClaimType.Locale:
                    {
                        returnValue = "locale";
                        break;
                    }
                case StringClaimType.PhoneNumber:
                    {
                        returnValue = "phone_number";
                        break;
                    }
                case StringClaimType.PhoneNumberVerified:
                    {
                        returnValue = "phone_number_verified";
                        break;
                    }
                case StringClaimType.Address:
                    {
                        returnValue = "address";
                        break;
                    }
                case StringClaimType.Audience:
                    {
                        returnValue = "aud";
                        break;
                    }
                case StringClaimType.Issuer:
                    {
                        returnValue = "iss";
                        break;
                    }
                case StringClaimType.NotBefore:
                    {
                        returnValue = "nbf";
                        break;
                    }
                case StringClaimType.Expiration:
                    {
                        returnValue = "exp";
                        break;
                    }
                case StringClaimType.UpdatedAt:
                    {
                        returnValue = "updated_at";
                        break;
                    }
                case StringClaimType.IssuedAt:
                    {
                        returnValue = "iat";
                        break;
                    }
                case StringClaimType.AuthenticationMethod:
                    {
                        returnValue = "amr";
                        break;
                    }
                case StringClaimType.SessionId:
                    {
                        returnValue = "sid";
                        break;
                    }
                case StringClaimType.AuthenticationContextClassReference:
                    {
                        returnValue = "acr";
                        break;
                    }
                case StringClaimType.AuthenticationTime:
                    {
                        returnValue = "auth_time";
                        break;
                    }
                case StringClaimType.AuthorizedParty:
                    {
                        returnValue = "azp";
                        break;
                    }
                case StringClaimType.AccessTokenHash:
                    {
                        returnValue = "at_hash";
                        break;
                    }
                case StringClaimType.AuthorizationCodeHash:
                    {
                        returnValue = "c_hash";
                        break;
                    }
                case StringClaimType.Nonce:
                    {
                        returnValue = "nonce";
                        break;
                    }
                case StringClaimType.JwtId:
                    {
                        returnValue = "jti";
                        break;
                    }
                case StringClaimType.ClientId:
                    {
                        returnValue = "client_id";
                        break;
                    }
                case StringClaimType.Scope:
                    {
                        returnValue = "scope";
                        break;
                    }
                case StringClaimType.Id:
                    {
                        returnValue = "id";
                        break;
                    }
                case StringClaimType.Secret:
                    {
                        returnValue = "secret";
                        break;
                    }
                case StringClaimType.IdentityProvider:
                    {
                        returnValue = "idp";
                        break;
                    }
                case StringClaimType.Role:
                    {
                        returnValue = "role";
                        break;
                    }
                case StringClaimType.ReferenceTokenId:
                    {
                        returnValue = "reference_token_id";
                        break;
                    }
                case StringClaimType.AuthorizationReturnUrl:
                    {
                        returnValue = "authorization_return_url";
                        break;
                    }
                case StringClaimType.PartialLoginRestartUrl:
                    {
                        returnValue = "partial_login_restart_url";
                        break;
                    }
                case StringClaimType.PartialLoginReturnUrl:
                    {
                        returnValue = "partial_login_return_url";
                        break;
                    }
                case StringClaimType.ExternalProviderUserId:
                    {
                        returnValue = "external_provider_user_id";
                        break;
                    }
                case StringClaimType.PartialLoginResumeId:
                    {
                        returnValue = "partial_login_resume_id";
                        break;
                    }
                case StringClaimType.Tenant:
                    {
                        returnValue = "tenant";
                        break;
                    }
                case StringClaimType.PasswordExpiry:
                    {
                        returnValue = "password_expiry";
                        break;
                    }
                case StringClaimType.Permission:
                    {
                        returnValue = "permission";
                        break;
                    }
            }

            return returnValue;
        }
        #endregion
    }
}