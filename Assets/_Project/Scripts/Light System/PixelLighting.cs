using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PixelLighting : MonoBehaviour
{
    public static PixelLighting Instance;
    public List<LightSource> lightSources = new List<LightSource>();
    public List<Tilemap> tilemaps;

    private void Awake()
    {
        Instance = this;
        //InvokeRepeating("UpdateLightSources", 0f, 0.15f);
        UpdateLightSources();
    }

    void UpdateLightSources()
    {
        lightSources = new List<LightSource>();

        foreach (var item in GameObject.FindGameObjectsWithTag("LightSource"))
        {
            lightSources.Add(item.GetComponent<LightSource>());
        }
    }

    public int GetTileLightLevel(Vector3Int location)
    {
        int lightLevel = 0;

        foreach (var light in lightSources)
        {
            if (Vector3.Distance(location, light.transform.position) < light.range)
            {
                lightLevel = Mathf.CeilToInt(light.range) - Mathf.CeilToInt(Vector3.Distance(location, light.transform.position));
            }
        }

        return lightLevel;
    }
}
