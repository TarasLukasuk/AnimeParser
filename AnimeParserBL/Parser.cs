using HtmlAgilityPack;
using System.Threading.Tasks;

namespace AnimeParserBL
{
    public abstract class Parser
    {
        abstract public void Start();
        abstract public void Stop();
        abstract protected void Parsers();
        abstract protected Task ParsersAsync();
        abstract protected void ProcessHrefValue(string hrefValue);
        abstract protected void ProcessAnimePages(HtmlNodeCollection anchorElements);
        abstract protected bool IsActive { get; set; }
    }
}
