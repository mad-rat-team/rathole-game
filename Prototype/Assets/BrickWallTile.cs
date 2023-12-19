using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/BrickWallTile")]
public class BrickWallTile : TileBase
{
    [SerializeField] private Sprite[] brickLeftFrontSprites;
    [SerializeField] private Sprite[] brickLeftBackSprites;
    [SerializeField] private Sprite[] brickRightFrontSprites;
    [SerializeField] private Sprite[] brickRightBackSprites;
    [SerializeField] private int seed = 0;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                tilemap.RefreshTile(position + new Vector3Int(dx, dy));
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = GetRandomSprite(brickLeftFrontSprites, position);

        //bool isLeftBrick = true;
    }

    private Sprite GetRandomSprite(Sprite[] sprites, Vector3Int tilePosition)
    {
        if (sprites.Length == 0) {
            Debug.LogWarning($"No sprites set for some tile");
            return null;
        }
        Random.InitState(System.HashCode.Combine(tilePosition, seed));
        return sprites[Random.Range(0, sprites.Length)];
    }
}
