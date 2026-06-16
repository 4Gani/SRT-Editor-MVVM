using SRTEditor_MVVM.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SRTEditor_MVVM.Infrastructure
{
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {

        private readonly Dictionary<string, List<string>> _errors = [];
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged = delegate { };

        // CS8766 is suppressed here because INotifyDataErrorInfo.GetErrors is a legacy interface
        // that predates nullable reference types and declares a non-nullable return type.
        // Returning null when no errors exist is the correct and expected behavior,
        // so the nullable return type is intentional and cannot be avoided without violating the contract.
        #pragma warning disable CS8766
        public System.Collections.IEnumerable? GetErrors(string? propertyName)
        {
            if (propertyName != null && _errors.TryGetValue(propertyName, out var errors))
                return errors;
            return null;
        }
        #pragma warning restore CS8766

        public bool HasErrors => _errors.Count != 0;

        public bool ValidateProperty(string validationRuleName, string propertyName, string value)
        {
            // Get validation rules
            Dictionary<string, IValidator> rulesList = ValidationRules.ValidationRulesList;

            ValidationResult? tempResult = null;

            if (rulesList.TryGetValue(validationRuleName, out IValidator? rule))
                tempResult = rule.Validate(value);

            List<ValidationResult> results = [];
            if (tempResult != null)
                results.Add(tempResult);

            return ApplyValidationResults(propertyName, results);
        }

        public bool AnnotationValidateProperty<T>(string propertyName, T value)
        {
            List<ValidationResult> results = [];
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(value, context, results);
            return ApplyValidationResults(propertyName, results);
        }

        private bool ApplyValidationResults(string propertyName, List<ValidationResult> results)
        {
            if (results.Count != 0)
            {
                _errors[propertyName] = results.ConvertAll(c => c.ErrorMessage!);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                return false;
            }

            _errors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            return true;
        }
    }
}
