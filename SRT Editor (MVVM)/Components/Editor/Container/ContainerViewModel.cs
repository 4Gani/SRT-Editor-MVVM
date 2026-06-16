using Microsoft.Extensions.DependencyInjection;
using SRTEditor_MVVM.Components.Editor.ImprovingReadability;
using SRTEditor_MVVM.Components.Editor.InputOutput;
using SRTEditor_MVVM.Components.Editor.Repairing;
using SRTEditor_MVVM.Components.Editor.TimeCorrecting;
using SRTEditor_MVVM.Components.Editor.TimeShifting;
using SRTEditor_MVVM.Infrastructure;

namespace SRTEditor_MVVM.Components.Editor.Container
{
    public class ContainerViewModel
    {
        public ContainerViewModel()
        {
            InputOutputViewModel = ContainerHelper.Container.GetRequiredService<InputOutputViewModel>();
            RepairingViewModel = ContainerHelper.Container.GetRequiredService<RepairingViewModel>();
            TimeShiftingViewModel = ContainerHelper.Container.GetRequiredService<TimeShiftingViewModel>();
            TimeCorrectingViewModel = ContainerHelper.Container.GetRequiredService<TimeCorrectingViewModel>();
            ImprovingReadabilityViewModel = ContainerHelper.Container.GetRequiredService<ImprovingReadabilityViewModel>();
        }


        /// <summary>
        /// All class's Fields and Properties define here
        /// </summary>
        #region Fields and Properties
        public InputOutputViewModel InputOutputViewModel { get; set; }
        public RepairingViewModel RepairingViewModel { get; set; }
        public TimeShiftingViewModel TimeShiftingViewModel { get; set; }
        public TimeCorrectingViewModel TimeCorrectingViewModel { get; set; }
        public ImprovingReadabilityViewModel ImprovingReadabilityViewModel { get; set; }
        #endregion
    }
}
