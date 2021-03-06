using CommandLine;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine.Text;

namespace AudioBookSplitterCmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new Parser(config => config.HelpWriter = null);
            var result = parser.ParseArguments<Options>(args);
            result.WithNotParsed(errors =>
            {
                foreach (var error in errors)
                {
                    if (error.Tag != ErrorType.HelpRequestedError &&
                        error.Tag != ErrorType.VersionRequestedError) continue;

                    Console.WriteLine(BuildHelp(result));
                    Environment.Exit(0);
                }

                var myHelpText = HelpText.AutoBuild(result, onError => BuildHelp(result), onExample => onExample);
                Console.Error.WriteLine(myHelpText);
                Environment.Exit(1);
            });

            if (result.GetType() != typeof(Parsed<Options>))
            {
                return;
            }

            var parsedResult = ((Parsed<Options>)result).Value;

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
                    var number = string.Empty;
                    try
                    {
                        number = (formatDescriptions.Tags.Disc ?? formatDescriptions.Tags.Date).ToFilePathSafeString();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"[WARN] FAILED to format track number '{(formatDescriptions.Tags.Disc ?? formatDescriptions.Tags.Date)}' as a valid file path part.");
                    }

                    var folder = string.Empty;


                    if (parsedResult.PreferShortTargetFolder)
                    {
                        folder = "";
                    }
                    else
                    {
                        try
                        {
                            folder = $"{formatDescriptions.Tags.AlbumArtist.ToFilePathSafeString()}";
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"[WARN] FAILED to format album artist '{formatDescriptions.Tags.AlbumArtist}' as a valid file path part.");
                        }

                        try
                        {
                            folder += $"\\{formatDescriptions.Tags.Album.ToFilePathSafeString()}";
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"[WARN] FAILED to format album  '{formatDescriptions.Tags.Album}' as a valid file path part.");
                        }
                        try
                        {
                            folder += $"\\{(number == null ? "" : number + " - ")}{formatDescriptions.Tags.Title.ToFilePathSafeString()}";
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"[WARN] FAILED to format title '{formatDescriptions.Tags.Title}' as a valid file path part.");
                        }
                    }

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
                        Console.WriteLine($"Trying to create \"{track:D2} - {chapter.Tags.Title}.mp3\" into folder \"{folder}\"");
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
                        arguments += $"-metadata title=\"{formatDescriptions.Tags.Title} - {chapter.Tags.Title}\" ";
                        // To add track number title as ID3 tag
                        arguments += $"-metadata track={track} \"{target}\"";
                        Console.WriteLine($"Extracting {track}/{chapters.Length}: '{track:D2} - {chapter.Tags.Title}' from '{formatDescriptions.Tags.Album}'");
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

        private static HelpText BuildHelp(ParserResult<Options> result)
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;

            var assemblyTitleAttribute = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute)).SingleOrDefault() as AssemblyTitleAttribute;
            var assemblyCopyrightAttribute = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute)).SingleOrDefault() as AssemblyCopyrightAttribute;
            var assemblyCompanyAttribute = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute)).SingleOrDefault() as AssemblyCompanyAttribute;
            var assemblyDescriptionAttribute = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute)).SingleOrDefault() as AssemblyDescriptionAttribute;
            var version = assembly.GetName().Version.ToString().ToString(CultureInfo.InvariantCulture);

            var nHelpText = new HelpText(SentenceBuilder.Create(), $"{assemblyTitleAttribute?.Title} {version}"
                                                                   + $"{(assemblyCopyrightAttribute == null && assemblyCompanyAttribute == null ? "" : "\r\n" + (assemblyCopyrightAttribute?.Copyright))} {assemblyCompanyAttribute?.Company}"
                                                                   + $"{((assemblyDescriptionAttribute == null) ? "" : "\r\n" + assemblyDescriptionAttribute.Description)}")
            {
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = true,
                MaximumDisplayWidth = 4000,
                AddEnumValuesToHelpText = true
            };
            nHelpText.AddOptions(result);
            return HelpText.DefaultParsingErrorsHandler(result, nHelpText);
        }

    }
}