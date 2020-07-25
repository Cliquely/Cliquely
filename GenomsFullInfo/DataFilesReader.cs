using System;
using System.Collections.Generic;
using System.IO;

namespace GenomsFullInfo
{
    public static class DataFilesReader
    {
        public static async IAsyncEnumerable<string[]> ReadFile(string filename, int rowsToSkip = 0, char seperator = '\t', StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            using var stream = new StreamReader(filename);

            for(var i = 0; i < rowsToSkip && !stream.EndOfStream; i++)
            {
                stream.ReadLine();
            }

            while (!stream.EndOfStream)
            {
                var line = await stream.ReadLineAsync();

                yield return line.Split(seperator, splitOptions);
            }
        }
    }
}
