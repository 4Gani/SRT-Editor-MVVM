using SRTEditor_MVVM.Services.ToolKit;
using System.IO;

namespace SRTEditor_MVVM.Services
{
    /// <summary>
    /// Shifts SRT file timestamps by a fixed time offset across a specified range of subtitle entries.
    /// </summary>
    public class SrtTimeShifter(SrtRepairer repairing, IReadWrite readWrite, ITime time)
    {
        #region Fields and Properties
        private readonly SrtRepairer _repairing = repairing;
        private readonly IReadWrite _readWrite = readWrite;
        private readonly ITime _time = time;
        #endregion

        #region Methods

        /// <summary>
        /// Shifts timestamps in the specified SRT file by the given time offset.
        /// Only subtitle entries within the specified line number range are affected.
        /// </summary>
        /// <param name="srtFileAddress">Path to the source SRT file.</param>
        /// <param name="srtSaveLocation">Path where the shifted file will be saved.</param>
        /// <param name="shiftingTime">The time offset to apply, in HH:MM:SS.mmm format.</param>
        /// <param name="from">The first subtitle line number to shift (ignored when applying to all lines).</param>
        /// <param name="to">The last subtitle line number to shift (ignored when applying to all lines).</param>
        /// <returns>A <see cref="ShiftResult"/> indicating success or the reason for failure.</returns>
        public ShiftResult ShiftTime(string srtFileAddress, string srtSaveLocation,
            string shiftingTime, string from, string to)
        {
            string tempPath = Path.Combine(AppSettings.Directory, "temp_Sub(repaired).srt");

            // Repair the file first and save it to a temporary location
            _repairing.RepairFile(srtFileAddress, tempPath);

            string startLineNumber;
            string endLineNumber;

            if (AppSettings.IsApplyToAllLines)
            {
                // Cover the entire file by using a range wider than any real SRT file
                startLineNumber = "1";
                endLineNumber = "99999";
            }
            else
            {
                // Use the line range provided by the user
                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    startLineNumber = from;
                    endLineNumber = Convert.ToString(Convert.ToInt32(to) + 1);
                }
                else
                {
                    File.Delete(tempPath);
                    return ShiftResult.InvalidRange;
                }
            }

            if (string.IsNullOrEmpty(shiftingTime))
            {
                File.Delete(tempPath);
                return ShiftResult.InvalidTime;
            }

            TimeSpan offsetTime = _time.GetTimeWithMiliSecond(shiftingTime);

            using (StreamReader srtFile = _readWrite.ReadFile(tempPath))
            using (TextWriter newSrtFile = _readWrite.WriteFile(srtSaveLocation))
            {
                string line;

                while (!srtFile.EndOfStream)
                {
                    line = srtFile.ReadLine() ?? string.Empty;

                    // Process lines within the specified range
                    if (line.Equals(startLineNumber))
                    {
                        while (line != string.Empty && line != endLineNumber)
                        {
                            if (_time.IsThisTimeLine(line))
                            {
                                // Raise or reduce timestamps based on the user's selection
                                string newLine = AppSettings.IsRaised
                                    ? _time.AddTime(line, offsetTime)
                                    : _time.SubtractTime(line, offsetTime);

                                newSrtFile.WriteLine(newLine);
                            }
                            else
                            {
                                newSrtFile.WriteLine(line);
                            }

                            line = srtFile.ReadLine() ?? string.Empty;
                        }

                        newSrtFile.WriteLine(line);
                    }
                    else
                    {
                        newSrtFile.WriteLine(line);
                    }
                }
            }

            File.Delete(tempPath);

            // If overwrite mode is enabled, replace the original file with the shifted one
            if (AppSettings.IsOverWritable)
                _readWrite.ReplaceFile(srtSaveLocation);

            return ShiftResult.Success;
        }

        #endregion
    }
}