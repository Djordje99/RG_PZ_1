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
using PredmetniZadatak_1.Commands;
using PredmetniZadatak_1.Windows;
using System.ComponentModel;

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
        private bool _drawEllipse = false;
        private bool _drawPolygon = false;
        private bool _addText = false;
        private List<Point> mousePoligonPoints = new List<Point>();
        private List<TextBlock> elementOnCanvas = new List<TextBlock>();
        private List<TextBlock> undoElement = new List<TextBlock>();
        private bool isClear = false;

        #region Property
        public bool DrawEllipseEnable
        {
            get
            {
                return _drawEllipse;
            }
            set
            {
                if (_drawEllipse != value)
                {
                    _drawEllipse = value;

                    if (_drawEllipse &&_drawPolygon)
                    {
                        _drawPolygon = false;
                        poCheck.IsChecked = _drawPolygon;
                    }
                    if(_drawEllipse && _addText)
                    {
                        _addText = false;
                        textCheck.IsChecked = _addText;
                    }
                    OnPropertyChanged("DrawEllipseEnable");
                }
            }
        }

        public bool DrawPolygonEnable
        {
            get
            {
                return _drawPolygon;
            }
            set
            {
                if (_drawPolygon != value)
                {
                    _drawPolygon = value;
                    if (_drawPolygon && _drawEllipse)
                    {
                        _drawEllipse = false;
                        elCheck.IsChecked = _drawEllipse;
                    }
                    if(_drawPolygon && _addText)
                    {
                        _addText = false;
                        textCheck.IsChecked = _addText;
                    }
                    OnPropertyChanged("DrawPolygonEnable");
                }
            }
        }

        public bool AddTextEnable
        {
            get
            {
                return _addText;
            }
            set
            {
                if (_addText != value)
                {
                    _addText = value;
                    if (_addText && _drawEllipse)
                    {
                        _drawEllipse = false;
                        elCheck.IsChecked = _drawEllipse;
                    }
                    if(_addText && _drawPolygon)
                    {
                        _drawPolygon = false;
                        poCheck.IsChecked = _drawPolygon;
                    }
                    OnPropertyChanged("AddTextEnable");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.canvas1.MouseWheel += Canvas_Zoom;

            DrawDots();
            line = new LineFormer(dot.DotModels);
            DrawLinesBfs();
            DrawLinesCrossing();
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
            List<Polyline> lines = line.AddLineBfs();

            foreach (var item in lines)
            {
                item.MouseRightButtonDown += ShowColorDialog;

                canvas1.Children.Add(item);
            }
        }

        private void DrawLinesCrossing()
        {
            List<Polyline> lines = line.AddLineCrossing(out List<DotModel> crossingDots);

            foreach (var item in lines)
            {
                item.MouseRightButtonDown += ShowColorDialog;

                canvas1.Children.Add(item);
            }

            foreach (var item in crossingDots)
            {
                Canvas.SetLeft(item.Ellipse, item.CanvasX - 1);
                Canvas.SetTop(item.Ellipse, item.CanvasY - 1);

                try
                {
                    canvas1.Children.Add(item.Ellipse);
                }
                catch 
                {
                    continue;
                }
            }
        }

        private void ShowColorDialog(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Polyline sourceLine = e.Source as Polyline;
            string toolTipString = sourceLine.ToolTip.ToString().Split(':')[1].Trim();

            string ellipse1ID = "id_" + toolTipString.Split('\n')[8].Split(' ')[1];
            string ellipse2ID = "id_" + toolTipString.Split('\n')[9].Split(' ')[1];
        
            Ellipse ellipse1 = canvas1.Children[indexOnCanvas[ellipse1ID]] as Ellipse;
            Ellipse ellipse2 = canvas1.Children[indexOnCanvas[ellipse2ID]] as Ellipse;

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

                int x = indexCoords[indexOnCanvas[ellipse1ID]].Item1;
                int y = indexCoords[indexOnCanvas[ellipse1ID]].Item2;
                Canvas.SetLeft(e1, x);
                Canvas.SetBottom(e1, y);

                canvas1.Children.Add(e1);

                x = indexCoords[indexOnCanvas[ellipse2ID]].Item1;
                y = indexCoords[indexOnCanvas[ellipse2ID]].Item2;
                Canvas.SetLeft(e2, x);
                Canvas.SetBottom(e2, y);

                canvas1.Children.Add(e2);

            }

        }

        private void Canvas_Zoom(object sender, MouseWheelEventArgs e)
        {
            double zoomMax = 50;
            double zoomMin = 1;
            double zoomAmount = 0.25;

            zoom += zoomAmount * (e.Delta / 120);
            if (zoom < zoomMin) { zoom = zoomMin; }
            if (zoom > zoomMax) { zoom = zoomMax; }

            Point mousePos = e.GetPosition(canvas1);

            if (zoom >= 1 && zoom <= 50)
            {
                canvas1.LayoutTransform = new ScaleTransform(zoom, zoom, mousePos.X, mousePos.Y);
                scrollBar.ScrollToVerticalOffset(scrollBar.ActualHeight * (960 / mousePos.X));
                scrollBar.ScrollToHorizontalOffset(scrollBar.ActualWidth * (960 / mousePos.Y));
            }
            else
            {
                canvas1.LayoutTransform = new ScaleTransform(zoom, zoom);
                scrollBar.ScrollToVerticalOffset(scrollBar.ActualHeight * (960 / mousePos.X));
                scrollBar.ScrollToHorizontalOffset(scrollBar.ActualWidth * (960 / mousePos.Y));
            }
        }

        #region CanExecuteCommands
        private void DrawEllipse_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DrawPolygon_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddText_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Clear_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region ExecuteCommands
        private void DrawEllipse_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void DrawPolygon_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void AddText_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (elementOnCanvas.Count > 0)
            {
                canvas1.Children.Remove(elementOnCanvas[elementOnCanvas.Count - 1]);
                undoElement.Add(elementOnCanvas[elementOnCanvas.Count - 1]);
                elementOnCanvas.RemoveAt(elementOnCanvas.Count - 1);
            }

            if (isClear)
            {
                foreach (var item in undoElement)
                {
                    canvas1.Children.Add(item);
                    elementOnCanvas.Add(item);
                }

                undoElement.Clear();

                isClear = false;
            }
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (undoElement.Count > 0)
            {
                canvas1.Children.Add(undoElement[undoElement.Count - 1]);
                elementOnCanvas.Add(undoElement[undoElement.Count - 1]);
                undoElement.RemoveAt(undoElement.Count - 1);
            }
        }

        private void Clear_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var item in elementOnCanvas)
            {
                canvas1.Children.Remove(item);
                undoElement.Add(item);
            }

            elementOnCanvas.Clear();

            isClear = true;
        }
        #endregion

        #region ClickEvents
        private void RightButtonDown_Click(object sender, MouseButtonEventArgs e)
        {
            if (_drawEllipse)
            {
                Point mousePos = e.GetPosition(canvas1);

                EllipseWindow ew = new EllipseWindow();
                ew.ShowDialog();

                Ellipse el = new Ellipse()
                {
                    Width = ew.widthProp,
                    Height = ew.heightProp,
                    Fill = ew.colorFillProp,
                    StrokeThickness = ew.borderProp,
                    Stroke = ew.colorBorderProp,
                };

                VisualBrush vb = new VisualBrush();
                vb.Visual = el;
                TextBlock tb = new TextBlock()
                {
                    Text = ew.textProp,
                    Foreground = ew.colorTextProp,
                    Height = ew.heightProp + 10,
                    Width = ew.widthProp + 10,
                    TextAlignment = TextAlignment.Center,
                    Padding = new Thickness() { Top = ((ew.heightProp - 6) / 2) },
                    Margin = new Thickness() { Top = mousePos.Y - (ew.heightProp / 2), Left = mousePos.X - (ew.widthProp / 2) }
                };

                tb.Background = vb;

                elementOnCanvas.Add(tb);

                canvas1.Children.Add(tb);
            }
            else if (_drawPolygon)
            {
                Point mousePos = e.GetPosition(canvas1);

                mousePoligonPoints.Add(mousePos);

            }
            else if (_addText)
            {
                Point mousePos = e.GetPosition(canvas1);

                TextWindow tw = new TextWindow();
                tw.ShowDialog();

                TextBlock tb = new TextBlock()
                {
                    Text = tw.textProp,
                    Foreground = tw.colorTextProp,
                    TextAlignment = TextAlignment.Center,
                    FontSize = tw.textSizeProp,
                    Padding = new Thickness() { Top = mousePos.Y - tw.textSizeProp, Left = mousePos.X }
                };

                elementOnCanvas.Add(tb);

                canvas1.Children.Add(tb);
            }
        }

        private void LeftButtomDown_Click(object sender, MouseButtonEventArgs e)
        {
            if (mousePoligonPoints.Count < 3)
                return;


            PolygonWindow pw = new PolygonWindow();
            pw.ShowDialog();

            double heightMax = mousePoligonPoints.Max(x => x.Y);
            double heightMin = mousePoligonPoints.Min(x => x.Y);
            double widthMax = mousePoligonPoints.Max(x => x.X);
            double widthMin = mousePoligonPoints.Min(x => x.X);

            StackPanel sp = new StackPanel();

            Polygon polygon = new Polygon()
            {
                Points = new PointCollection(mousePoligonPoints),
                Fill = pw.colorFillProp,
                Stroke = pw.colorBorderProp,
                StrokeThickness = pw.borderProp,
            };

            TextBlock tb = new TextBlock()
            {
                Text = pw.textProp,
                Foreground = pw.colorTextProp,
                TextAlignment = TextAlignment.Center,
                Height = heightMax - heightMin,
                Width = widthMax - widthMin,
                Margin = new Thickness() { Top = heightMin, Left = widthMin },
                Padding = new Thickness() { Top = (heightMax - heightMin + 6) / 2 }

            };

            VisualBrush vb = new VisualBrush();
            vb.Visual = polygon;
            tb.Background = vb;

            elementOnCanvas.Add(tb);

            canvas1.Children.Add(tb);

            mousePoligonPoints.Clear();
        }
        #endregion
    }
}
