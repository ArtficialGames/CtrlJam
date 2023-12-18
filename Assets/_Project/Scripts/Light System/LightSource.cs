using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightSource : MonoBehaviour
{
    [SerializeField] public float range;

    private void Start()
    {
        InvokeRepeating("UpdateLighting", 0f, 0.15f);
    }

    /*private void Update()
    {
        UpdateLighting();
    }*/

    protected void UpdateLighting()
    {
        foreach (var tilemap in PixelLighting.Instance.tilemaps)
        {
            BFS((Vector2Int)tilemap.WorldToCell(transform.position), Mathf.CeilToInt(range), tilemap);
        }
    }


    Dictionary<Vector2Int, int> distanceChart = new Dictionary<Vector2Int, int>();
    Dictionary<Vector2Int, Vector2Int> pathChart = new Dictionary<Vector2Int, Vector2Int>();

    public void BFS(Vector2Int startPos, int maxDistance, Tilemap tilemap)
    {
        Vector2Int currentPos = startPos;
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();

        distanceChart.Clear();
        pathChart.Clear();

        frontier.Enqueue(currentPos);
        distanceChart.Add(currentPos, 0);
        pathChart.Add(currentPos, new Vector2Int(-1, -1));

        while (frontier.Count > 0)
        {
            currentPos = frontier.Dequeue();
            if (distanceChart[currentPos] < maxDistance + 3)
            {
                foreach (Vector2Int nextPos in GetNeighbors(currentPos))
                {
                    if (distanceChart.ContainsKey(nextPos) == false)
                    {
                        frontier.Enqueue(nextPos);
                        distanceChart.Add(nextPos, 1 + distanceChart[currentPos]);
                        pathChart.Add(nextPos, currentPos);

                        //tilemap.SetTile((Vector3Int)nextPos, testTile);

                        LightTile tile = tilemap.GetTile((Vector3Int)nextPos) as LightTile;

                        if (tile != null && distanceChart[currentPos] >= maxDistance)
                            tile.lightLevel = 0;
                        else if(tile != null && PixelLighting.Instance != null)
                        {
                            tile.lightLevel = PixelLighting.Instance.GetTileLightLevel((Vector3Int)nextPos, this);
                        }

                        tilemap.RefreshTile((Vector3Int)nextPos);
                    }
                }
            }
        }
    }

    List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        neighbors.Add(pos + Vector2Int.up);
        neighbors.Add(pos + Vector2Int.down);
        neighbors.Add(pos + Vector2Int.right);
        neighbors.Add(pos + Vector2Int.left);

        return neighbors;
    }
}
