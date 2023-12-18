using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PixelLighting : MonoBehaviour
{
    public static PixelLighting Instance;
    public List<Tilemap> tilemaps;

    private void Awake()
    {
        Instance = this;

        foreach (var tilemap in tilemaps)
        {
            tilemap.RefreshAllTiles();
        }

    }

    public int GetTileLightLevel(Vector3Int location, LightSource light)
    {
        int lightLevel = 0;

        if (Vector3.Distance(location, light.transform.position) < light.range)
        {
            lightLevel = Mathf.FloorToInt(light.range) - Mathf.FloorToInt(Vector3.Distance(location, light.transform.position));
        }

        return lightLevel;
    }
}
