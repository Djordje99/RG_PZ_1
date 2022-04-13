using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetniZadatak_1.Model
{
    public class DotModel
    {
        private long id;
        private int canvasX;
        private int canvasY;

        public long Id { get => id; set => id = value; }
        public int CanvasX { get => canvasX; set => canvasX = value; }
        public int CanvasY { get => canvasY; set => canvasY = value; }

        public DotModel(long id, int canvasX, int canvasY)
        {
            this.id = id;
            this.canvasX = canvasX;
            this.canvasY = canvasY;
        }
    }
}
