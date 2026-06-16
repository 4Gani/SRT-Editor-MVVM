using SRTEditor_MVVM.Model;
using static SRTEditor_MVVM.Services.ToolKit.ToolKitEnums;

namespace SRTEditor_MVVM.Services
{
    public interface ISrtRepository
    {
        Srt GetSrt();
        Srt AddSrt(Srt file);
        Srt UpdateSrt(Srt file);
        void UpdateSrtProperty(PropertyNames propertyName, string propertyValue);
        void DeleteSrt();
    }
}
