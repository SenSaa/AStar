using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired by the detailed and well written explanation here:
// http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html

namespace astar
{
    public class Heurisrics
    {

        public static float ManhattanDistance((float,float) nodePos, (float, float) goalPos, float weight=1)
        {
            var dx = Mathf.Abs(nodePos.Item1 - goalPos.Item1);
            var dy = Mathf.Abs(nodePos.Item2 - goalPos.Item2);
            var d = (dx + dy);

            var weightedDist = d * weight;
            return weightedDist;
        }

        public static float DiagonalDistance((float, float) nodePos, (float, float) goalPos, float weight = 1)
        {
            var dx = Mathf.Abs(nodePos.Item1 - goalPos.Item1);
            var dy = Mathf.Abs(nodePos.Item2 - goalPos.Item2);
            var d = ((dx + dy) + (-1) * Mathf.Min(dx, dy));

            var weightedDist = d * weight;
            return weightedDist;
        }

        public static float EuclideanDistance((float, float) nodePos, (float, float) goalPos, float weight = 1)
        {
            var dx = Mathf.Abs(nodePos.Item1 - goalPos.Item1);
            var dy = Mathf.Abs(nodePos.Item2 - goalPos.Item2);
            var d = Mathf.Sqrt(dx * dx + dy * dy);

            var weightedDist = d * weight;
            return weightedDist;
        }

    }
}
