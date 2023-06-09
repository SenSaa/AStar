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
    private HashSet<Obstacle> Obstacles;

    private void Awake()
    {
        EventManager.SearchInputEvent += HandleSearchInputEvent;
    }

    private void HandleSearchInputEvent(object o, EventManager.SearchInputEventArgs e)
    {
        StartPos = e.StartPosition;
        GoalPos = e.GoalPosition;
        Obstacles = e.Obstacles;
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

        ///var astar = new AStar(startNodePos, goalNodePos, obstacles, gridCellSize, gridWidth, gridHeight);
        var astar = new AStar(startNodePos, goalNodePos, Obstacles, HeuristicDropdownValue, GridCellSize, gridWidth, gridHeight);
        astar.Search();
        Debug.Log(astar.PrintClosedSetString());
        var path = astar.GetPath();

        Debug.Log("Time in ms = " + astar.GetElapsedTime());
        Debug.Log("Time in s = " + Mathf.Round(astar.GetElapsedTime() / 1000));
        
        DrawPath(path);
        DrawPoints(path, GridCellSize);
        DrawPoints(astar.GetClosedSet(), GridCellSize);
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

    private void DrawPoints(HashSet<Node> path, float size)
    {
        Vector3 pos = Vector3.zero;
        //Vector3 scale = new Vector3(0.9f, 0.01f, 0.9f);
        Vector3 scale = new Vector3(size, 0.01f, size);
        for (int i = 0; i < path.Count; i++)
        {
            pos.Set(path.ElementAt(i).GetNodePosition().Item1, 0.01f, path.ElementAt(i).GetNodePosition().Item2);
            GameObject pointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointObj.transform.position = pos;
            pointObj.transform.localScale = scale;
        }
    }
    private void DrawPoints(CustomQueue<Node> path, float size)
    {
        Vector3 pos = Vector3.zero;
        //Vector3 scale = new Vector3(0.9f, 0.01f, 0.9f);
        Vector3 scale = new Vector3(size, 0.01f, size);
        for (int i = 0; i < path.length; i++)
        {
            pos.Set(path.Get(i).GetNodePosition().Item1, 0.01f, path.Get(i).GetNodePosition().Item2);
            GameObject pointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointObj.transform.position = pos;
            pointObj.transform.localScale = scale;
        }
    }

}
