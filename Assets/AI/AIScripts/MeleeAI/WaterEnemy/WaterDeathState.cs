using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDeathState : MeleeDeathState
{
    public override void VirtualEnterState()
    {
        base.VirtualEnterState();

        int roundX = Mathf.RoundToInt(transform.position.x);
        int roundY = Mathf.RoundToInt(transform.position.y);

        for (int x = roundX - 1; x <= roundX + 1; x++)
        {
            WritePointWithWater(x, roundY);
        }
        for (int y = roundY - 1; y <= roundY + 1; y++)
        {
            if (y != 0) //we already got that tile, right. 
            {
                WritePointWithWater(roundX, y);
            }
        }
    }

    void WritePointWithWater(int x, int y)
    {
        MapNode node = new MapNode(true, TileType.Water);

        DestroyWallsOnPoint(x, y);

        MapManager.WritePoint(x, y, node);
        GetMapDrawer.GetSceneMapDrawer().RewritePoint(x, y, node);
    }

    void DestroyWallsOnPoint(int x, int y)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(x, y), Vector2.zero, 1f);
        foreach (RaycastHit2D hit in hits)
        {
            PlayerWall wall;
            if (hit.transform.TryGetComponent<PlayerWall>(out wall))
            {
                wall.HandleEvent(new Event(EventType.Physical, 10000f));
            }
        }

    }
}
