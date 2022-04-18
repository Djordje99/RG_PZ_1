using PredmetniZadatak_1.Dots;
using PredmetniZadatak_1.Lines;
using PredmetniZadatak_1.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace PredmetniZadatak_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DotFormater dot = new DotFormater();
        private LineFormer line;
        private double zoom = 0;
        private Dictionary<string, int> indexOnCanvas = new Dictionary<string, int>();
        private Dictionary<int, Tuple<int, int>> indexCoords = new Dictionary<int, Tuple<int, int>>();
        public MainWindow()
        {
            InitializeComponent();

            this.canvas1.MouseWheel += Canvas_Zoom;

            DrawDots();
            line = new LineFormer(dot.DotModels);
            //DrawLinesBfs();
            //DrawLinesCrossing();
        }

        private void DrawDots()
        {
            List<Ellipse> ellipses =  dot.AddDots(out List<int> placeXList, out List<int> placeYList);

            for (int i = 0; i < ellipses.Count; i++)
            {
                Canvas.SetLeft(ellipses[i], placeXList[i]);
                Canvas.SetBottom(ellipses[i], placeYList[i]);

                indexOnCanvas.Add(ellipses[i].Name, i);

                canvas1.Children.Add(ellipses[i]);

                indexCoords.Add(i, new Tuple<int, int>(placeXList[i], placeYList[i]));
            }
        }

        private void DrawLinesBfs()
        {
            List<Line> lines = line.AddLineBfs();

            foreach (var item in lines)
            {
                item.MouseRightButtonDown += ShowColorDialog;
                canvas1.Children.Add(item);
            }
        }

        private void DrawLinesCrossing()
        {
            List<Line> lines = line.AddLineCrossing(out List<DotModel> crossingDots);

            foreach (var item in lines)
            {
                canvas1.Children.Add(item);
            }

            foreach (var item in crossingDots)
            {
                Canvas.SetLeft(item.Ellipse, item.CanvasX);
                Canvas.SetTop(item.Ellipse, item.CanvasY);

                canvas1.Children.Add(item.Ellipse);
            }
        }

        private void ShowColorDialog(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Line sourceLine = e.Source as Line;
            string toolTipString = sourceLine.ToolTip.ToString().Split(':')[1].Trim();

            Ellipse ellipse1 = canvas1.Children[indexOnCanvas["id_" + toolTipString.Split('-')[0]]] as Ellipse;
            Ellipse ellipse2 = canvas1.Children[indexOnCanvas["id_" + toolTipString.Split('-')[1]]] as Ellipse;

            ColorDialog colorPicker = new ColorDialog();
            
            if(colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush color = new SolidColorBrush(Color.FromArgb(colorPicker.Color.A, colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B));

                Ellipse e1 = new Ellipse()
                {
                    Width = 2,
                    Height = 2,
                    Fill = color,
                    ToolTip = ellipse1.ToolTip,
                };
                Ellipse e2 = new Ellipse()
                {
                    Width = 2,
                    Height = 2,
                    Fill = color,
                    ToolTip = ellipse2.ToolTip,
                };

                int x = indexCoords[indexOnCanvas["id_" + toolTipString.Split('-')[0]]].Item1;
                int y = indexCoords[indexOnCanvas["id_" + toolTipString.Split('-')[0]]].Item2;
                Canvas.SetLeft(e1, x);
                Canvas.SetBottom(e1, y);

                canvas1.Children.Add(e1);

                x = indexCoords[indexOnCanvas["id_" + toolTipString.Split('-')[1]]].Item1;
                y = indexCoords[indexOnCanvas["id_" + toolTipString.Split('-')[1]]].Item2;
                Canvas.SetLeft(e2, x);
                Canvas.SetBottom(e2, y);

                canvas1.Children.Add(e2);

            }

        }

        private void Canvas_Zoom(object sender, MouseWheelEventArgs e)
        {
            double zoomMax = 50;
            double zoomMin = 1;
            double zoomAmount = 5;

            zoom += zoomAmount * (e.Delta / 120);
            if (zoom < zoomMin) { zoom = zoomMin; }
            if (zoom > zoomMax) { zoom = zoomMax; }

            Point mousePos = e.GetPosition(canvas1);

            if (zoom >= 1 && zoom <= 50)
            {
                canvas1.RenderTransform = new ScaleTransform(zoom, zoom, mousePos.X, mousePos.Y);
            }
            else
            {
                canvas1.RenderTransform = new ScaleTransform(zoom, zoom);
            }
        }
    }
}
