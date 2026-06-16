namespace SRTEditor_MVVM.Validation
{
    public static class ValidationRules
    {
        /** Register all validation rules **/
        static ValidationRules()
        {
            ValidationRulesList.Add("FullAddressValidation", new FullAddressValidation());
            ValidationRulesList.Add("AddressValidation", new AddressValidation());
        }

        public static Dictionary<string, IValidator> ValidationRulesList { get; set; } = [];
    }
}
