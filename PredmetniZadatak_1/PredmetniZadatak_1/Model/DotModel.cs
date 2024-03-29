﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PredmetniZadatak_1.Model
{
    public class DotModel
    {
        private long id;
        private int canvasX;
        private int canvasY;
        private Ellipse ellipse;

        public long Id { get => id; set => id = value; }
        public int CanvasX { get => canvasX; set => canvasX = value; }
        public int CanvasY { get => canvasY; set => canvasY = value; }
        public Ellipse Ellipse { get => ellipse; set => ellipse = value; }

        public DotModel(long id, int canvasX, int canvasY)
        {
            this.id = id;
            this.canvasX = canvasX;
            this.canvasY = canvasY;
        }

        public DotModel(int canvasX, int canvasY, Ellipse ellipse)
        {
            this.canvasX = canvasX;
            this.canvasY = canvasY;
            this.ellipse = ellipse;
        }
    }
}
