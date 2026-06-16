using SRTEditor_MVVM.Services.ToolKit;
using System.IO;

namespace SRTEditor_MVVM.Services
{
    /// <summary>
    /// Repairs SRT files by correcting line numbers, time formats,
    /// and removing malformed or extraneous lines.
    /// </summary>
    public class SrtRepairer(IReadWrite readWrite, INumber number, ITime time, ILine line)
    {
        #region Fields and Properties
        private readonly IReadWrite _readWrite = readWrite;
        private readonly INumber _number = number;
        private readonly ITime _time = time;
        private readonly ILine _line = line;
        #endregion

        #region Methods

        /// <summary>
        /// Reads the specified SRT file, corrects its structure, and writes the result
        /// to the save location. If overwrite mode is enabled, replaces the original file.
        /// <para>
        /// SRT files consist of four repeating sections:
        /// <list type="number">
        ///   <item>Line number (e.g. 52)</item>
        ///   <item>Time line (e.g. 00:00:16,902 --> 00:00:19,302)</item>
        ///   <item>Subtitle text (e.g. All I wanna do is lose control)</item>
        ///   <item>Empty separator line</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="fileAddress">Path to the source SRT file.</param>
        /// <param name="saveLocation">Path where the repaired file will be saved.</param>
        public void RepairFile(string fileAddress, string saveLocation)
        {
            string line;
            int trueLineNumber = 1;

            using (StreamReader srtFile = _readWrite.ReadFile(fileAddress))
            using (TextWriter newSrtFile = _readWrite.WriteFile(saveLocation))
            {
                while (!srtFile.EndOfStream)
                {
                    line = srtFile.ReadLine() ?? string.Empty;

                    if (_number.IsThisLineNumber(line))
                    {
                        // Add a blank separator line before each section except the first
                        if (trueLineNumber > 1)
                            newSrtFile.WriteLine("");

                        // Replace the original line number with a sequential one starting from 1
                        newSrtFile.WriteLine(trueLineNumber);
                        trueLineNumber++;
                    }
                    else if (_time.IsThisTimeLine(line))
                    {
                        // Normalize the time line format before writing
                        newSrtFile.WriteLine(_time.TimeLineCorrection(line));
                    }
                    else if (_line.IsThisFreeLine(line))
                    {
                        // Skip stray empty lines — they are re-added before each section
                        continue;
                    }
                    else
                    {
                        // Write subtitle text lines as-is
                        newSrtFile.WriteLine(line);
                    }
                }
            }

            // If overwrite mode is enabled, replace the original file with the repaired one
            if (AppSettings.IsOverWritable)
                _readWrite.ReplaceFile(saveLocation);
        }

        #endregion
    }
}