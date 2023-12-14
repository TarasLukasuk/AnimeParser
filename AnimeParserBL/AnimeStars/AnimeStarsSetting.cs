namespace AnimeParserBL.AnimeStars
{
    public class AnimeStarsSetting : AnimeParserSetting
    {
        public AnimeStarsSetting(string genres = null, string years = null, int start = 1, int end = 1)
        {
            Genres = genres != null ? $"/aniserials/video/{genres}/" : string.Empty;
            Years = years != null ? $"/xfsearch/year/{years}/" : string.Empty;
            Prefix = "/page/{Id}/";
            GetUrl = $"{Url}{Genres}{Years}{Prefix}";

            StartPoint = start;
            EndPoint = end;
        }

        public AnimeStarsSetting(string genres, string years, int start, int end, out string url)
            : this(genres, years, start, end)
        {
            url = $"{Url}/";
        }


        public override string Url => "...";
        public override string Genres { get; }
        public override string Years { get; }
        public override string Prefix { get; }
        public override string GetUrl { get; set; }

        public override int StartPoint { get; set; }
        public override int EndPoint { get; set; }
    }
}
