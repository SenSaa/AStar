using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using System;

namespace astar
{
    public class AStar
    {

        private (float, float) StartNodePos;
        private (float, float) GoalNodePos;
        private int maxIteration = 1000;
        private Graph graph;
        private CustomQueue<Node> NodeSet;
        private CustomQueue<Node> ClosedSet;
        private Dictionary<(float, float), float> VisitedSet;
        private HashSet<Node> Path;
        private HashSet<Obstacle> Obstacles;
        private HashSet<(float, float)> ObstaclesPositions;
        private Stopwatch stopwatch;
        private float elapsedTime;
        private int HeurisricMode;

        public AStar((float, float) StartNodePos, (float, float) GoalNodePos, HashSet<Obstacle> obs, int HeurisricMode, float cellSize = 1, int gridWidth = 101, int gridHeight = 101)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            this.StartNodePos = StartNodePos;
            this.GoalNodePos = GoalNodePos;
            graph = new Graph(cellSize);

            this.HeurisricMode = HeurisricMode;
            UnityEngine.Debug.Log(HeurisricMode);

            Obstacles = obs;

            ClosedSet = new CustomQueue<Node>();
            VisitedSet = new Dictionary<(float, float), float>();

            graph.SetGridWidth(gridWidth);
            graph.SetGridHeight(gridHeight);
            NodeSet = new CustomQueue<Node>();
            var startingCellPos = (graph.GetGridStartPos().Item1, graph.GetGridStartPos().Item2);
            //for (float j = cellSize; j <= graph.GetGridHeight(); j += cellSize)
            for (float j = startingCellPos.Item2; j <= graph.GetGridHeight(); j += cellSize)
            {
                //for (float i = cellSize; i <= graph.GetGridWidth(); i += cellSize)
                for (float i = startingCellPos.Item1; i <= graph.GetGridWidth(); i += cellSize)
                {
                    Node node = new Node((i, j));
                    node.SetGcost(float.PositiveInfinity); // If g-cost is not infinite...
                    node.SetHcost(float.PositiveInfinity);
                    node.SetFcost(float.PositiveInfinity);
                    NodeSet.Enqueue(node);
                }
            }

            // *
            ObstaclesPositions = GetObstaclesPositions();
        }

        public void Search()
        {
            // Initialize the queues for the open and closed set.
            CustomQueue<Node> openSet = new CustomQueue<Node>();
            CustomQueue<Node> closedSet = new CustomQueue<Node>();

            // Add the starting node to the queue.
            Node startNode = FindNearestNode(StartNodePos);
            openSet.Enqueue(startNode);

            // * Also find nearest node to the goal pos from the grid cells.
            Node goalNode = FindNearestNode(GoalNodePos);
            GoalNodePos = goalNode.GetNodePosition();

            // Initialise starting node costs as 0.
            startNode.SetGcost(0);
            startNode.SetHcost(0);
            startNode.SetFcost(0);

            // To avoid being stuck in the loop, set an iteration bound
            // while the open list is not empty
            int iteration = 0;
            while (openSet.length > 0 && iteration < maxIteration)
            {
                iteration++;

                // find the node with the least f cost in the open queue
                Node currentNode = FindNodeWithLeastFCost(openSet);

                // Deque current node off the open list
                openSet.Dequeue();

                // generate current node's neighbours
                List<Node> neighbours = FindNodeNeighbours(currentNode);
                currentNode.SetNeighbours(neighbours);

                // for each neighbour
                foreach (var neighbour in neighbours)
                {
                    // if neighbour is the goal, stop search
                    if (neighbour.GetNodePosition().Equals(GoalNodePos))
                    {
                        UnityEngine.Debug.Log("Goal!");
                        UnityEngine.Debug.Log(currentNode.GetNodePosition() + "     ,     " + neighbour.GetNodePosition());
                        ExtractPath(currentNode);
                        ClosedSet = closedSet;

                        stopwatch.Stop();
                        elapsedTime = stopwatch.ElapsedMilliseconds;

                        return;
                    }

                    // neighbour (g) cost = current node (g) cost + distance between neighbour and current node
                    neighbour.SetGcost(currentNode.GetGcost() + Heurisrics.EuclideanDistance(currentNode.GetNodePosition(), neighbour.GetNodePosition()));
                    if (HeurisricMode == 1) { neighbour.SetHcost(Heurisrics.EuclideanDistance(GoalNodePos, neighbour.GetNodePosition())); }
                    else { neighbour.SetHcost(Heurisrics.ManhattanDistance(GoalNodePos, neighbour.GetNodePosition())); }
                    // **
                    // neighbour (f) cost = neighbour (g) + neighbour (h)
                    var fCost = neighbour.GetGcost() + neighbour.GetHcost();
                    neighbour.SetFcost(fCost);

                    // *
                    //if (!VisitedSet.ContainsKey(neighbour.GetNodePosition())) { VisitedSet.Add(neighbour.GetNodePosition(), neighbour.GetFcost()); }

                    // if a node with the same position as neighbour is in the OPEN list,
                    // which has a lower f than neighbour
                    // skip this neighbour
                    if (NodeWithSamePosAndLowerFCostExists(openSet, neighbour))
                    {
                        continue;
                    }

                    // if a node with the same position as neighbour is in the CLOSED list,
                    // which has a lower f than neighbour,
                    // skip this neighbour
                    if (NodeWithSamePosAndLowerFCostExists(closedSet, neighbour))
                    {
                        continue;
                    }

                    // otherwise, add the node to the open list
                    openSet.Enqueue(neighbour);

                    neighbour.SetParent(currentNode);

                    // end(for loop)
                }

                // Enqueue current node to closed list end(while loop)
                closedSet.Enqueue(currentNode);
            }
        }


