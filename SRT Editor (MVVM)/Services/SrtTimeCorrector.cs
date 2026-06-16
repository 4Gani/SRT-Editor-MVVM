using SRTEditor_MVVM.Services.ToolKit;
using System.IO;

namespace SRTEditor_MVVM.Services
{
    /// <summary>
    /// Corrects SRT file timestamps by calculating a proportional time offset
    /// across all subtitle entries, based on a user-supplied target end time.
    /// </summary>
    public class SrtTimeCorrector(SrtRepairer repairing, IReadWrite readWrite, INumber number, ITime time)
    {
        #region Fields and Properties
        private readonly SrtRepairer _repairing = repairing;
        private readonly IReadWrite _readWrite = readWrite;
        private readonly INumber _number = number;
        private readonly ITime _time = time;
        #endregion

        #region Methods

        /// <summary>
        /// Corrects the timestamps in the specified SRT file so that the last subtitle
        /// ends at the given target time. Intermediate timestamps are adjusted proportionally.
        /// Uses a two-pass approach: the first pass collects metadata, the second applies corrections.
        /// </summary>
        /// <param name="fileAddress">Path to the source SRT file.</param>
        /// <param name="saveLocation">Path where the corrected file will be saved.</param>
        /// <param name="correctingTime">The target end time in HH:MM:SS format.</param>
        public void CorrectTime(string fileAddress, string saveLocation, string correctingTime)
        {
            string tempPath = Path.Combine(AppSettings.Directory, "temp_Sub(corrected).srt");

            // Repair the file first and save it to a temporary location
            _repairing.RepairFile(fileAddress, tempPath);

            // Parse the user-supplied target end time
            TimeSpan correctTime = _time.GetTime(correctingTime);

            // --- First pass: collect the last line number and last time line ---
            int lastNumberLine = 1;
            string lastTimeLine = "";

            using (StreamReader srtFile = _readWrite.ReadFile(tempPath))
            {
                string line;
                while (!srtFile.EndOfStream)
                {
                    line = srtFile.ReadLine() ?? string.Empty;

                    if (_number.IsThisLineNumber(line))
                        lastNumberLine = int.Parse(line);

                    if (_time.IsThisTimeLine(line))
                        lastTimeLine = line;
                }
            }

            // Calculate the total time difference between the target and the actual last timestamp
            TimeSpan lastTime = _time.GetTime(lastTimeLine);
            double differTime = correctTime.Subtract(lastTime).TotalMilliseconds;

            // Distribute the correction evenly across all parts except the first
            // (the first subtitle always stays at its original time)
            double correction = differTime / (lastNumberLine - 1);

            // --- Second pass: apply proportional correction to each time line ---
            int lineNumber = 1;

            using (StreamReader srtFile = _readWrite.ReadFile(tempPath))
            using (TextWriter newSrtFile = _readWrite.WriteFile(saveLocation))
            {
                string line;
                while (!srtFile.EndOfStream)
                {
                    line = srtFile.ReadLine() ?? string.Empty;

                    if (_number.IsThisLineNumber(line))
                        lineNumber = int.Parse(line);

                    // The first subtitle section is left unchanged
                    if (_time.IsThisTimeLine(line) && lineNumber > 1)
                    {
                        // Correction grows linearly with the line number.
                        // Negative corrections are not supported and are clamped to zero.
                        TimeSpan correctionValue = TimeSpan.FromMilliseconds(
                            correction > 0 ? (lineNumber - 1) * correction : 0);

                        newSrtFile.WriteLine(_time.AddTime(line, correctionValue));
                    }
                    else
                    {
                        newSrtFile.WriteLine(line);
                    }
                }
            }

            File.Delete(tempPath);

            // If overwrite mode is enabled, replace the original file with the corrected one
            if (AppSettings.IsOverWritable)
                _readWrite.ReplaceFile(saveLocation);
        }

        #endregion
    }
}