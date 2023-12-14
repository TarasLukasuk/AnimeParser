using AnimeParserBL.Memento;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace AnimeParserBL.AnimeStars
{
    public class AnimeStarsParser : Parser
    {
        private readonly AnimeParserSetting animeParserSetting;
        private Anime anime = new Anime();

        protected override bool IsActive { get; set; }

        private string
        titleText = string.Empty,
        imageUrl = string.Empty,
        viewingTimeText = string.Empty,
        yearManufactureText = string.Empty,
        directorText = string.Empty,
        originalAuthorText = string.Empty,
        studiosText = string.Empty,
        genresText = string.Empty,
        descriptionText = string.Empty;

        public delegate void ParsingCompletedHandler(List<AnimeMemento> mementos);

        public event ParsingCompletedHandler ParsingCompleted;


        public AnimeStarsParser(AnimeParserSetting animeParserSetting)
        {
            this.animeParserSetting = animeParserSetting;
        }

        public override void Start()
        {
            IsActive = true;
            ParsersAsync();


        }

        protected override async Task ParsersAsync()
        {
            await Task.Run(() => Parsers());
        }

        public override void Stop()
        {
            IsActive = false;
            var rezalt = anime.GetAllData();

            OnParsingCompleted(rezalt);
        }

        private void OnParsingCompleted(List<AnimeMemento> rezalt)
        {
            ParsingCompleted?.Invoke(rezalt);
        }

        protected override void ProcessHrefValue(string hrefValue)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = httpClient.GetAsync(hrefValue).Result;
                    response.EnsureSuccessStatusCode();

                    string content = response.Content.ReadAsStringAsync().Result;

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(content);

                    HtmlNode imege = document?.DocumentNode?.SelectSingleNode("//div[contains(@class, 'pmovie__poster')]");
                    HtmlNode title = document?.DocumentNode?.SelectSingleNode("//header[contains(@class, 'page__subcol-header')]");
                    HtmlNodeCollection info = document?.DocumentNode?.SelectNodes("//ul[@class='page__subcol-info pmovie__header-list']/li");
                    HtmlNodeCollection descriptions = document?.DocumentNode?.SelectNodes("//div[contains(@class, 'page__text')]/p");

                    titleText = title?.SelectSingleNode(".//h1[@itemprop='name']")?.InnerText?.Trim() ?? string.Empty;
                    imageUrl = imege?.SelectSingleNode(".//img")?.GetAttributeValue("src", "") ?? string.Empty;

                    if (info != null)
                    {
                        viewingTimeText = info[0]?.InnerText?.ToString();
                        yearManufactureText = info[1]?.InnerText?.ToString();
                        try
                        {
                            directorText = info[2]?.InnerText?.ToString();
                        }
                        catch (Exception)
                        {
                            descriptionText = string.Empty;
                        }

                        try
                        {
                            originalAuthorText = info[3]?.InnerText?.ToString();
                        }
                        catch (Exception)
                        {
                            originalAuthorText = string.Empty;
                        }

                        try
                        {
                            studiosText = info[4].InnerText.ToString();
                        }
                        catch (Exception)
                        {
                            studiosText = string.Empty;
                        }

                        try
                        {
                            genresText = info[5].InnerText.ToString();
                        }
                        catch (Exception)
                        {
                            genresText = string.Empty;
                        }
                    }


                    if (descriptions != null)
                    {
                        StringBuilder descriptionBuilder = new StringBuilder();
                        foreach (HtmlNode description in descriptions)
                        {
                            descriptionBuilder.Append(description.InnerText);
                        }
                        descriptionText = $"Описание {descriptionBuilder}";
                    }
                }
            }
            catch (Exception)
            { }


            anime.Title = titleText;
            anime.ImageUrl = imageUrl;
            anime.ViewingTime = viewingTimeText;
            anime.YearManufacture = yearManufactureText;
            anime.Director = directorText;
            anime.OriginalAuthor = originalAuthorText;
            anime.Studios = studiosText;
            anime.Genres = genresText;
            anime.Description = descriptionText;

            anime.SaveToMemento();

        }

        protected override void Parsers()
        {
            for (int i = animeParserSetting.StartPoint; i <= animeParserSetting.EndPoint; i++)
            {
                if (!IsActive)
                {
                    return;
                }

                string url = animeParserSetting.GetUrl.Replace("{Id}", i.ToString());

                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = httpClient.GetAsync(url).Result;
                        response.EnsureSuccessStatusCode();


                        string content = response.Content.ReadAsStringAsync().Result;

                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(content);

                        HtmlNodeCollection anchorElements = document.DocumentNode.SelectNodes("//a[contains(@class, 'poster grid-item d-flex fd-column has-overlay')]");

                        if (anchorElements != null)
                        {
                            ProcessAnimePages(anchorElements);
                        }
                    }
                }
                catch (Exception)
                { }
            }
            Stop();
        }

        protected override void ProcessAnimePages(HtmlNodeCollection anchorElements)
        {
            foreach (var anchorElement in anchorElements)
            {
                string hrefValue = anchorElement.GetAttributeValue("href", "");
                ProcessHrefValue(hrefValue);
            }
        }
    }
}
