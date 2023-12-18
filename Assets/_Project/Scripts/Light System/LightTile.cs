using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Light Tile", menuName = "Tiles/Light Tile")]
public class LightTile : Tile
{
    public Sprite[] lightSprite;

    public int lightLevel = 0;
    private Sprite newSprite;

    public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
    {
        base.GetTileData(location, tileMap, ref tileData);

        newSprite = lightSprite[lightSprite.Length - 1];

        if(Application.isPlaying)
            tileData.sprite = newSprite;
    }
}
