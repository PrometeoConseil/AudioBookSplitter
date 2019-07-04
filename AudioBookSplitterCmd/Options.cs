using CommandLine;

namespace AudioBookSplitterCmd
{
    public class Options
    {
        [Option('s',"Source",Required = true, HelpText="MP3 file to split")]
        public string SourceFile { get; set; }

        [Option('o', "Output", HelpText = "The folder where the files will be stored.")]
        public string  OutputFolder { get; set; }

        [Option('p', "PreferShortTargetFolder",Default = false, HelpText = "No track chapter metadata will be added to output folder name.")]
        public bool PreferShortTargetFolder { get; set; }
    }
}
