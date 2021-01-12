using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tile[] tile;
    public Tile blackTile;
    public Tile grayTile;
    public Tile whiteTile;
    public Tilemap map;
    private int rows = 256;
    private int cols = 256;
    private List<List<Vector3Int>> circlePositions = new List<List<Vector3Int>>();
    List<Vector3Int> circle;
    // Start is called before the first frame update
    void Start()
    {
        //creates initial 256x256 grid
        for(int x = 0; x < rows; x++)
        {
            for(int y = 0; y< cols; y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                map.SetTile(p, tile[Random.Range(0,tile.Length)]);
                
            }
        }
    }
    //creates new list on clicck
    private void OnMouseDown()
    {
        circle = new List<Vector3Int>();
    }
    //draws circle and stores points
    private void OnMouseDrag()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mousePosition = new Vector3Int((int)temp.x, (int)temp.y, 0);
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        if (map.HasTile(gridPosition))
        {
            map.SetTile(gridPosition, blackTile);
            circle.Add(gridPosition);
            
        }
    }

    private void OnMouseUp()
    {

        circlePositions.Add(circle);
        
    }

    List<Vector3Int> visited;
    private void fillCircle(Tile tile, Vector3Int point)
    {
        //checks if within boundaries and if point is visited.
        if ( !visited.Contains(new Vector3Int(point.x, point.y, 0)) && point.x >= 0 && point.x < cols && point.y >= 0 && point.y < rows)
        {
            
            if(map.GetTile(new Vector3Int(point.x, point.y, 0)).name != "boxblack")
            {
                visited.Add(point);
                map.SetTile(point, tile);
                if (!visited.Contains(new Vector3Int(point.x - 1, point.y, 0)))
                {
                    fillCircle(tile, new Vector3Int(point.x - 1, point.y, 0));
                }
                if (!visited.Contains(new Vector3Int(point.x, point.y + 1, 0)))
                {
                    fillCircle(tile, new Vector3Int(point.x, point.y + 1, 0));
                }
                if (!visited.Contains(new Vector3Int(point.x + 1, point.y, 0)))
                {
                    fillCircle(tile, new Vector3Int(point.x + 1, point.y, 0));
                }
                if (!visited.Contains(new Vector3Int(point.x, point.y - 1, 0)))
                {
                    fillCircle(tile, new Vector3Int(point.x, point.y - 1, 0));
                }
            }
            
        }
        
    }


    private void OnPressedEnter()
    {
        //iterates through each circle coordinates, finds a middle point inside the circle and fills it using an algorithm.

        foreach (List<Vector3Int> circle in circlePositions)
        {
            int top = 0, bottom = 0, left = 0, right = 0;

            foreach (Vector3Int point in circle)
            {
                if (top == 0 && bottom == 0 && left == 0 && right == 0)
                {
                    top = point.y;
                    bottom = point.y;
                    left = point.x;
                    right = point.x;
                }
                if (point.y > top)
                {
                    top = point.y;

                }
                if (point.y < bottom)
                {
                    bottom = point.y;

                }
                if (point.x > right)
                {
                    right = point.x;

                }
                if (point.x < left)
                {
                    left = point.x;

                }
            }

            Vector3Int avgPoint = new Vector3Int((right + left) / 2, (top + bottom) / 2, 0);

            visited = new List<Vector3Int>();
            fillCircle(whiteTile, avgPoint);
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (map.GetTile(new Vector3Int(x, y, 0)).name != "boxwhite" && map.GetTile(new Vector3Int(x, y, 0)).name != "boxblack")
                {
                    map.SetTile(new Vector3Int(x, y, 0), grayTile);
                }
                
            }
        }
    }  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnPressedEnter();
        }
    }
        
}
