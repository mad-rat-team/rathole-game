using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestRenderScript : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform player;
    public Color tileColor;

    private void Update()
    {
        Vector3Int playerCell = tilemap.WorldToCell(player.position);
        BoundsInt bounds = new BoundsInt(playerCell.x-5, playerCell.y-5, 1, 5, 5, 10);
        BoundsInt prevBounds = new BoundsInt(playerCell.x-6, playerCell.y-6, 1, 7, 7, 10);

        foreach (Vector3Int position in prevBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            if (tile != null)
            {
                tilemap.SetTile(position, tile); // This refreshes the tile to apply the new material
                tilemap.SetTileFlags(position, TileFlags.None);
                tilemap.SetColor(position, Color.white);
            }
        }
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            if (tile != null)
            {
                tilemap.SetColor(position, tileColor);
            }
        }
    }
}
