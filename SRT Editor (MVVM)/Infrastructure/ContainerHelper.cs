using Microsoft.Extensions.DependencyInjection;
using SRTEditor_MVVM.Components.Editor.ImprovingReadability;
using SRTEditor_MVVM.Components.Editor.InputOutput;
using SRTEditor_MVVM.Components.Editor.Repairing;
using SRTEditor_MVVM.Components.Editor.TimeCorrecting;
using SRTEditor_MVVM.Components.Editor.TimeShifting;
using SRTEditor_MVVM.Components.Viewer;
using SRTEditor_MVVM.Services;
using SRTEditor_MVVM.Services.ToolKit;

namespace SRTEditor_MVVM.Infrastructure
{
    /// <summary>
    /// Configures and provides the dependency injection container for the application.
    /// </summary>
    public static class ContainerHelper
    {
        private static readonly IServiceProvider _container;

        static ContainerHelper()
        {
            var services = new ServiceCollection();

            // Repositories
            services.AddSingleton<ISrtRepository, SrtRepository>();

            // Services
            services.AddTransient<SrtRepairer>();
            services.AddTransient<SrtTimeShifter>();
            services.AddTransient<SrtTimeCorrector>();
            services.AddTransient<SrtReadabilityImprover>();

            // ViewModels
            services.AddTransient<InputOutputViewModel>();
            services.AddTransient<RepairingViewModel>();
            services.AddTransient<TimeShiftingViewModel>();
            services.AddTransient<TimeCorrectingViewModel>();
            services.AddTransient<ImprovingReadabilityViewModel>();
            services.AddTransient<FileViewerViewModel>();

            // ToolKit
            services.AddTransient<IReadWrite, ReadWrite>();
            services.AddTransient<INumber, Number>();
            services.AddTransient<ITime, Time>();
            services.AddTransient<ILine, Line>();

            _container = services.BuildServiceProvider();
        }

        public static IServiceProvider Container => _container;
    }
}