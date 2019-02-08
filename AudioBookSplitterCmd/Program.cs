using CommandLine;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace AudioBookSplitterCmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new Parser(config => config.HelpWriter = Console.Out);
            var result = parser.ParseArguments<Options>(args);

            if (result.GetType() != typeof(Parsed<Options>))
            {
                return;
            }

            var parsedResult = ((Parsed<Options>) result).Value;

            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                var source = parsedResult.SourceFile;
                if (!File.Exists(source))
                {
                    Console.WriteLine($"File '{source}' not found.");
                }
                else
                {
                    // using ffprobe, extract chapter list and info (album, artist, date, ...) from mp3.
                    // Command is : ffprobe -v quiet -print_format json -show_chapters -show_format <filename>.mp3

                    // Redirect the output stream of the child process.
                    var p = new Process
                    {
                        StartInfo =
                        {
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            FileName = "ffprobe.exe",
                            Arguments = $"-v quiet -print_format json -show_chapters -show_format \"{source}\""
                        }
                    };

                    // Start the child process.
                    p.Start();

                    // Read the output stream first and then wait.
                    var output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();

                    var serializedOutput = JsonConvert.DeserializeObject<ffprobeOutput.RootObject>(output);
                    var chapters = serializedOutput.Chapters;
                    var formatDescriptions = serializedOutput.Format;

                    var folder = $"{formatDescriptions.Tags.AlbumArtist.ToFilePathSafeString()}\\{formatDescriptions.Tags.Date.ToFilePathSafeString()} - {formatDescriptions.Tags.Album.ToFilePathSafeString()}";

                    if (!string.IsNullOrEmpty(parsedResult.OutputFolder))
                    {
                        folder = Path.Combine(parsedResult.OutputFolder, folder);
                    }

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var track = 0;
                    foreach (var chapter in chapters)
                    {
                        track++;
                        var target = Path.Combine(folder, $"{track:D2} - {chapter.Tags.Title}.mp3".ToFilePathSafeString());
                        
                        // remove existing file
                        if (File.Exists(target))
                            File.Delete(target);

                        var start = Convert.ToDouble(chapter.StartTime, new CultureInfo("en-US"));
                        var duration = Convert.ToDouble(chapter.EndTime, new CultureInfo("en-US")) - start;

                        var arguments = $"-v quiet -stats -ss {start.ToString(new CultureInfo("en-US"))} ";
                        arguments += $"-t {duration.ToString(new CultureInfo("en-US"))} -i \"{source}\" ";
                        // To add artist as ID3 tag
                        arguments += $"-metadata artist=\"{formatDescriptions.Tags.Artist}\" ";
                        // To add chapter title as mp3 title ID3 tag
                        arguments += $"-metadata title=\"{chapter.Tags.Title}\" ";
                        // To add track number title as ID3 tag
                        arguments += $"-metadata track={track} \"{target}\"";
                        Console.WriteLine($"Extracting '{track:D2} - {chapter.Tags.Title}' from '{formatDescriptions.Tags.Album}'");
                        var pExtract = new Process
                        {
                            StartInfo =
                            {
                                UseShellExecute = false,
                                FileName = "ffmpeg.exe",
                                Arguments = arguments
                            }
                        };                       
                        pExtract.Start();
                        // wait for end of extraction.                    
                        pExtract.WaitForExit();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.Write("Press any key to exit the program.");
            Console.ReadKey(true);
        }

       
    }
}