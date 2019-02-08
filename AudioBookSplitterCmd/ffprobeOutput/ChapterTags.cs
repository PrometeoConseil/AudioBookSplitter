using Newtonsoft.Json;

namespace AudioBookSplitterCmd.ffprobeOutput
{
    public class ChapterTags
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }

}
