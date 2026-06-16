using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SRTEditor_MVVM.Validation
{
    /// <summary>
    /// Validates that a file path points to an existing SRT file with a rooted, well-formed path.
    /// Used for validating the input file address.
    /// </summary>
    public class FullAddressValidation : IValidator
    {
        /// <summary>
        /// Validates the given file path. An empty string is considered valid (no file selected).
        /// Returns null on success, or a <see cref="ValidationResult"/> describing the error.
        /// </summary>
        /// <param name="value">The file path to validate.</param>
        public ValidationResult? Validate(string value)
        {
            // An empty value is acceptable — it means no file has been selected yet
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;

            try
            {
                Path.GetFullPath(value);

                if (!Path.IsPathRooted(value))
                    return new ValidationResult("File address is not valid.");

                if (!Equals(Path.GetExtension(value), ".srt"))
                    return new ValidationResult("File is not an SRT file.");

                if (!File.Exists(value))
                    return new ValidationResult("File does not exist.");

                return ValidationResult.Success;
            }
            catch (Exception)
            {
                return new ValidationResult("File address is not valid.");
            }
        }
    }
}