using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

[RequireComponent(typeof(Camera))]
public class EnviroManager : MonoBehaviour
{

    [SerializeField] private Vector3 StartPosition; // Name of RNDF file with position data.
    [SerializeField] private Vector3 GoalPosition; // Name of RNDF file with position data. 
    [SerializeField] private GameObject CursorPrefab;
    [SerializeField] private GameObject CursorPositionText;
    private GameObject Cursor;
    [SerializeField] private Vector3 MouseWorldPos;
    private Camera Camera;
    [SerializeField] private Dropdown HeuristicDropdown;
    [SerializeField] private astar.Heurisrics.HeurisricMode HeuristicMode;
    [SerializeField] private int HeuristicDropdownValue;
    [SerializeField] private float GridCellSize;
    private HashSet<Obstacle> Obstacles;

    private enum Modes
    {
        _,
        SetStartMode,
        SetGoalMode
    }
    [SerializeField] private Modes Mode;

    void Start()
    {
        Camera = GetComponent<Camera>();
        Cursor = Instantiate(CursorPrefab);
        Mode = Modes._;

        HeuristicDropdownInit();

        GridCellSize = 1;

        Obstacles = new HashSet<Obstacle>();
    }

    private void HeuristicDropdownInit()
    {
        var manhattanOption = new Dropdown.OptionData();
        manhattanOption.text = "Manhattan";
        var euclideanOption = new Dropdown.OptionData();
        euclideanOption.text = "Euclidean";
        var options = new List<Dropdown.OptionData>() { manhattanOption, euclideanOption };
        HeuristicDropdown.AddOptions(options);

        HeuristicDropdown.onValueChanged.AddListener(delegate {
            OnDropdownValueChanged(HeuristicDropdown);
        });
    }


    void Update()
    {
        OnMouseClick();
        UpdateMousePosition();
        UpdateCursor();
        MoveCamera();
    }

    private void OnMouseClick()
    {
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) // Left click while ignoring scene object!
        {
            if (Mode == Modes.SetStartMode)
            {
                StartPosition = MouseWorldPos;
            }
            else if (Mode == Modes.SetGoalMode)
            {
                GoalPosition = MouseWorldPos;
            }
            if (Mode != Modes._) { Mode = Modes._; }
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) // Left click while ignoring scene object!
        {
            Obstacles.Add(new Obstacle(MouseWorldPos.x, MouseWorldPos.z));
            DrawPoints(MouseWorldPos, GridCellSize);
        }
    }

    private bool IsShiftKeyPressed()
    {
        return Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
    }

    private void UpdateMousePosition()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1f);
        Ray ray = Camera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 1100f))
        {
            MouseWorldPos = hit.point;
            DetermineHoveredObject(hit);
            return;
        }

        MouseWorldPos = Camera.ScreenToWorldPoint(mousePos);
    }

    private void DetermineHoveredObject(RaycastHit raycastHit)
    {
        Collider currentCollider = raycastHit.collider;
        GameObject currentObjectAtMousePos;

        if (currentCollider.attachedRigidbody != null)
        {
            currentObjectAtMousePos = currentCollider.attachedRigidbody.gameObject;
        }
        else
        {
            currentObjectAtMousePos = currentCollider.gameObject;
        }
    }

    private void UpdateCursor()
    {
        if (Cursor != null)
        {
            Vector3 mousePos = new Vector3(MouseWorldPos.x, 0.5f, MouseWorldPos.z);

            Cursor.transform.position = mousePos;

            if (Mode == Modes.SetStartMode)
            {
                Cursor.GetComponent<Light>().color = Color.cyan;
            }
            else if (Mode == Modes.SetGoalMode)
            {
                Cursor.GetComponent<Light>().color = Color.yellow;
            }
            else
            {
                Cursor.GetComponent<Light>().color = Color.gray;
            }

            UpdateCursorPositionTextComponent();
        }
    }

    private void UpdateCursorPositionTextComponent()
    {
        Text cursorPositionText = CursorPositionText.GetComponent<Text>();
        cursorPositionText.text = Cursor.transform.position.x + ", " + Cursor.transform.position.z;
    }

    private void MoveCamera()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        float ZoomInput = Input.GetAxisRaw("Mouse ScrollWheel");
        float speed = 100;
        if (IsShiftKeyPressed())
            speed = 300;

        if (ZoomInput != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - ZoomInput * speed, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x + horizontalInput * Time.unscaledDeltaTime * speed, transform.position.y, transform.position.z + verticalInput * Time.unscaledDeltaTime * speed);
        }
    }


    public void OnSelectStartPosition()
    {
        Mode = Modes.SetStartMode;
    }

    public void OnSelectGoalPosition()
    {
        Mode = Modes.SetGoalMode;
    }

    public void OnStartSearch()
    {
        BroadcastUserInputData();
    }

    void OnDropdownValueChanged(Dropdown change)
    {
        HeuristicDropdownValue = change.value;
        HeuristicMode = change.value == 1 ? astar.Heurisrics.HeurisricMode.Euclidean : astar.Heurisrics.HeurisricMode.Manhattan;
    }

    public void OnGridCellSizeValueChanged(InputField inputField)
    {
        GridCellSize = float.Parse(inputField.text);
    }

    private void BroadcastUserInputData()
    {
        EventManager.SearchInputEventArgs e = new EventManager.SearchInputEventArgs
        {
            StartPosition = this.StartPosition,
            GoalPosition = this.GoalPosition,
            Obstacles = this.Obstacles,
            HeurisricMode = this.HeuristicMode,
            HeuristicDropdownValue = this.HeuristicDropdownValue,
            GridCellSize = this.GridCellSize
        };
        EventManager.InvokeSearchInputEvent(name,e);
    }


    private void DrawPoints(Vector3 pos, float size)
    {
        //Vector3 scale = new Vector3(0.9f, 0.01f, 0.9f);
        Vector3 scale = new Vector3(size, 0.01f, size);
        GameObject pointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointObj.transform.position = pos;
        pointObj.transform.localScale = scale;
        pointObj.GetComponent<MeshRenderer>().material.color = Color.gray;
    }

}
