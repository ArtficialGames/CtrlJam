using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Light Tile", menuName = "Tiles/Light Tile")]
public class LightTile : Tile
{
    public Sprite[] lightSprite;

    private int lightLevel = 0;
    private Sprite newSprite;

    public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
    {
        base.GetTileData(location, tileMap, ref tileData);

        newSprite = lightSprite[lightSprite.Length - 1];

        if (PixelLighting.Instance != null)
        {
            lightLevel = PixelLighting.Instance.GetTileLightLevel(location);
            if (lightLevel < lightSprite.Length)
                newSprite = lightSprite[lightLevel];
        }

        if(Application.isPlaying)
            tileData.sprite = newSprite;
    }
}
