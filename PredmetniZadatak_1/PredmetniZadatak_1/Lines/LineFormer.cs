using PredmetniZadatak_1.BFS;
using PredmetniZadatak_1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PredmetniZadatak_1.Lines
{
    public class LineFormer : LineScaler
    {
        List<DotModel> dotModels = new List<DotModel>();
        Dictionary<long, int> dotModelX = new Dictionary<long, int>();
        Dictionary<long, int> dotModelY = new Dictionary<long, int>();
        BfsAlgorithm bfs = new BfsAlgorithm();
        private List<Tuple<double, string, long, long>> leftLines = new List<Tuple<double, string, long, long>>();
        private List<Tuple<double, string, long, long>> distanceLineIds = new List<Tuple<double, string, long, long>>();

        public LineFormer(List<DotModel> dotModels) : base()
        {
            this.dotModels = dotModels;
            foreach (var item in dotModels)
            {
                dotModelX.Add(item.Id, item.CanvasX);
                dotModelY.Add(item.Id, item.CanvasY);
            }
        }

        public List<Polyline> AddLineBfs()
        {
            LeastToMostDistance();

            List<Polyline> lines = new List<Polyline>();

            foreach (var line in distanceLineIds)
            {
                if (dotModelX.ContainsKey(line.Item3) && dotModelY.ContainsKey(line.Item3)
              && dotModelX.ContainsKey(line.Item4) && dotModelY.ContainsKey(line.Item4))
                {
                    int startX = dotModelX[line.Item3];
                    int startY = 960 - dotModelY[line.Item3];
                    int endX = dotModelX[line.Item4];
                    int endY = 960 - dotModelY[line.Item4];

                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = line.Item2;

                    int moves = bfs.Solve(startX, startY, endX, endY);

                    if (moves == -1)
                    {
                        leftLines.Add(line);
                        continue;
                    }

                    Polyline nodes = bfs.ReconstructPath(startX, startY, endX, endY);

                    if (nodes.Points.Count == 0)
                    {
                        leftLines.Add(line);
                        continue;
                    }

                    nodes.ToolTip = toolTip;
                    nodes.Stroke = Brushes.Red;
                    nodes.StrokeThickness = 0.5;
                    lines.Add(nodes);
                }
            }

            return lines.Distinct<Polyline>().ToList();
        }

        public List<Polyline> AddLineCrossing(out List<DotModel> crossingDots)
        {
            List<Polyline> lines = new List<Polyline>();
            crossingDots = new List<DotModel>();
            List<DotModel> crossingDotsTemp = new List<DotModel>();
            int[,] lineMatrix = bfs.Matrix;
            int disposedLines = 0;
            DotModel prevDot = null;

            int aX, aY, bX, bY;

            foreach (var item in leftLines)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.Content = item.Item2;
                int startCoord = 0;

                aY = 960 - dotModelY[item.Item3];
                bY = 960 - dotModelY[item.Item4];
                aX = dotModelX[item.Item3];
                bX = dotModelX[item.Item4];

                startCoord = aX > bX ? bX : aX;

                for (int i = 0; i < Math.Abs(aX - bX); i++)
                {
                    if (lineMatrix[startCoord + i, aY] == 1)
                    {
                        crossingDotsTemp.Add(new DotModel(startCoord + i + 1, aY - 1,
                            new Ellipse() { Fill = Brushes.Green, Height = 2, Width = 2 }));
                    }
                }

                startCoord = aY > bY ? bY : aY;

                for (int i = 0; i < Math.Abs(aY - bY); i++)
                {
                    if (lineMatrix[bX, startCoord + i] == 1)
                    {
                        crossingDotsTemp.Add(new DotModel(bX + 1, startCoord + i - 1,
                            new Ellipse() { Fill = Brushes.Green, Height = 2, Width = 2 }));
                    }
                }

                if(crossingDotsTemp.Count > 15)
                {
                    crossingDotsTemp.Clear();
                    disposedLines++;
                    continue;
                }
                else
                {
                    foreach (var crossDot in crossingDotsTemp)
                    {
                        if(prevDot != null && (prevDot.CanvasX != crossDot.CanvasX || prevDot.CanvasY != crossDot.CanvasY))
                        {
                            lineMatrix[crossDot.CanvasX - 1, crossDot.CanvasY + 1] = 1;
                            crossingDots.Add(crossDot);
                        }
                        prevDot = crossDot;
                    }
                }

                Polyline polyline = new Polyline();
                polyline.ToolTip = toolTip;
                polyline.Stroke = Brushes.Purple;
                polyline.StrokeThickness = 0.5;
                polyline.Points.Add(new System.Windows.Point(aX + 1, aY - 1));
                polyline.Points.Add(new System.Windows.Point(bX + 1, aY - 1));
                polyline.Points.Add(new System.Windows.Point(bX + 1, bY - 1));

                lines.Add(polyline);
            }

            return lines;
        }

        private void LeastToMostDistance()
        {
            foreach (var line in LineIDs)
            {
                if (dotModelX.ContainsKey(line.Item2) && dotModelY.ContainsKey(line.Item3)
              && dotModelX.ContainsKey(line.Item3) && dotModelY.ContainsKey(line.Item3))
                {
                    int startX = dotModelX[line.Item2];
                    int startY = 960 - dotModelY[line.Item2];
                    int endX = dotModelX[line.Item3];
                    int endY = 960 - dotModelY[line.Item3];

                    double distance = Math.Sqrt((Math.Pow(startX - endX, 2) + Math.Pow(startY - endY, 2)));

                    distanceLineIds.Add(new Tuple<double, string, long, long>(distance, line.Item1, line.Item2, line.Item3));
                }
            }

            distanceLineIds = distanceLineIds.OrderBy(x => x.Item1).ToList();
        }
    }
}
