using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace astar
{
    public class Node
    {

        private string Name;
        private (float, float) Position;
        private float gCost;
        private float hCost;
        private float fCost;
        private List<Node> Neighbours;
        private Node Parent;

        public Node((float, float) Position)
        {
            this.Position = Position;
        }

        public Node(string Name, (float, float) Position)
        {
            this.Name = Name;
            this.Position = Position;
        }


        public (float, float) GetNodePosition()
        {
            return Position;
        }


        public void SetGcost(float cost)
        {
            this.gCost = cost;
        }
        public float GetGcost()
        {
            return this.gCost;
        }

        public void SetHcost(float cost)
        {
            this.hCost = cost;
        }
        public float GetHcost()
        {
            return this.hCost;
        }

        public void SetFcost(float cost)
        {
            this.fCost = cost;
        }
        public float GetFcost()
        {
            return this.fCost;
        }

        public void SetNeighbours(List<Node> Neighbours)
        {
            this.Neighbours = Neighbours;
        }
        public List<Node> GetNeighbours()
        {
            return this.Neighbours;
        }

        public void SetParent(Node Parent)
        {
            this.Parent = Parent;
        }

        public Node GetParent()
        {
            return this.Parent;
        }

    }
}
