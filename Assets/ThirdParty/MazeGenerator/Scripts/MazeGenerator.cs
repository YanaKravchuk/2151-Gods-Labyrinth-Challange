using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGenerator
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private TileBase _groundTile;
        [SerializeField] private Tilemap _wallTilemap;
        [SerializeField] private Tilemap _groundTilemap;
        [SerializeField] private Tilemap _mapTilemap;

        private int[,] maze;

        void Start()
        {
            GenerateMaze();
            DrawMaze();
        }

        void GenerateMaze()
        {
            maze = new int[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    maze[x, y] = 1;
                }
            }

            GenerateWithDFS(1, 1);
        }

        void GenerateWithDFS(int x, int y)
        {
            // Mark the current cell as a floor
            maze[x, y] = 0;

            // Define possible directions to move (up, down, left, right)
            List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int(0, 1),   // Up
        new Vector2Int(1, 0),   // Right
        new Vector2Int(0, -1),  // Down
        new Vector2Int(-1, 0)   // Left
    };

            // Shuffle directions to create random paths
            directions = directions.OrderBy(a => Random.value).ToList();

            // Explore in all directions
            foreach (var dir in directions)
            {
                int nx = x + dir.x * 2;
                int ny = y + dir.y * 2;

                if (nx > 0 && nx < _width - 1 && ny > 0 && ny < _height - 1 && maze[nx, ny] == 1)
                {
                    // Carve the path between (x, y) and (nx, ny)
                    maze[x + dir.x, y + dir.y] = 0;
                    GenerateWithDFS(nx, ny);
                }
            }
        }

        void DrawMaze()
        {
            _groundTilemap.ClearAllTiles();
            _wallTilemap.ClearAllTiles();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (maze[x, y] == 1)
                    {
                        _wallTilemap.SetTile(new Vector3Int(x, y, 0), _wallTile);
                        _mapTilemap.SetTile(new Vector3Int(x, y, 0), _wallTile);
                    }
                    else
                    {
                        _groundTilemap.SetTile(new Vector3Int(x, y, 0), _groundTile);
                        _mapTilemap.SetTile(new Vector3Int(x, y, 0), _groundTile);
                    }
                }
            }
        }
    }

}