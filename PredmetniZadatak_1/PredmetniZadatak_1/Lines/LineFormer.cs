using PredmetniZadatak_1.BFS;
using PredmetniZadatak_1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<Line> AddLine()
        {
            LeastToMostDistance();

            List<Line> lines = new List<Line>();

            //int i = 203;

            //if (dotModelX.ContainsKey(this.LineIDs.Item1[i]) && dotModelY.ContainsKey(this.LineIDs.Item1[i])
            //   && dotModelX.ContainsKey(this.LineIDs.Item2[i]) && dotModelY.ContainsKey(this.LineIDs.Item2[i]))
            //{
            //    int startX = dotModelX[this.LineIDs.Item1[i]];
            //    int startY = 960 - dotModelY[this.LineIDs.Item1[i]];
            //    int endX = dotModelX[this.LineIDs.Item2[i]];
            //    int endY = 960 - dotModelY[this.LineIDs.Item2[i]];

            //    int moves = bfs.Solve(startX, startY, endX, endY);

            //    List<Node> nodes = bfs.ReconstructPath(startX, startY, endX, endY);

            //    for (int j = 0; j < nodes.Count - 1; j++)
            //    {
            //        lines.Add(new Line()
            //        {
            //            X1 = nodes[j].row,
            //            Y1 = nodes[j].colum,
            //            X2 = nodes[j + 1].row,
            //            Y2 = nodes[j + 1].colum,
            //            StrokeThickness = 0.5,
            //            Stroke = Brushes.Red,
            //            Fill = Brushes.Red
            //        });
            //    }
            //}

            for (int i = 0; i < 62; i++)//distanceLineIds.Count
            {
                if (dotModelX.ContainsKey(distanceLineIds[i].Item2) && dotModelY.ContainsKey(distanceLineIds[i].Item2)
               && dotModelX.ContainsKey(distanceLineIds[i].Item3) && dotModelY.ContainsKey(distanceLineIds[i].Item3))
                {
                    int startX = dotModelX[distanceLineIds[i].Item2];
                    int startY = 960 - dotModelY[distanceLineIds[i].Item2];
                    int endX = dotModelX[distanceLineIds[i].Item3];
                    int endY = 960 - dotModelY[distanceLineIds[i].Item3];

                    int moves = bfs.Solve(startX, startY, endX, endY);

                    List<Node> nodes = bfs.ReconstructPath(startX, startY, endX, endY);

                    for (int j = 0; j < nodes.Count - 1; j++)
                    {
                        lines.Add(new Line()
                        {
                            X1 = nodes[j].row,
                            Y1 = nodes[j].colum,
                            X2 = nodes[j + 1].row,
                            Y2 = nodes[j + 1].colum,
                            StrokeThickness = 0.5,
                            Stroke = Brushes.Red,
                            Fill = Brushes.Red
                        });
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
