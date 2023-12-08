using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorRandomizer : MonoBehaviour
{
    [SerializeField] Tile[] tiles;

    private void Awake()
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        foreach (var tile in tilemap.cellBounds.allPositionsWithin)
        {
            tilemap.SetTile(tile, tiles[Random.Range(0, tiles.Length)]);
        }
    }
}
