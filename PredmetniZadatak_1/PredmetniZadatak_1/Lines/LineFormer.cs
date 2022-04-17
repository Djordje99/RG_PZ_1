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
        private List<Tuple<double, long, long>> leftLines = new List<Tuple<double, long, long>>();
        private List<Tuple<double, long, long>> distanceLineIds = new List<Tuple<double, long, long>>();

        public LineFormer(List<DotModel> dotModels) : base()
        {
            this.dotModels = dotModels;
            foreach (var item in dotModels)
            {
                dotModelX.Add(item.Id, item.CanvasX);
                dotModelY.Add(item.Id, item.CanvasY);
            }
        }

        public List<Line> AddLineBfs()
        {
            LeastToMostDistance();

            List<Line> lines = new List<Line>();

            for (int i = 0; i < distanceLineIds.Count; i++)//distanceLineIds.Count
            {
                if (dotModelX.ContainsKey(distanceLineIds[i].Item2) && dotModelY.ContainsKey(distanceLineIds[i].Item2)
               && dotModelX.ContainsKey(distanceLineIds[i].Item3) && dotModelY.ContainsKey(distanceLineIds[i].Item3))
                {
                    int startX = dotModelX[distanceLineIds[i].Item2];
                    int startY = 960 - dotModelY[distanceLineIds[i].Item2];
                    int endX = dotModelX[distanceLineIds[i].Item3];
                    int endY = 960 - dotModelY[distanceLineIds[i].Item3];

                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = string.Format("{0}-{1}", distanceLineIds[i].Item2, distanceLineIds[i].Item3);

                    int moves = bfs.Solve(startX, startY, endX, endY);

                    if (moves == -1)
                    {
                        leftLines.Add(distanceLineIds[i]);
                        continue;
                    }

                    List<Node> nodes = bfs.ReconstructPath(startX, startY, endX, endY);

                    if(nodes.Count == 0)
                    {
                        leftLines.Add(distanceLineIds[i]);
                    }

                    for (int j = 0; j < nodes.Count - 1; j++)
                    {
                        lines.Add(new Line()
                        {
                            X1 = nodes[j].row + 1.25,
                            Y1 = nodes[j].colum - 1.25,
                            X2 = nodes[j + 1].row + 1.25,
                            Y2 = nodes[j + 1].colum - 1.25,
                            StrokeThickness = 0.5,
                            Stroke = Brushes.Red,
                            Fill = Brushes.Red,
                            ToolTip = toolTip,
                        });
                    }
                }
            }

            return lines;
        }

        public List<Line> AddLineCrossing(out List<DotModel> crossingDots)
        {
            List<Line> lines = new List<Line>();
            crossingDots = new List<DotModel>();
            int[,] lineMatrix = bfs.Matrix;

            int a, b, startCoord = 0;

            foreach (var item in leftLines)
            {
                lines.Add(new Line()
                {
                    X1 = dotModelX[item.Item2],
                    Y1 = 960 - dotModelY[item.Item2],
                    X2 = dotModelX[item.Item2],
                    Y2 = 960 - dotModelY[item.Item3],
                    StrokeThickness = 0.5,
                    Stroke = Brushes.Purple,
                    Fill = Brushes.Purple
                });

                a = 960 - dotModelY[item.Item2];
                b = 960 - dotModelY[item.Item3];

                startCoord = a > b ? b : a;

                for (int i = 0; i < Math.Abs(a - b); i++)
                {
                    if(lineMatrix[dotModelX[item.Item2], startCoord + i] == 1)
                    {
                        crossingDots.Add(new DotModel(dotModelX[item.Item2], startCoord + i, new Ellipse() { Fill = Brushes.Green, Height = 2, Width = 2 }));
                    }
                    else
                    {
                        lineMatrix[dotModelX[item.Item2], startCoord + i] = 1;
                    }
                }

                lines.Add(new Line()
                {
                    X1 = dotModelX[item.Item2],
                    Y1 = 960 - dotModelY[item.Item3],
                    X2 = dotModelX[item.Item3],
                    Y2 = 960 - dotModelY[item.Item3],
                    StrokeThickness = 0.5,
                    Stroke = Brushes.Purple,
                    Fill = Brushes.Purple
                });

                a = dotModelX[item.Item2];
                b = dotModelX[item.Item3];

                startCoord = a > b ? b : a;

                for (int i = 0; i < Math.Abs(a - b); i++)
                {
                    if (lineMatrix[startCoord + i, dotModelY[item.Item3]] == 1)
                    {
                        crossingDots.Add(new DotModel(startCoord + i, dotModelY[item.Item3], new Ellipse() { Fill = Brushes.Green, Height = 2, Width = 2 }));
                    }
                    else
                    {
                        lineMatrix[startCoord + i, 960 - dotModelY[item.Item3]] = 1;
                    }
                }
            }

            return lines;
        }

        private void LeastToMostDistance()
        {
            for (int i = 0; i < this.LineIDs.Item1.Count; i++)//this.LineIDs.Item1.Count
            {
                if (dotModelX.ContainsKey(this.LineIDs.Item1[i]) && dotModelY.ContainsKey(this.LineIDs.Item1[i])
               && dotModelX.ContainsKey(this.LineIDs.Item2[i]) && dotModelY.ContainsKey(this.LineIDs.Item2[i]))
                {
                    int startX = dotModelX[this.LineIDs.Item1[i]];
                    int startY = 960 - dotModelY[this.LineIDs.Item1[i]];
                    int endX = dotModelX[this.LineIDs.Item2[i]];
                    int endY = 960 - dotModelY[this.LineIDs.Item2[i]];

                    double distance = Math.Sqrt((Math.Pow(startX - endX, 2) + Math.Pow(startY - endY, 2)));

                    distanceLineIds.Add(new Tuple<double, long, long>(distance, this.LineIDs.Item1[i], this.LineIDs.Item2[i]));
                }
            }

            distanceLineIds = distanceLineIds.OrderBy(x => x.Item1).ToList();
        }
    }
}
