namespace Bedrock.Shared.Data.Validation.Enumeration
{
    public enum ValidationRequestType
    {
        None = 1,
        Full = 2,
        FullHaltOnError = 3,
        FullHaltOnErrorFields = 4,
        FullHaltOnErrorRules = 5,
        FullNoRecursion = 6,
        FieldsRecursive = 7,
        FieldsHaltOnErrorRecursive = 8,
        FieldsNoCollectionRecursive = 9,
        FieldsNoCollectionHaltOnErrorRecursive = 10,
        FieldsNoRecursion = 11,
        RulesRecursive = 12,
        RulesHaltOnErrorRecursive = 13,
        RulesNoCollectionRecursive = 14,
        RulesNoCollectionHaltOnErrorRecursive = 15,
        RulesNoRecursion = 16
    }
}
