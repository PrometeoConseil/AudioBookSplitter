using CommandLine;

namespace AudioBookSplitterCmd
{
    public class Options
    {
        [Option('s', HelpText="MP3 file to split")]
        public string SourceFile { get; set; }

        [Option('o', HelpText = "The folder where the files will be stored.")]
        public string  OutputFolder { get; set; }
    }
}
