using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PredmetniZadatak_1.Windows
{
    /// <summary>
    /// Interaction logic for TextWindow.xaml
    /// </summary>
    public partial class TextWindow : Window
    {
        public string textProp = "";
        public double textSizeProp = 0;
        public SolidColorBrush colorTextProp = Brushes.Transparent;
        public bool isFormGood = false;
        public TextWindow()
        {
            InitializeComponent();
        }

        public TextWindow(TextBlock source)
        {
            InitializeComponent();
            text.Text = source.Text;
            size.Text = source.FontSize.ToString();
            colorText.Background = source.Foreground;
        }

        private void PickColorText(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //colorTextProp = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                colorText.Background = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
            }
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            colorText.BorderBrush = Brushes.Black;
            text.BorderBrush = Brushes.Black;

            textProp = text.Text;
            colorTextProp = colorText.Background as SolidColorBrush;

            if (size.Text == "" || !double.TryParse(size.Text, out textSizeProp))
                size.BorderBrush = Brushes.Red;
            if (text.Text == "")
                text.BorderBrush = Brushes.Red;

            textProp = text.Text;

            if (textSizeProp > 0 && textProp != "")
            {
                isFormGood = true;
                this.Close();
            }

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
