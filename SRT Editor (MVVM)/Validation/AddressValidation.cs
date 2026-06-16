using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SRTEditor_MVVM.Validation
{
    /// <summary>
    /// Validates that a file path is a well-formed, rooted path pointing to an SRT file.
    /// Used for validating the output save location (file does not need to exist yet).
    /// </summary>
    public class AddressValidation : IValidator
    {
        /// <summary>
        /// Validates the given file path. An empty string is considered valid (no location selected yet).
        /// Returns null on success, or a <see cref="ValidationResult"/> describing the error.
        /// </summary>
        /// <param name="value">The file path to validate.</param>
        public ValidationResult? Validate(string value)
        {
            // An empty value is acceptable — it means no save location has been selected yet
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;

            try
            {
                Path.GetFullPath(value);

                if (!Path.IsPathRooted(value))
                    return new ValidationResult("File address is not valid.");

                if (!Equals(Path.GetExtension(value), ".srt"))
                    return new ValidationResult("File is not an SRT file.");

                return ValidationResult.Success;
            }
            catch (Exception)
            {
                return new ValidationResult("File address is not valid.");
            }
        }
    }
}