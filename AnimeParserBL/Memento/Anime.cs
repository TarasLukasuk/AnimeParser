using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeParserBL.Memento
{
    public class Anime
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ViewingTime { get; set; }
        public string YearManufacture { get; set; }
        public string Director { get; set; }
        public string OriginalAuthor { get; set; }
        public string Studios { get; set; }
        public string Genres { get; set; }
        public string Description { get; set; }

        public List<AnimeMemento> Mementos { get; } = new List<AnimeMemento>();

        public void SaveToMemento()
        {
            Mementos.Add(new AnimeMemento
            {
                Title = Title,
                ImageUrl = ImageUrl,
                ViewingTime = ViewingTime,
                YearManufacture = YearManufacture,
                Director = Director,
                OriginalAuthor = OriginalAuthor,
                Studios=Studios,
                Genres = Genres,
                Description = Description
            });

        }

        public List<AnimeMemento> GetAllData()
        {
            return Mementos.Distinct().ToList();
        }
    }
}
