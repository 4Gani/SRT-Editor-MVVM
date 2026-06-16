using SRTEditor_MVVM.Model;
using static SRTEditor_MVVM.Services.ToolKit.ToolKitEnums;

namespace SRTEditor_MVVM.Services
{
    /// <summary>
    /// Acts as a bridge between ViewModels and the in-memory database.
    /// All SRT file state is read and written through this repository.
    /// </summary>
    public class SrtRepository : ISrtRepository
    {
        /// <summary>Returns the current SRT file from the database.</summary>
        public Srt GetSrt()
        {
            return SrtDataBase.CurrentFile;
        }

        /// <summary>Replaces the current SRT file in the database with the provided one.</summary>
        public Srt AddSrt(Srt file)
        {
            SrtDataBase.CurrentFile = file;
            return file;
        }

        /// <summary>Updates the current SRT file in the database.</summary>
        public Srt UpdateSrt(Srt file)
        {
            SrtDataBase.CurrentFile = file;
            return file;
        }

        /// <summary>
        /// Updates a single property of the current SRT file in the database.
        /// </summary>
        /// <param name="propertyName">The property to update.</param>
        /// <param name="propertyValue">The new value to assign.</param>
        public void UpdateSrtProperty(PropertyNames propertyName, string propertyValue)
        {
            switch (propertyName)
            {
                case PropertyNames.SrtFileAddress:
                    SrtDataBase.CurrentFile.SrtFileAddress = propertyValue;
                    break;
                case PropertyNames.SrtFileName:
                    SrtDataBase.CurrentFile.SrtFileName = propertyValue;
                    break;
                case PropertyNames.SrtSaveLocation:
                    SrtDataBase.CurrentFile.SrtSaveLocation = propertyValue;
                    break;
            }
        }

        /// <summary>Resets the current SRT file to an empty state.</summary>
        public void DeleteSrt()
        {
            SrtDataBase.CurrentFile = new Srt();
        }
    }
}