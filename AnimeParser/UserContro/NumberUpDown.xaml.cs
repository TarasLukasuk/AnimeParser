using System.Windows;
using System.Windows.Controls;

namespace AnimeParser.UserContro
{
    /// <summary>
    /// Interaction logic for NumberUpDown.xaml
    /// </summary>
    public partial class NumberUpDown : UserControl
    {
        public NumberUpDown()
        {
            InitializeComponent();
            NumberValue = 1;
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            NumberValue++;
           
        }

        private int numberValue;

        public int NumberValue
        {
            get { return numberValue; }
            private set 
            {
                if (value>=1)
                {
                    numberValue = value;
                    NumberText.Text = numberValue.ToString();
                }
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            NumberValue--;
        }
    }
}
