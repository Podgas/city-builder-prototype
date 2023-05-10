using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField]
    Camera _camera;

    [SerializeField]
    private Transform testTransform;
    [SerializeField]
    private GameObject _buildDebugPrefab;
    [SerializeField]
    private GameObject _noBuildDebugPrefab;

    [SerializeField]
    private Grid<GridObject> grid;
    private WorldManager worldManager;
    public Vector3 MousePosition;


    private void Start()
    {
        
        worldManager = WorldManager.Instance;
        grid = new Grid<GridObject>(worldManager._width, worldManager._height, worldManager._depth, worldManager._cellSize, Vector3.zero, (Grid<GridObject> g, int x, int y, int z) => new GridObject(g,x,y,z));
        for(int x = 0; x < worldManager.GetGrid().Width; x++)
        {
            for(int y = 0; y < worldManager.GetGrid().Height; y++)
            {
                for(int z = 0;  z < worldManager.GetGrid().Depth; z++)
                {
                    if(worldManager.GetGrid().GetGridObject(x, y, z).GetTerrainType().name != "Water")
                    {
                        grid.GetGridObject(x,y,z).SetIsLand(true);
                    }

                }
            }
        }

        //CreateDebug();
    }

    private void Update()
    {
        MousePosition = GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXYZ(GetMouseWorldPosition(), out int x, out int y, out int z);
            GridObject gridObject = grid.GetGridObject(x, y-1, z);

            if (gridObject.CanBuild() && gridObject.IsLand())
            {
                Transform buildTransform = Instantiate(testTransform, grid.GetCellCenter(x, y, z), Quaternion.identity);
                gridObject.SetTransform(buildTransform);
            }
            else
            {
                Debug.Log("Cant Build");
            }

            
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit raycasthit, 999f))
        {
            return raycasthit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    [Serializable]
    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int y;
        private int z;
        private Transform _transform;
        private bool isLand = false;

        public GridObject(Grid<GridObject> grid, int x, int y, int z)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            this.z = z;

        }
        public void SetIsLand(bool value)
        {
            isLand = value;
        }
        public bool IsLand()
        {
            return isLand;
        }
        public void SetTransform(Transform transform)
        {
            _transform = transform;
        }
        public void ClearTransform()
        {
            _transform = null; ;
        }
        public Transform GetTransform()
        {
            return _transform;
        }
        public bool CanBuild()
        {
            return _transform == null;
        }

    }

    public void CreateDebug()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int z = 0; z < grid.Depth; z++)
                {
                    if (grid.GetGridObject(x, y, z).CanBuild())
                    {
                        GameObject.Instantiate(_buildDebugPrefab, grid.GetCellCenter(x, y, z) + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    }
                    else
                    {
                        GameObject.Instantiate(_noBuildDebugPrefab, grid.GetCellCenter(x, y, z) + new Vector3(0,0.5f,0), Quaternion.identity);
                    }
                    
                }
            }
        }
    }


    
}
