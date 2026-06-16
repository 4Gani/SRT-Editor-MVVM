using Microsoft.Extensions.DependencyInjection;
using SRTEditor_MVVM.Components.MessageBox;
using SRTEditor_MVVM.Infrastructure;
using SRTEditor_MVVM.Services;
using static SRTEditor_MVVM.Services.ToolKit.ToolKitEnums;

namespace SRTEditor_MVVM.Validation
{
    /// <summary>
    /// Validates that both the input SRT file and the output save location are properly
    /// configured before any processing operation is performed.
    /// </summary>
    public class InputOutputValidation
    {
        public InputOutputValidation()
        {
            _repo = ContainerHelper.Container.GetRequiredService<ISrtRepository>();
        }

        private readonly ISrtRepository _repo;

        /// <summary>
        /// Checks that the input file address and output save location are both set and valid.
        /// Displays a warning message and returns false if any condition is not met.
        /// When overwrite mode is active, the save location is automatically set to the input path.
        /// </summary>
        /// <returns>True if all conditions are satisfied and processing can proceed.</returns>
        public bool IsInputAndOutputInitialized()
        {
            // Ensure an input file has been selected
            if (string.IsNullOrEmpty(_repo.GetSrt().SrtFileAddress))
            {
                MessageBoxHelper.Show("Please select an SRT file", "Warning",
                    MessageButton.OK, MessageIcon.Warning);
                return false;
            }

            // In overwrite mode, the output path is the same as the input path
            if (AppSettings.IsOverWritable)
            {
                _repo.UpdateSrtProperty(PropertyNames.SrtSaveLocation, _repo.GetSrt().SrtFileAddress);
                return true;
            }

            // Ensure a separate save location has been selected
            if (string.IsNullOrEmpty(_repo.GetSrt().SrtSaveLocation))
            {
                MessageBoxHelper.Show("Please select a location to save the output file", "Warning",
                    MessageButton.OK, MessageIcon.Warning);
                return false;
            }

            // Prevent the output from overwriting the input when not in overwrite mode
            if (_repo.GetSrt().SrtSaveLocation == _repo.GetSrt().SrtFileAddress)
            {
                MessageBoxHelper.Show("The save location cannot be the same as the source file", "Warning",
                    MessageButton.OK, MessageIcon.Warning);
                return false;
            }

            return true;
        }
    }
}