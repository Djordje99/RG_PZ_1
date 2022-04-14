using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetniZadatak_1.BFS
{
    public struct Node
    {
        public int row;
        public int colum;
    }

    public class BfsAlgorithm
    {
        private int rowSize = 960;
        private int columSize = 960;
        private int[,] matrix;
        private Queue<int> rowQueue = new Queue<int>();
        private Queue<int> columQueue = new Queue<int>();
        private int moveCount = 0;
        private int nodesLeftInLayer = 1;
        private int nodesInNextLayer = 0;
        private bool isEnd = false;
        private bool[,] visitedNodes;
        private int[] directionRow = new int[] { -1, 1, 0, 0 };
        private int[] directionColum = new int[] { 0, 0, 1, -1 };
        private Node[,] prev;

        public BfsAlgorithm()
        {
            matrix = new int[rowSize, columSize];
            visitedNodes = new bool[rowSize, columSize];
            prev = new Node[rowSize, columSize];

            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columSize; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        public int Solve(int startRow, int startColum, int endRow, int endColum)
        {
            InitStart();

            int currentRow, currentColum;

            rowQueue.Enqueue(startRow);
            columQueue.Enqueue(startColum);
            visitedNodes[startRow, startColum] = true;

            while(rowQueue.Count > 0)
            {
                currentRow = rowQueue.Dequeue();
                currentColum = columQueue.Dequeue();

                if(currentRow == endRow && currentColum == endColum)
                {
                    isEnd = true;
                    break;
                }

                ExploreNodes(currentRow, currentColum);

                nodesLeftInLayer--;

                if(nodesLeftInLayer == 0)
                {
                    nodesLeftInLayer = nodesInNextLayer;
                    nodesInNextLayer = 0;
                    moveCount++;
                }
            }

            if (isEnd)
                return moveCount;

            return -1;
        }

        private void ExploreNodes(int currentRow, int currentColum)
        {
            for (int i = 0; i < 4; i++)
            {
                int nextRow = currentRow + directionRow[i];
                int nextColum = currentColum + directionColum[i];

                if (nextRow < 0 || nextColum < 0)
                    continue;
                if (nextRow >= rowSize || nextColum >= columSize)
                    continue;
                if (visitedNodes[nextRow, nextColum])
                    continue;
                if (matrix[nextRow, nextColum] == 1)
                    continue;

                rowQueue.Enqueue(nextRow);
                columQueue.Enqueue(nextColum);

                visitedNodes[nextRow, nextColum] = true;
                nodesInNextLayer++;

                Node node = new Node();
                node.row = currentRow;
                node.colum = currentColum;

                prev[nextRow, nextColum] = node;
            }
        }

        public List<Node> ReconstructPath(int startRow, int startColum, int endRow, int endColum)
        {
            List<Node> path = new List<Node>();

            int rowRead = endRow;
            int columRead = endColum;

            for (int i = 0; i < moveCount + 1; i++)
            {
                Node node = new Node();
                node.row = rowRead;
                node.colum = columRead;

                path.Add(node);

                Node n = prev[rowRead, columRead];

                rowRead = n.row;
                columRead = n.colum;
            }

            path.Reverse();

            if(path[0].row == startRow && path[0].colum == startColum)
            {
                foreach (var item in path)
                {
                    matrix[item.row, item.colum] = 1;
                }
                return path;
            }

            path.Clear();
            return path;
        }

        private void InitStart()
        {
            rowQueue.Clear();
            columQueue.Clear();
            moveCount = 0;
            nodesLeftInLayer = 1;
            nodesInNextLayer = 0;
            isEnd = false;
            visitedNodes = new bool[rowSize, columSize];
            prev = new Node[rowSize, columSize];
        }
    }
}
