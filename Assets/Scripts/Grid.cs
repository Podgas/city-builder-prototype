using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using System;


public class Grid<TGridObject>
{

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
        public int z;
    }

    private int _width;
    private int _height;
    private int _depth;
    public int Width { get { return _width; } }
    public int Height { get { return _height; } }
    public int Depth { get { return _depth; } }


    private float _cellSize;
    private Vector3 _originPosition;
    private TGridObject[,,] _gridArray;
    private TextMeshPro[,,] debugTextArray;

    public Grid(int width, int height, int depth, float cellSize, Vector3 originPosition, System.Func<Grid<TGridObject>, int, int, int, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _depth = depth;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, height, depth];
        debugTextArray = new TextMeshPro[width, height, depth];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < _gridArray.GetLength(2); z++)
                {
                    _gridArray[x, y, z] = createGridObject(this, x, y, z);
                }
            }
        }
    }

    public float GetCellSize()
    {
        return _cellSize;
    }

    public Vector3 GetGridWorldCenter()
    {
        Vector3 center = GetWorldPosition(Width, Height, Depth) / 2f;
        return center;
    }

    public TGridObject GetGridObject(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < _width && y < _height && z < _depth)
        {
            return _gridArray[x, y, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public void SetGridObject(int x, int y, int z, TGridObject value)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < _width && y < _height && z < _depth)
        {
            _gridArray[x, y, z] = value;
            debugTextArray[x, y, z].SetText(value.ToString());
        }
    }
    public void TriggerGridObjectChanged(int x, int y, int z)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        SetGridObject(x, y, z, value);
    }

    private void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition.x - _originPosition.x) / _cellSize);
        y = Mathf.FloorToInt((worldPosition.y - _originPosition.y) / _cellSize);
        z = Mathf.FloorToInt((worldPosition.z - _originPosition.z) / _cellSize);
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * _cellSize + _originPosition;
    }
    public void DebugDrawGrid()
    {
        // Draw grid lines in XY plane
        for (int x = 0; x <= _width; x++)
        {
            for (int y = 0; y <= _height; y++)
            {
                Vector3 start = GetWorldPosition(x, y, 0);
                Vector3 end = GetWorldPosition(x, y, _depth);
                Debug.DrawLine(start, end, Color.black, 100f);
            }
        }

        // Draw grid lines in XZ plane
        for (int x = 0; x <= _width; x++)
        {
            for (int z = 0; z <= _depth; z++)
            {
                Vector3 start = GetWorldPosition(x, 0, z);
                Vector3 end = GetWorldPosition(x, _height, z);
                Debug.DrawLine(start, end, Color.black, 100f);
            }
        }

        // Draw grid lines in YZ plane
        for (int y = 0; y <= _height; y++)
        {
            for (int z = 0; z <= _depth; z++)
            {
                Vector3 start = GetWorldPosition(0, y, z);
                Vector3 end = GetWorldPosition(_width, y, z);
                Debug.DrawLine(start, end, Color.black, 100f);
            }
        }

        // Draw cell labels
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    // Create a text mesh in the middle of the cell
                    Vector3 center = GetTopCenterCell(x,y,z);
                    GameObject textObject = new GameObject($"({x}, {y}, {z})", typeof(TextMeshPro));
                    textObject.transform.position = center;
                    TextMeshPro textMesh = textObject.GetComponent<TextMeshPro>();
                    textMesh.text = _gridArray[x, y, z].ToString();
                    textMesh.fontSize = 1;
                    textMesh.alignment = TextAlignmentOptions.Center;
                    textMesh.sortingLayerID = 1;
                    debugTextArray[x, y, z] = textMesh;
                }
            }
        }
        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
        {
            debugTextArray[eventArgs.x, eventArgs.y, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.y, eventArgs.z].ToString();
        };
    }
    public Vector3 GetCellCenter(int x, int y, int z)
    {
        return GetWorldPosition(x, y, z) + new Vector3(_cellSize, _cellSize, _cellSize) * 0.5f;
    }
    public Vector3 GetTopCenterCell(int x, int y, int z)
    {
        return GetWorldPosition(x, y, z) + new Vector3(_cellSize * 0.5f, _cellSize * 1.15f, _cellSize * 0.5f);
    }

}