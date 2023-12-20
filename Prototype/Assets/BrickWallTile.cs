using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/BrickWallTile")]
public class BrickWallTile : TileBase
{
    [SerializeField] private Sprite[] brickLeftFrontSprites;
    [SerializeField] private Sprite[] brickLeftBackSprites;
    [SerializeField] private Sprite[] brickRightFrontSprites;
    [SerializeField] private Sprite[] brickRightBackSprites;
    [SerializeField] private Sprite[] brickHalfSprites;
    [SerializeField] private int seed = 0;

    private enum BrickType
    {
        Null,
        //Unknown,
        LeftFront,
        LeftBack,
        RightFront,
        RightBack,
        HalfBrick
    }

    private BrickType[,,] bricks;

    private class TilePosComparer : IComparer<Vector3Int>
    {
        public int Compare(Vector3Int x, Vector3Int y)
        {
            return 0;
        }
    }


    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        //Vector3 worldPos = tilemap.GetComponent<Tilemap>().CellToWorld(position);
        ////Debug.Log(worldPos);
        //Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.5f, Color.red, 1);

        //Debug.Log($"{tilemap.size} {tilemap.cellBounds.size}");

        //foreach(Vector3Int bi in tilemap.cellBounds.allPositionsWithin)
        //{
        //    Vector3 worldPos = tilemap.GetComponent<Tilemap>().CellToWorld(bi);
        //    Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.5f, Color.red, 1);
        //}

        bricks = new BrickType[tilemap.size.x, tilemap.size.y, tilemap.size.z];


        SortedSet<Vector3Int> unknownBricks = new();

        foreach (Vector3Int tilePos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(tilePos) == null) continue;
            //bricks[tilePos.x, tilePos.y, tilePos.z] = BrickType.Unknown;
        }

        Vector3 worldPos = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(tilemap.cellBounds.xMax - 1, tilemap.cellBounds.yMax - 1, 0));
        Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.5f, Color.red, 1);

        //for (int x = 0; x < tilemap.size.x; x++)
        //{
        //    for (int y = 0; y < tilemap.size.y; y++)
        //    {
        //        Vector3 worldPos = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(x + tilemap.origin.x, y + tilemap.origin.y, 0));
        //        //Debug.Log(worldPos);
        //        Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.5f, Color.red, 1);
        //    }
        //}


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

        //Vector3 worldPos = tilemap.GetComponent<Tilemap>().CellToWorld(position);
        //Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.25f, Color.blue, 1);

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
