using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private Transform _cam;
    [SerializeField] private int[] _wallLengths;
    private Dictionary<Vector2, Tile> _tiles;
    private List<Wall>_walls = new List<Wall>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
                spawnedTile.width = x;
                spawnedTile.height = y;

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }


        //Generate Random Walls
        for (int i = 0; i < _wallLengths.Length; i++)
        {
            int length = _wallLengths[i];
            bool vertical = Random.Range(0, 2)  == 0;
            if (vertical)
            {
                int start = Random.Range(1, _height-length);
                int otherAxis = Random.Range(1, _width);
                Wall wall = new Wall(vertical, start, start + length, otherAxis);
                for (int j = 0; j  < length; j++)
                {
                    Instantiate(_wallPrefab, new Vector3(otherAxis - .5f, start + j, -1), Quaternion.Euler(0, 0, 90));
                }
                _walls.Add(wall);
            }
            else 
            {
                int start = Random.Range(1, _width-length);
                int otherAxis = Random.Range(1, _height);
                var wall = new Wall(vertical, start, start + length, otherAxis);
                for (int j = 0; j  < length; j++)
                {
                    Instantiate(_wallPrefab, new Vector3(start + j, otherAxis - .5f, -1), Quaternion.identity);
                }
                _walls.Add(wall);
            }
        }

        _cam.transform.position = new Vector3((float) _width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Player1Move);

    }

    public void ClearGrid() // Clear InRange() out
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile current = _tiles[new Vector2(x, y)];
                current.OutOfRange();
            }
        }
    }

    public int GetWidth()
    {
        int width = _width;
        return width;
    }
    public int GetHeight()
    {
        int height = _height;
        return height;
    }

    public Tile GetPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }

    public Wall[] GetWalls()
    {
        return _walls.ToArray();
    }

    public bool IsReachable(int x1, int y1, int x2, int y2)
    {
        Wall[] walls = this.GetWalls();
        //You can always reach yourself
        if (x1 == x2 && y1 == y2)
        {
            return true;
        }

        //Function only works for touching squares
        if (Math.Abs(x1-x2) > 1 || Math.Abs(y1-y2) > 1)
        {
            Debug.LogError("IsReachable trying to use on tiles not touching");
        }

        for (int i = 0; i < walls.Length; i++)
        {
            bool vInnerCheck = walls[i].Contains(Math.Max(x1, x2), y1, Math.Max(x1, x2), y1 + 1);
            bool hInnerCheck = walls[i].Contains(x1, Math.Max(y1, y2), x1 + 1, Math.Max(y1, y2));
            bool vOuterCheck = walls[i].Contains(Math.Max(x1, x2), y2, Math.Max(x1, x2), y2 + 1);
            bool hOuterCheck = walls[i].Contains(x2, Math.Max(y1, y2), x2 + 1, Math.Max(y1, y2));

            //horizontal wall case
            if (x1 == x2)
            {
                if (hInnerCheck)
                {
                    return false;
                }
            }
            //vertical wall case
            else if (y1 == y2)
            {
                if (vInnerCheck)
                {
                    return false;
                }
            }
            //diagonal case
            else if ((vInnerCheck && vOuterCheck) || (hOuterCheck && hInnerCheck) || (vInnerCheck && hInnerCheck) || (vOuterCheck && hOuterCheck))
            {
                return false;
            }
        }
        
        return true;
    }
}

public struct Wall {
    public int start;
    public int end;
    public int otherAxis;
    public bool vertical;
    public List<GameObject> wallObjects;

    public Wall(bool vertical, int start, int end, int otherAxis)
    {
        this.vertical = vertical;
        this.start = start;
        this.end = end;
        this.otherAxis = otherAxis;
        this.wallObjects = new List<GameObject>();
    }

    public bool Contains(int x1, int y1, int x2, int y2)
    {
        bool xCheck = false;
        bool yCheck = false;
        if (this.vertical)
        {
            xCheck = x1 == x2 && x1 == this.otherAxis;
            yCheck = y1 >= this.start && y1 < this.end && y2 >= this.start && y2 <= this.end;
        }
        else
        {
            yCheck = y1 == y2 && y1 == this.otherAxis;
            xCheck = x1 >= this.start && x1 < this.end && x2 >= this.start && x2 <= this.end;
        }

        return yCheck && xCheck;
    }
}