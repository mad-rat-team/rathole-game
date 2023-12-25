using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/Experimental/BrickWallTile")]
public class BrickWallTile : TileBase
{
    [SerializeField] private Sprite[] brickLeftFrontSprites;
    [SerializeField] private Sprite[] brickLeftBackSprites;
    [SerializeField] private Sprite[] brickRightFrontSprites;
    [SerializeField] private Sprite[] brickRightBackSprites;
    [SerializeField] private Sprite[] brickHalfLeftSprites;
    [SerializeField] private Sprite[] brickHalfRightSprites;
    [SerializeField] private int seed = 0;

    private enum BrickType
    {
        Null,
        //Unknown,
        LeftFront,
        LeftBack,
        RightFront,
        RightBack,
        HalfLeft,
        HalfRight
    }

    //private BrickType[,,] bricks;

    private Dictionary<int, BrickType[,,]> bricks = new();

    private class TilePosComparer : IComparer<Vector3Int>
    {
        public int Compare(Vector3Int a, Vector3Int b)
        {
            if (a.x > b.x) return 1;
            if (a.x < b.x) return -1;
            //if a.x == b.x
            if (a.y > b.y) return 1;
            if (a.y < b.y) return -1;
            //if a == b
            return 0;
        }
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        RefreshBricks(tilemap);

        foreach (Vector3Int tilePos in tilemap.cellBounds.allPositionsWithin) // We already find all non-null tiles, so we can only refresh those
        {
            tilemap.RefreshTile(tilePos);
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        BrickType[,,] brickTypes;
        bool brickTypesExists = bricks.TryGetValue(tilemap.GetComponent<Tilemap>().GetInstanceID(), out brickTypes);


        if (
            !brickTypesExists
            || position.x - tilemap.origin.x > brickTypes.GetUpperBound(0)
            || position.y - tilemap.origin.y > brickTypes.GetUpperBound(1)
            || position.z - tilemap.origin.z > brickTypes.GetUpperBound(2)
            || position.x - tilemap.origin.x < 0
            || position.y - tilemap.origin.y < 0
            || position.z - tilemap.origin.z < 0
            )
        {
            //Debug.Log("bricks is null");
            tileData.sprite = GetRandomSprite(brickHalfRightSprites, position);
            return;
        };

        tileData.sprite = GetRandomSprite(
            brickTypes[position.x - tilemap.origin.x, position.y - tilemap.origin.y, position.z - tilemap.origin.z] switch
            {
                BrickType.LeftFront => brickLeftFrontSprites,
                BrickType.LeftBack => brickLeftBackSprites,
                BrickType.RightFront => brickRightFrontSprites,
                BrickType.RightBack => brickRightBackSprites,
                BrickType.HalfLeft => brickHalfLeftSprites,
                BrickType.HalfRight => brickHalfRightSprites,
                _ => null,
            },
            position);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        bool res = base.StartUp(position, tilemap, go);
        RefreshBricks(tilemap);

        //Debug.Log(string.Join(",", bricks.Keys));

        return res;
    }

