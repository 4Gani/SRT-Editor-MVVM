using System.ComponentModel.DataAnnotations;

namespace SRTEditor_MVVM.Validation
{
    public interface IValidator
    {
        ValidationResult? Validate(string value);
    }
}