using Newtonsoft.Json;

namespace AudioBookSplitterCmd.ffprobeOutput
{
    public class Chapter
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "time_base")]
        public string TimeBase { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "start_time")]
        public string StartTime { get; set; }

        [JsonProperty(PropertyName = "end")]
        public int End { get; set; }

        [JsonProperty(PropertyName = "end_time")]
        public string EndTime { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public ChapterTags Tags { get; set; }
    }
}