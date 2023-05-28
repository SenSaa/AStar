using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using astar;
using System.Linq;
using System;
using System.Threading;
using Utils;

[RequireComponent(typeof(LineRenderer))]
public class RunAStar : MonoBehaviour
{

    [SerializeField] private Transform StartTransform;
    [SerializeField] private Transform GoalTransform;

    [SerializeField] private Vector3 StartPos;
    [SerializeField] private Vector3 GoalPos;
    private Heurisrics.HeurisricMode HeurisricMode;
    private int HeuristicDropdownValue;
    private float GridCellSize;

    private void Awake()
    {
        EventManager.SearchInputEvent += HandleSearchInputEvent;
    }

    private void HandleSearchInputEvent(object o, EventManager.SearchInputEventArgs e)
    {
        StartPos = e.StartPosition;
        GoalPos = e.GoalPosition;
        HeurisricMode = e.HeurisricMode;
        HeuristicDropdownValue = e.HeuristicDropdownValue;
        GridCellSize = e.GridCellSize;
        Search();
    }

    //void Start()
    private void Search()

    {
        //Vector3 StartPos = new Vector3(StartTransform.position.x, StartTransform.position.y, StartTransform.position.z);
        //Vector3 GoalPos = new Vector3(GoalTransform.position.x, GoalTransform.position.y, GoalTransform.position.z);
        var startNodePos = (StartPos.x, StartPos.z);
        var goalNodePos = (GoalPos.x, GoalPos.z);

        //float gridCellSize = 1f;
        int gridWidth = Mathf.CeilToInt(goalNodePos.x + startNodePos.x);
        int gridHeight = Mathf.CeilToInt(goalNodePos.z + startNodePos.z);

        var obstacles = new HashSet<Obstacle>();
        /*
        int x_step = 5;
        int z_step = 5;
        for (int y = 1; y < gridHeight / 2; y += x_step)
        {
            for (int x = 1; x < gridWidth / 2; x += z_step)
            {
                if (x != StartPos.x && y != StartPos.z && x != GoalPos.x && y != GoalPos.z)
                {
                    obstacles.Add(new Obstacle(x, y));
                }
            }
        }
        */

        ///var astar = new AStar(startNodePos, goalNodePos, obstacles, gridCellSize, gridWidth, gridHeight);
        var astar = new AStar(startNodePos, goalNodePos, obstacles, HeuristicDropdownValue, GridCellSize, gridWidth, gridHeight);
        astar.Search();
        Debug.Log(astar.PrintClosedSetString());
        var path = astar.GetPath();

        Debug.Log("Time in ms = " + astar.GetElapsedTime());
        Debug.Log("Time in s = " + Mathf.Round(astar.GetElapsedTime() / 1000));
        
        DrawPath(path);
        DrawPoints(path);
        DrawPoints(astar.GetClosedSet());
    }

    void Update()
    {

    }

    private void DrawPath(HashSet<Node> path)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = path.Count;
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < path.Count; i++)
        {
            pos.Set(path.ElementAt(i).GetNodePosition().Item1, 0.1f, path.ElementAt(i).GetNodePosition().Item2);
            lineRenderer.SetPosition(i, pos);
        }
        lineRenderer.widthMultiplier = 0.3f;
    }

    private void DrawPoints(HashSet<Node> path)
    {
        Vector3 pos = Vector3.zero;
        Vector3 scale = new Vector3(0.9f, 0.01f, 0.9f);
        for (int i = 0; i < path.Count; i++)
        {
            pos.Set(path.ElementAt(i).GetNodePosition().Item1, 0.01f, path.ElementAt(i).GetNodePosition().Item2);
            GameObject pointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointObj.transform.position = pos;
            pointObj.transform.localScale = scale;
        }
    }
    private void DrawPoints(CustomQueue<Node> path)
    {
        Vector3 pos = Vector3.zero;
        Vector3 scale = new Vector3(0.9f, 0.01f, 0.9f);
        for (int i = 0; i < path.length; i++)
        {
            pos.Set(path.Get(i).GetNodePosition().Item1, 0.01f, path.Get(i).GetNodePosition().Item2);
            GameObject pointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointObj.transform.position = pos;
            pointObj.transform.localScale = scale;
        }
    }

}
