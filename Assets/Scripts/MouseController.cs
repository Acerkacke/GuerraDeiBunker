using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class MouseController : MonoBehaviour
{
    public GameObject circleCursorPrefab;

    public float fastSpeedMultiplier = 2;
    public float keyScrollSpeed = 2;

    public int zoomSpeed = 1;
    public int zoomMax = 25;
    public int zoomMin = 50;

    private Vector3 lastFramePosition;
    Vector3 currFramePosition;
    
    Tile.TileType buildModeTile = Tile.TileType.Pavimento;

    bool buildModeIsObject = false;
    string buildModeObjectType;

    Vector3 dragStartPosition;
    List<GameObject> dragPreviewGameObjects;

    void Start()
    {
        dragPreviewGameObjects = new List<GameObject>();
    }

    void Update()
    {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        UpdateKeyboardScroll();
        //CheckZoom();
        UpdateCameraMovement();
        UpdateDragging();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    private void UpdateKeyboardScroll()
    {
        float translationX = Input.GetAxis("Horizontal");
        float translationY = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            Camera.main.transform.Translate(translationX * fastSpeedMultiplier * keyScrollSpeed, translationY * fastSpeedMultiplier * keyScrollSpeed, 0);
        else
            Camera.main.transform.Translate(translationX * keyScrollSpeed, translationY * keyScrollSpeed, 0);
    }

    void UpdateCameraMovement()
    {
        // Handle screen panning
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {   // Right or Middle Mouse Button

            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);

        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomMax, zoomMin);
    }

    void UpdateDragging()
    {
        // If we're over a UI element, then bail out from this.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Start Drag
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currFramePosition;
        }

        int start_x = Mathf.FloorToInt(dragStartPosition.x);
        int end_x = Mathf.FloorToInt(currFramePosition.x);
        int start_y = Mathf.FloorToInt(dragStartPosition.y);
        int end_y = Mathf.FloorToInt(currFramePosition.y);

        // We may be dragging in the "wrong" direction, so flip things if needed.
        if (end_x < start_x)
        {
            int tmp = end_x;
            end_x = start_x;
            start_x = tmp;
        }
        if (end_y < start_y)
        {
            int tmp = end_y;
            end_y = start_y;
            start_y = tmp;
        }

        // Clean up old drag previews
        while (dragPreviewGameObjects.Count > 0)
        {
            GameObject go = dragPreviewGameObjects[0];
            dragPreviewGameObjects.RemoveAt(0);
            SimplePool.Despawn(go);
        }

        if (Input.GetMouseButton(0))
        {
            // Display a preview of the drag area
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.getTile(x, y);
                    if (t != null)
                    {
                        // Display the building hint on top of this tile position
                        GameObject go = SimplePool.Spawn(circleCursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(this.transform, true);
                        dragPreviewGameObjects.Add(go);
                    }
                }
            }
        }

        // End Drag
        if (Input.GetMouseButtonUp(0))
        {

            // Loop through all the tiles
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.getTile(x, y);
                    if (t != null)
                    {
                        if (!buildModeIsObject)
                        {
                            t.Type = buildModeTile;
                        }
                        else
                        {
                            WorldController.Instance.World.PlaceInstalledObject(buildModeObjectType, t);
                        }
                    }
                }
            }
        }
    }

    public Tile getTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);
        return WorldController.Instance.World.getTile(x, y); 
    }

    public void SetMode_BuildFloor()
    {
        buildModeIsObject = false;
        buildModeTile = Tile.TileType.Pavimento;
    }

    public void SetMode_Bulldoze()
    {
        buildModeIsObject = false;
        buildModeTile = Tile.TileType.Vuoto;
    }

    public void SetMode_BuildObject(string objName)
    {
        buildModeObjectType = objName;
        buildModeIsObject = true;
    }
}