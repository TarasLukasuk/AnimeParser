namespace AnimeParserBL
{
    public abstract class AnimeParserSetting
    {
        public abstract string Url { get;}
        public abstract string GetUrl { get; set; }
        public abstract string Genres { get; }
        public abstract string Years { get; }
        public abstract string Prefix { get; }
        public abstract int StartPoint { get; set; }
        public abstract int EndPoint { get; set; }
    }
}
