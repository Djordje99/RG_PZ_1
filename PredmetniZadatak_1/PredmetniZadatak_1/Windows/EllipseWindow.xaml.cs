﻿using System;
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
    /// Interaction logic for EllipseWindow.xaml
    /// </summary>
    public partial class EllipseWindow : Window
    {
        public string textProp = "";
        public SolidColorBrush colorFillProp = Brushes.Transparent;
        public SolidColorBrush colorBorderProp = Brushes.Transparent;
        public double widthProp = 0;
        public double heightProp = 0;
        public double borderProp = 0;
        public SolidColorBrush colorTextProp = Brushes.Transparent;

        public EllipseWindow()
        {
            InitializeComponent();
        }

        private void PickColorFill(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if(cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
            height.BorderBrush = Brushes.Black;
            width.BorderBrush = Brushes.Black;
            border.BorderBrush = Brushes.Black;

            textProp = text.Text;

            if (height.Text == "" || !double.TryParse(height.Text, out heightProp))
                height.BorderBrush = Brushes.Red;
            if (width.Text == "" || !double.TryParse(width.Text, out widthProp))
                width.BorderBrush = Brushes.Red;
            if (border.Text == "" || !double.TryParse(border.Text, out borderProp))
                border.BorderBrush = Brushes.Red;

            if (heightProp > 0 && widthProp > 0 && borderProp > 0)
                this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
