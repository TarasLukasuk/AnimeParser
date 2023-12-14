using AnimeParser.UserContro;
using AnimeParserBL.AnimeStars;
using AnimeParserBL.Memento;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace AnimeParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AnimeStarsParser animeStarsParser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool isResizing = false;
        private Point lastMousePosition;

        private string tags;
        private string year;
        private string url;


        private void ParsingCompleted(List<AnimeMemento> mementos)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (AnimeMemento item in mementos)
                {
                    ShowAnime showAnime = new ShowAnime();
                    showAnime.Height = 120;
                    showAnime.Width = this.ActualWidth;
                    showAnime.VerticalAlignment = VerticalAlignment.Top;
                    showAnime.Margin = new Thickness(0, 0, 0, 5);

                    showAnime.Title.Text = item.Title;
                    showAnime.Time.Text = item.ViewingTime;
                    BitmapImage bitmapImage = new BitmapImage(new Uri($"{url}{item.ImageUrl}", UriKind.RelativeOrAbsolute));
                    showAnime.Image.Source = bitmapImage;
                    showAnime.Description.Document.Blocks.Clear();
                    showAnime.Description.Document.Blocks.Add(new Paragraph(new Run(item.Description)));

                    if (ContainsWord(item.Director, "Жанр"))
                    {
                        showAnime.Genres.Text = item.Director;
                    }

                    if (ContainsWord(item.Director, "Режиссер"))
                    {
                        showAnime.Director.Text = item.Director;
                    }

                    if (ContainsWord(item.Genres, "Студия"))
                    {
                        showAnime.Studio.Text = item.Genres;
                    }

                    if (ContainsWord(item.Genres, "Жанр"))
                    {
                        showAnime.Genres.Text = item.Genres;
                    }

                    if (ContainsWord(item.OriginalAuthor, "Студия"))
                    {
                        showAnime.Studio.Text = item.OriginalAuthor;
                    }

                    if (ContainsWord(item.OriginalAuthor, "Жанр"))
                    {
                        showAnime.Genres.Text = item.OriginalAuthor;
                    }

                    if (ContainsWord(item.Studios, "Автор оригинала"))
                    {
                        showAnime.AuthorOriginal.Text = item.Studios;
                    }

                    if (ContainsWord(item.Studios, "Студия"))
                    {
                        showAnime.Studio.Text = item.Studios;
                    }

                    Content.Children.Add(showAnime);
                }
            });
        }

        private void StartParsing_Click(object sender, RoutedEventArgs e)
        {
            string parsedUrl;
            animeStarsParser = new AnimeStarsParser(new AnimeStarsSetting(genres: tags,
                                                                          years: year,
                                                                          start: StartPoint.NumberValue,
                                                                          end: StopPoint.NumberValue,
                                                                          url: out parsedUrl));
            url = parsedUrl;

            animeStarsParser.ParsingCompleted += ParsingCompleted;
            animeStarsParser.Start();
        }

        private void StopParsing_Click(object sender, RoutedEventArgs e)
        {
            animeStarsParser.Stop();
        }

        private bool ContainsWord(string input, string wordToCheck)
        {
            return input.Contains(wordToCheck);
        }

        private void Genres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItemGenres = Genres.SelectedItem.ToString();
            string selectedItemGenresReplace = selectedItemGenres.Replace("System.Windows.Controls.ComboBoxItem: ", string.Empty);
            ReplacementTags(selectedItemGenresReplace);
        }

        private void Genres_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.IsDropDownOpen = true;
            }
        }

        private void Years_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItemYear = Years.SelectedItem.ToString();
            string selectedItemYearReplace = selectedItemYear.Replace("System.Windows.Controls.ComboBoxItem: ", string.Empty);
            year = selectedItemYearReplace;
        }

        private void ReplacementTags(string selectedItem)
        {
            Dictionary<string, string> tagMappings = new Dictionary<string, string>()
            {
                {"Экшен", "action"},
                {"Боевые искусства", "martial_arts"},
                {"Вампиры","vampires" },
                {"Война","war"},
                {"Гарем ","garem"},
                {"Гарем для девушек","garem-dlya-devushek"},
                {"Гендерная интрига","gender_intriga"},
                {"Детектив","detective"},
                {"Дзёсэй","josei"},
                {"Драма","drama"},
                {"Игра","game"},
                {"История","historical"},
                {"Киберпанк","cyberpunk"},
                {"Комедия","comedy"},
                {"Меха","mecha"},
                {"Мистика","mystery"},
                {"Пародия","parody"},
                {"Повседневность","natural"},
                {"Постапокалиптика","postapocalypse"},
                {"Приключения","adventure"},
                {"Психология","psychological"},
                {"Романтика","romance"},
                {"Самураи","samurai"},
                {"Сёдзё","shoujo"},
                {"Сёнэн","shounen"},
                {"Спорт","sports"},
                {"Сэйнэн","seinen"},
                {"Трагедия","tragedy"},
                {"Триллер","thriller"},
                {"Ужасы","horror"},
                {"Фантастика","fantastic"},
                {"Фэнтези","fantasy"},
                {"Школа","school"},
                {"Эччи","jechchi"}
            };

            tags = tagMappings.ContainsKey(selectedItem) ? tagMappings[selectedItem] : null;
        }

        private void NormalMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                NormalMaxImage.Source = new BitmapImage(new Uri(@"UserContro\Resource\normal.png", UriKind.Relative));
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                NormalMaxImage.Source = new BitmapImage(new Uri(@"UserContro\Resource\max.png", UriKind.Relative));
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Move_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Minimized_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
