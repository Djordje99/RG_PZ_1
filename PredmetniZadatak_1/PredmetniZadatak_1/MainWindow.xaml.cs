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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        private double zoom = 1;
        public MainWindow()
        {
            InitializeComponent();

            this.canvas1.MouseWheel += Canvas_Zoom;
            //this.canvas1.MouseLeftButtonDown += Canvas_Click;

            DrawDots();
            line = new LineFormer(dot.DotModels);
            DrawLinesBfs();
            //DrawLinesCrossing();
        }

        private void DrawDots()
        {
            List<Ellipse> ellipses =  dot.AddDots(out List<int> placeXList, out List<int> placeYList);

            for (int i = 0; i < ellipses.Count; i++)
            {
                Canvas.SetLeft(ellipses[i], placeXList[i]);
                Canvas.SetBottom(ellipses[i], placeYList[i]);

                canvas1.Children.Add(ellipses[i]);
            }
        }

        private void DrawLinesBfs()
        {
            List<Line> lines = line.AddLineBfs();

            foreach (var item in lines)
            {
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

        private void Canvas_Click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Click");
        }

        private void Canvas_Zoom(object sender, MouseWheelEventArgs e)
        {
            double zoomMax = 50;
            double zoomMin = 1;
            double zoomAmount = 5;

            zoom += zoomAmount * (e.Delta / 120); // Ajust zooming speed (e.Delta = Mouse spin value )
            if (zoom < zoomMin) { zoom = zoomMin; } // Limit Min Scale
            if (zoom > zoomMax) { zoom = zoomMax; } // Limit Max Scale

            Point mousePos = e.GetPosition(canvas1);

            if (zoom >= 1 && zoom <= 50)
            {
                canvas1.RenderTransform = new ScaleTransform(zoom, zoom, mousePos.X, mousePos.Y); // transform Canvas size from mouse position
            }
            else
            {
                canvas1.RenderTransform = new ScaleTransform(zoom, zoom); // transform Canvas size
            }
        }


        #region oldFuctions
        //private void AddSubstations()
        //{
        //    XmlNodeList nodes = xmlLoader.ReadXml("/NetworkModel/Substations/SubstationEntity");

        //    foreach (XmlNode node in nodes)
        //    {
        //        Console.WriteLine(node);

        //        string id = node.SelectSingleNode("Id").InnerText;
        //        string name = node.SelectSingleNode("Name").InnerText;

        //        Ellipse ellipse = new Ellipse
        //        {
        //            Width = 2,
        //            Height = 2,
        //            Fill = Brushes.Black,
        //        };

        //        double utmX = double.Parse(node.SelectSingleNode("X").InnerText);
        //        double utmY = double.Parse(node.SelectSingleNode("X").InnerText);
        //        double X;
        //        double Y;

        //        ConvertToLatLon.ToLatLon(utmX, utmY, 34, out X, out Y);

        //        Console.WriteLine(string.Format("{0}, {1}", X, Y));

        //        long decimalX = Int64.Parse(X.ToString().Split('.')[1]) % canvasDim;
        //        long decimalY = Int64.Parse(Y.ToString().Split('.')[1]) % canvasDim; //namapira se na velicinu kanvasa

        //        //spiral search algorithm
        //        long devDecimalX;
        //        long devDecimalY;

        //        while (true)
        //        {
        //            devDecimalX = decimalX / 3;
        //            devDecimalY = decimalY / 3;
        //            if (dotsMatrix[devDecimalX, devDecimalY] == false) //trazi se kordinata da ima 3x3 slobodan prostor za smestanje tacke
        //            {
        //                dotsMatrix[devDecimalX, devDecimalY] = true;
        //                break;
        //            }

        //            while (true)//i >= 8 * siprlCount)
        //            {
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalX + 1 < 300)
        //                    devDecimalX++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalY--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalY--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalX--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalX--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalX + 1 < 300)
        //                    devDecimalX++;

        //            }
        //        }

        //        Canvas.SetLeft(ellipse, devDecimalX * 3);
        //        Canvas.SetTop(ellipse, devDecimalY * 3);

        //        canvas1.Children.Add(ellipse);
        //    }
        //}

        //private void AddNodes()
        //{
        //    XmlNodeList nodes = xmlLoader.ReadXml("/NetworkModel/Nodes/NodeEntity");

        //    foreach (XmlNode node in nodes)
        //    {
        //        Console.WriteLine(node);

        //        string id = node.SelectSingleNode("Id").InnerText;
        //        string name = node.SelectSingleNode("Name").InnerText;

        //        Ellipse ellipse = new Ellipse
        //        {
        //            Width = 2,
        //            Height = 2,
        //            Fill = Brushes.Black,
        //        };

        //        double utmX = double.Parse(node.SelectSingleNode("X").InnerText);
        //        double utmY = double.Parse(node.SelectSingleNode("X").InnerText);
        //        double X;
        //        double Y;

        //        ConvertToLatLon.ToLatLon(utmX, utmY, 34, out X, out Y);

        //        Console.WriteLine(string.Format("{0}, {1}", X, Y));

        //        long decimalX = Int64.Parse(X.ToString().Split('.')[1]) % canvasDim;
        //        long decimalY = Int64.Parse(Y.ToString().Split('.')[1]) % canvasDim;

        //        int i = 0;
        //        int siprlCount = 1;

        //        long devDecimalX;
        //        long devDecimalY;

        //        while (true)
        //        {
        //            devDecimalX = decimalX / 3;
        //            devDecimalY = decimalY / 3;
        //            if (dotsMatrix[devDecimalX, devDecimalY] == false) //trazi se kordinata da ima 3x3 slobodan prostor za smestanje tacke
        //            {
        //                dotsMatrix[devDecimalX, devDecimalY] = true;
        //                break;
        //            }

        //            while (true)//i >= 8 * siprlCount)
        //            {
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalX + 1 < 300)
        //                    devDecimalX++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalY--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalY--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalX--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalX--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalX + 1 < 300)
        //                    devDecimalX++;

        //            }
        //        }

        //        Canvas.SetLeft(ellipse, devDecimalX * 3);
        //        Canvas.SetTop(ellipse, devDecimalY * 3);

        //        canvas1.Children.Add(ellipse);
        //    }
        //}

        //private void AddSwitches()
        //{
        //    XmlNodeList nodes = xmlLoader.ReadXml("/NetworkModel/Switches/SwitchEntity");

        //    foreach (XmlNode node in nodes)
        //    {
        //        Console.WriteLine(node);

        //        string id = node.SelectSingleNode("Id").InnerText;
        //        string name = node.SelectSingleNode("Name").InnerText;

        //        Ellipse ellipse = new Ellipse
        //        {
        //            Width = 2,
        //            Height = 2,
        //            Fill = Brushes.Black,
        //        };

        //        double utmX = double.Parse(node.SelectSingleNode("X").InnerText);
        //        double utmY = double.Parse(node.SelectSingleNode("X").InnerText);
        //        double X;
        //        double Y;

        //        ConvertToLatLon.ToLatLon(utmX, utmY, 34, out X, out Y);

        //        Console.WriteLine(string.Format("{0}, {1}", X, Y));

        //        long decimalX = Int64.Parse(X.ToString().Split('.')[1]) % canvasDim;
        //        long decimalY = Int64.Parse(Y.ToString().Split('.')[1]) % canvasDim;

        //        long devDecimalX;
        //        long devDecimalY;

        //        while (true)
        //        {
        //            devDecimalX = decimalX / 3;
        //            devDecimalY = decimalY / 3;
        //            if (dotsMatrix[devDecimalX, devDecimalY] == false) //trazi se kordinata da ima 3x3 slobodan prostor za smestanje tacke
        //            {
        //                dotsMatrix[devDecimalX, devDecimalY] = true;
        //                break;
        //            }

        //            while (true)//i >= 8 * siprlCount)
        //            {
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalX + 1 < 300)
        //                    devDecimalX++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalY--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalY--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalX--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                decimalX--;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalY + 1 < 300)
        //                    devDecimalY++;
        //                if (dotsMatrix[devDecimalX, devDecimalY] == false)
        //                {
        //                    dotsMatrix[devDecimalX, devDecimalY] = true;
        //                    break;
        //                }
        //                if (devDecimalX + 1 < 300)
        //                    devDecimalX++;

        //            }
        //        }

        //        Canvas.SetLeft(ellipse, devDecimalX * 3);
        //        Canvas.SetTop(ellipse, devDecimalY * 3);

        //        canvas1.Children.Add(ellipse);
        //    }
        //}
        #endregion
    }
}
