namespace Bedrock.Shared.Enumeration.StringHelper
{
    public enum StringError
    {
        UnhandledException = 1,
        GenericException = 2,
        InvalidFieldsOrRules = 3,
        InvalidScheme = 4,
        InvalidCertificate = 5,
        InvalidData = 6,
        EntityNotFound = 7,
        InvalidDataFilter = 8,
        EntityNotFoundFilter = 9,
        KeyNotFoundFilter = 10,
        NotImplementedFilter = 11,
        SecurityFilter = 12,
        SqlFilter = 13,
        DocumentNotFound = 14,
        IncompleteTransaction = 15,
        ContractMalformed = 16,
        ResultIsEmpty = 17,
        RuleEvaluationFailed = 18
    }
}
