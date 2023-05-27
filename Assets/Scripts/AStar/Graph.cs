using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace astar
{
    public class Graph
    {
        private float GridCellSize;
        private int GridWidth;
        private int GridHeight;
        private (float, float) GridStartPos;

        public Graph(float GridCellSize)
        {
            this.GridCellSize = GridCellSize;
        }

        public void SetGridCellSize(float GridCellSize)
        {
            this.GridCellSize = GridCellSize;
        }
        public float GetGridCellSize()
        {
            return this.GridCellSize;
        }

        public void SetGridWidth(int GridWidth)
        {
            this.GridWidth = GridWidth;
        }
        public int GetGridWidth()
        {
            return this.GridWidth;
        }

        public void SetGridHeight(int GridHeight)
        {
            this.GridHeight = GridHeight;
        }
        public int GetGridHeight()
        {
            return this.GridHeight;
        }
        
        public void SetGridStartPos((float, float) GridStartPos)
        {
            this.GridStartPos = GridStartPos;
        }
        public (float, float) GetGridStartPos()
        {
            return GridStartPos;
        }

    }
}