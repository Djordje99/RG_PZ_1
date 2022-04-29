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
            List<DotModel> crossingDotsTemp = new List<DotModel>();
            int[,] lineMatrix = bfs.Matrix;

            int aX, aY, bX, bY;

            foreach (var item in leftLines)
            {
                ToolTip toolTip = new ToolTip();
                toolTip.Content = item.Item2;
                int startCoord = 0;
                int crossingCount = 0;

                aY = 960 - dotModelY[item.Item3];
                bY = 960 - dotModelY[item.Item4];
                aX = dotModelX[item.Item3];
                bX = dotModelX[item.Item4];

                startCoord = aX > bX ? bX : aX;

                for (int i = 0; i < Math.Abs(aX - bX); i++)
                {
                    if (lineMatrix[startCoord + i, aY] >= 1)
                    {
                        crossingCount++;
                    }

                    //lineMatrix[startCoord + i, aY]++;
                }

                startCoord = aY > bY ? bY : aY;

                for (int i = 0; i < Math.Abs(aY - bY); i++)
                {
                    if (lineMatrix[bX, startCoord + i] >= 1)
                    {
                        crossingCount++;
                    }

                    //lineMatrix[bX, startCoord + i]++;
                }

                if(crossingCount < 20)
                {
                    startCoord = aX > bX ? bX : aX;
                    for (int i = 0; i < Math.Abs(aX - bX); i++)
                    {
                        lineMatrix[startCoord + i, aY]++;
                    }

                    startCoord = aY > bY ? bY : aY;
                    for (int i = 0; i < Math.Abs(aY - bY); i++)
                    {
                        lineMatrix[bX, startCoord + i]++;
                    }
                }
                else
                {
                    continue;
                }

                //if (crossingCount >= 15)
                //{

                //    continue;
                //}

                //if (crossingCount >= 15)
                //{
                //    for (int i = 0; i < Math.Abs(aY - bY); i++)
                //    {
                //        lineMatrix[bX, startCoord + i]--;
                //    }
                //    continue;
                //}

                Polyline polyline = new Polyline();
                polyline.ToolTip = toolTip;
                polyline.Stroke = Brushes.Purple;
                polyline.StrokeThickness = 0.5;
                polyline.Points.Add(new System.Windows.Point(aX + 1, aY - 1));
                polyline.Points.Add(new System.Windows.Point(bX + 1, aY - 1));
                polyline.Points.Add(new System.Windows.Point(bX + 1, bY - 1));

                lines.Add(polyline);
            }

            crossingDots = FindIntersection(lineMatrix);

            return lines;
        }

        private List<DotModel> FindIntersection(int[,] lineMatrix)
        {
            List<DotModel> intersections = new List<DotModel>();

            for (int i = 20; i < 960; i++)
            {
                for (int j = 20; j < 960; j++)
                {
                    //if (lineMatrix[i, j] >= 2)
                    //{
                    //    intersections.Add(new DotModel(i, j, new Ellipse() { Fill = Brushes.Green, Height = 1.5, Width = 1.5 }));
                    //}
                    if (lineMatrix[i, j] >= 2
                        && lineMatrix[i + 1, j] <= 1
                        && lineMatrix[i - 1, j] <= 1
                        && lineMatrix[i, j - 1] <= 1
                        && lineMatrix[i, j + 1] <= 1)
                        //&& lineMatrix[i + 1, j + 1] == 0
                        //&& lineMatrix[i - 1, j + 1] == 0
                        //&& lineMatrix[i + 1, j - 1] == 0
                        //&& lineMatrix[i - 1, j - 1] == 0)
                    {
                        intersections.Add(new DotModel(i, j, new Ellipse() { Fill = Brushes.Green, Height = 1.5, Width = 1.5 }));
                    }
                }
            }

            return intersections;
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
