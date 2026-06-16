using System.IO;
using System.Text;

namespace SRTEditor_MVVM.Services.ToolKit
{
    /// <summary>
    /// This class holds methods for reading and writing srt files
    /// </summary>
    public class ReadWrite : IReadWrite
    {
        /// <summary>
        /// All class's Methods define here
        /// </summary>
        #region Methods

        /// <summary>
        /// Read a SRT file with automatic encoding detection.
        /// Falls back to UTF-8 if encoding cannot be determined.
        /// </summary>
        /// <param name="fileAddress">File address</param>
        /// <returns>Return a StreamReader object</returns>
        public StreamReader ReadFile(string fileAddress)
        {
            // detectEncodingFromByteOrderMarks: true — automatically detects UTF-8, UTF-16, etc.
            // If no BOM exists, falls back to UTF-8 which covers most modern Persian SRT files
            return new StreamReader(fileAddress, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        }

        /// <summary>
        /// Write a SRT file
        /// </summary>
        /// <param name="saveLocation">Save location</param>
        /// <returns>Return a TextWriter object</returns>
        public TextWriter WriteFile(string saveLocation)
        {
            TextWriter newSrtFile;

            // checking to determine that file must be overwritten
            // or save in a new file
            if (AppSettings.IsOverWritable)
            {
                newSrtFile = new StreamWriter(Path.Combine(AppSettings.Directory, "temp_Sub.srt"), false, Encoding.UTF8);
            }

            else  // action for save in a new file
            {
                newSrtFile = new StreamWriter(saveLocation, false, Encoding.UTF8);
            }

            return newSrtFile;
        }

        /// <summary>
        /// Replace original file with edited file
        /// </summary>
        /// <param name="fileAddress">File address</param>
        public void ReplaceFile(string fileAddress)
        {
            File.Copy(Path.Combine(AppSettings.Directory, "temp_Sub.srt"), fileAddress, true);
            File.Delete(Path.Combine(AppSettings.Directory, "temp_Sub.srt"));
        }
        #endregion
    }
}
