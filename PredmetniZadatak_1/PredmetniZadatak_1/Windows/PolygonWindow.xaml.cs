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
    /// Interaction logic for PolygonWindow.xaml
    /// </summary>
    public partial class PolygonWindow : Window
    {
        public string textProp = "";
        public SolidColorBrush colorFillProp = Brushes.Transparent;
        public SolidColorBrush colorBorderProp = Brushes.Transparent;
        public double borderProp = 0;
        public SolidColorBrush colorTextProp = Brushes.Transparent;

        public PolygonWindow()
        {
            InitializeComponent();
        }

        private void PickColorFill(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorFillProp = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                colorFill.Background = colorFillProp;
            }
        }

        private void PickColorText(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorTextProp = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                colorText.Background = colorTextProp;
            }
        }

        private void PickColorBorder(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorBorderProp = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                colorBorder.Background = colorBorderProp;
            }
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            border.BorderBrush = Brushes.Black;

            textProp = text.Text;

            if (border.Text == "" || !double.TryParse(border.Text, out borderProp))
                border.BorderBrush = Brushes.Red;

            if (borderProp > 0)
                this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
