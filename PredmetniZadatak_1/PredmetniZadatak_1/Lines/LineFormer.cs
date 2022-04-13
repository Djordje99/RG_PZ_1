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
            List<Line> lines = new List<Line>();

            for (int i = 0; i < this.LineIDs.Item1.Count; i++)
            {
                if(dotModelX.ContainsKey(this.LineIDs.Item1[i]) && dotModelY.ContainsKey(this.LineIDs.Item1[i]) 
                    && dotModelX.ContainsKey(this.LineIDs.Item2[i]) && dotModelY.ContainsKey(this.LineIDs.Item2[i]))
                {
                    lines.Add(new Line()
                    {
                        X1 = dotModelX[this.LineIDs.Item1[i]],
                        Y1 = 960 - dotModelY[this.LineIDs.Item1[i]],
                        X2 = dotModelX[this.LineIDs.Item2[i]],
                        Y2 = 960 - dotModelY[this.LineIDs.Item1[i]],
                        StrokeThickness = 0.5,
                        Stroke = Brushes.Black,
                        Fill = Brushes.Black
                    });

                    lines.Add(new Line()
                    {
                        X1 = dotModelX[this.LineIDs.Item2[i]],
                        Y1 = 960 - dotModelY[this.LineIDs.Item1[i]],
                        X2 = dotModelX[this.LineIDs.Item2[i]],
                        Y2 = 960 - dotModelY[this.LineIDs.Item2[i]],
                        StrokeThickness = 0.5,
                        Stroke = Brushes.Black,
                        Fill = Brushes.Black
                    });
                }
            }

            return lines;
        }
    }
}
