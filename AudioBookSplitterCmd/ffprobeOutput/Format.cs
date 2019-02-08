using Newtonsoft.Json;

namespace AudioBookSplitterCmd.ffprobeOutput
{
    public class Format
    {
        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "nb_streams")]
        public int NbStreams { get; set; }

        [JsonProperty(PropertyName = "nb_programs")]
        public int NbPrograms { get; set; }

        [JsonProperty(PropertyName = "format_name")]
        public string FormatName { get; set; }

        [JsonProperty(PropertyName = "format_long_name")]
        public string FormatLongName { get; set; }

        [JsonProperty(PropertyName = "start_time")]
        public string StartTime { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }

        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "bit_rate")]
        public string BitRate { get; set; }

        [JsonProperty(PropertyName = "probe_score")]
        public int ProbeScore { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public FormatTags Tags { get; set; }
    }
}