    private void RefreshBricks(ITilemap tilemap)
    {
        //bricks = new BrickType[tilemap.size.x, tilemap.size.y, tilemap.size.z];
        int tilemapInstanceID = tilemap.GetComponent<Tilemap>().GetInstanceID();

        // NOTE: You can do this only if there is no key tilemapInstanceID or if the bounds of tilemap have changed but it does not matter too much probably
        bricks[tilemapInstanceID] = new BrickType[tilemap.size.x, tilemap.size.y, tilemap.size.z]; // Filled by BrickType.Null's

        SortedSet<Vector3Int> unknownBricks = new SortedSet<Vector3Int>(new TilePosComparer());

        //TODO: Make it work for other Z Positions

        foreach (Vector3Int tilePos in tilemap.cellBounds.allPositionsWithin)
        {
            //if(tilePos.y > 0)
            //    Debug.Log(tilePos);
            if (tilemap.GetTile(tilePos) == null) continue; //TODO: Check if tile is a brick not if it exists
            

            unknownBricks.Add(tilePos);
            //bricks[tilePos.x, tilePos.y, tilePos.z] = BrickType.Unknown;

            //Vector3 worldPos = tilemap.GetComponent<Tilemap>().CellToWorld(tilePos);
            //Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.5f, Color.red, 1);
        }

        //Debug.Log(unknownBricks.Count);

        while (unknownBricks.Count > 0)
        {
            Vector3Int startPos = unknownBricks.Max;

            BrickType brickType;
            Vector3Int offsetPerStep = Vector3Int.zero;

            if (unknownBricks.Contains(startPos + new Vector3Int(0, -1, 0)))
            {
                brickType = BrickType.RightBack;
                offsetPerStep = new Vector3Int(0, -1, 0);
            }
            else if (unknownBricks.Contains(startPos + new Vector3Int(-1, 0, 0)))
            {
                brickType = BrickType.LeftBack;
                offsetPerStep = new Vector3Int(-1, 0, 0);
            }
            else
            {
                //bricks[tilemapInstanceID][startPos.x - tilemap.origin.x, startPos.y - tilemap.origin.y, startPos.z - tilemap.origin.z] = BrickType.HalfLeft;
                if (startPos.x - tilemap.origin.x + 1 <= bricks[tilemapInstanceID].GetUpperBound(0) && bricks[tilemapInstanceID][startPos.x - tilemap.origin.x + 1, startPos.y - tilemap.origin.y, startPos.z - tilemap.origin.z] != BrickType.Null)
                {
                    brickType = BrickType.HalfLeft;
                }
                else
                {
                    brickType = BrickType.HalfRight;
                }

                bricks[tilemapInstanceID][startPos.x - tilemap.origin.x, startPos.y - tilemap.origin.y, startPos.z - tilemap.origin.z] = brickType;
                unknownBricks.Remove(startPos);
                continue;
            }

            for (Vector3Int pos = startPos; unknownBricks.Contains(pos); pos += offsetPerStep)
            {
                if ((brickType == BrickType.LeftBack || brickType == BrickType.RightBack) && !unknownBricks.Contains(pos + offsetPerStep)) break;

                bricks[tilemapInstanceID][pos.x - tilemap.origin.x, pos.y - tilemap.origin.y, pos.z - tilemap.origin.z] = brickType;
                unknownBricks.Remove(pos);

                brickType = brickType switch
                {
                    BrickType.LeftBack => BrickType.LeftFront,
                    BrickType.LeftFront => BrickType.LeftBack,
                    BrickType.RightBack => BrickType.RightFront,
                    BrickType.RightFront => BrickType.RightBack,
                    _ => BrickType.Null
                };
            }
        }

        //Vector3 worldPos = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(tilemap.cellBounds.xMax - 1, tilemap.cellBounds.yMax - 1, 0));
        //Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.5f, Color.red, 1);

        //for (int x = 0; x < tilemap.size.x; x++)
        //{
        //    for (int y = 0; y < tilemap.size.y; y++)
        //    {
        //        Vector3 worldPos = tilemap.GetComponent<Tilemap>().CellToWorld(new Vector3Int(x + tilemap.origin.x, y + tilemap.origin.y, 0));
        //        //Debug.Log(worldPos);
        //        Debug.DrawLine(worldPos, worldPos + Vector3.up * 0.5f, Color.red, 1);
        //    }
        //}


        //for (int dx = -1; dx <= 1; dx++)
        //{
        //    for (int dy = -1; dy <= 1; dy++)
        //    {
        //        tilemap.RefreshTile(position + new Vector3Int(dx, dy));
        //    }
        //}
    }

    private Sprite GetRandomSprite(Sprite[] sprites, Vector3Int tilePosition)
    {
        if (sprites == null) return null;
        if (sprites.Length == 0) {
            Debug.LogWarning($"No sprites set for some tile");
            return null;
        }
        Random.InitState(System.HashCode.Combine(tilePosition, seed));
        return sprites[Random.Range(0, sprites.Length)];
    }
}