        private Node FindNearestNode((float, float) nodePos)
        {
            var queueNode = NodeSet.head;
            while (queueNode != null)
            {
                Node graphNode = queueNode.value;
                if (Mathf.Abs(nodePos.Item1 - graphNode.GetNodePosition().Item1) < graph.GetGridCellSize()
                    && Mathf.Abs(nodePos.Item2 - graphNode.GetNodePosition().Item2) < graph.GetGridCellSize())
                {
                    return graphNode;
                }
                queueNode = queueNode.next;
            }
            return null;
        }

        
        private Node FindNodeWithLeastFCost(CustomQueue<Node> openSet)
        {
            var queueNode = openSet.head;
            var lowestFcostNode = queueNode.value;
            while (queueNode != null)
            {
                Node graphNode = queueNode.value;
                if (graphNode.GetFcost() < lowestFcostNode.GetFcost())
                {
                    lowestFcostNode = graphNode;
                }
                queueNode = queueNode.next;
            }
            return lowestFcostNode;
        }


        private List<Node> FindNodeNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            ///var dirs = new List<(float, float)> { (graph.GetGridCellSize(), 0), (0, graph.GetGridCellSize()), (-graph.GetGridCellSize(), 0), (0, -graph.GetGridCellSize()) };
            var dirs = new List<(float, float)> { (graph.GetGridCellSize(), 0), (0, graph.GetGridCellSize()), (-graph.GetGridCellSize(), 0), (0, -graph.GetGridCellSize()), (graph.GetGridCellSize(), graph.GetGridCellSize()), (graph.GetGridCellSize(), -graph.GetGridCellSize()), (-graph.GetGridCellSize(), graph.GetGridCellSize()), (-graph.GetGridCellSize(), -graph.GetGridCellSize()) };
            foreach (var dir in dirs)
            {
                var neighborPos = (node.GetNodePosition().Item1 + dir.Item1, node.GetNodePosition().Item2 + dir.Item2);
                var neighbor = FindNearestNode(neighborPos);
                if (neighbor == null) { continue; }
                if (0 <= neighbor.GetNodePosition().Item1 && neighbor.GetNodePosition().Item1 < graph.GetGridWidth()
                    && 0 <= neighbor.GetNodePosition().Item2 && neighbor.GetNodePosition().Item2 < graph.GetGridHeight())
                {
                    if (NodeObstalceFree(neighbor.GetNodePosition()))
                    {
                        neighbours.Add(neighbor);
                    }
                }
            }
            return neighbours;
        }


        private bool NodeObstalceFree((float, float) nodePos)
        {
            foreach (var obstPos in ObstaclesPositions)
            {
                if (obstPos.Equals(nodePos))
                {
                    return false;
                }
            }
            return true;
        }

        private HashSet<(float, float)> GetObstaclesPositions()
        {
            var obstaclesPositions = new HashSet<(float, float)>();
            foreach (var obst in Obstacles)
            {
                obstaclesPositions.Add(FindNearestNode((obst.x, obst.y)).GetNodePosition());
            }
            return obstaclesPositions;
        }


        private bool NodeWithSamePosAndLowerFCostExists(CustomQueue<Node> nodeSet, Node currentNodeNeighbour)
        {
            var queueNode = nodeSet.head;
            while (queueNode != null)
            {
                Node graphNode = queueNode.value;
                if (currentNodeNeighbour.GetNodePosition().Equals(graphNode.GetNodePosition()))
                {
                    if (graphNode.GetFcost() <= currentNodeNeighbour.GetFcost())
                    {
                        return true;
                    }
                }
                queueNode = queueNode.next;
            }
            return false;
        }


        public void ExtractPath(Node currentNode)
        {
            HashSet<Node> path = new HashSet<Node>();
            Node goalNode = new Node(GoalNodePos);
            Node node = currentNode;
            path.Add(goalNode);
            path.Add(node);
            int iteration = 0;
            while (!node.GetNodePosition().Equals(StartNodePos)
                && iteration < NodeSet.length)
            {
                if (node.GetParent() != null)
                {
                    path.Add(node.GetParent());
                }
                else
                {
                    break;
                }
                node = node.GetParent();
            }
            Path = path;
        }


        public HashSet<Node> GetPath()
        {
            return Path;
        }


        public CustomQueue<Node> GetClosedSet()
        {
            return ClosedSet;
        }


        public string PrintClosedSetString()
        {
            string closedSetString = "";
            for (int i = 0; i < ClosedSet.length; i++)
            {
                closedSetString += ClosedSet.Get(i).GetNodePosition().Item1 + " , " + ClosedSet.Get(i).GetNodePosition().Item2 + "   |   ";
            }
            return closedSetString;
        }


        public float GetElapsedTime()
        {
            return elapsedTime;
        }


    }
}
