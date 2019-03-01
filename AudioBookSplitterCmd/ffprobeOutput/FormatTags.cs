using Newtonsoft.Json;

namespace AudioBookSplitterCmd.ffprobeOutput
{
    public class FormatTags
    {
        [JsonProperty(PropertyName = "album")]
        public string Album { get; set; }

        [JsonProperty(PropertyName = "artist")]
        public string Artist { get; set; }

        [JsonProperty(PropertyName = "album_artist")]
        public string AlbumArtist { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }

        [JsonProperty(PropertyName = "encoder")]
        public string Encoder { get; set; }

        [JsonProperty(PropertyName = "genre")]
        public string Genre { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "track")]
        public string Track { get; set; }

        [JsonProperty(PropertyName = "ENCODINGTIME")]
        public string EncodingTime { get; set; }

        [JsonProperty(PropertyName = "major_brand")]
        public string MajorBrand { get; set; }

        [JsonProperty(PropertyName = "minor_version")]
        public string MinorVersion { get; set; }

        [JsonProperty(PropertyName = "compatible_brands")]
        public string CompatibleBrands { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "disc")]
        public string Disc { get; set; }
    }
}