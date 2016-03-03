using UnityEngine;
using System;
using System.Collections;

public class Tile {
    public enum TileType { Vuoto, Pavimento};

    private int x;
    private int y;
    private World world;
    private TileType type;
    private Action<Tile> cbTileTypeChange;
    private InstalledObject installedObject;

    public InstalledObject InstalledObject
    {
        get
        {
            return installedObject;
        }
        set
        {
            if (value == null)
            {
                installedObject = null;
                return;
            }

            if (installedObject != null)
            {
                installedObject = value;
            }
        }
    }

    public TileType Type
    {
        get
        {
            return type;
        }
        set
        {
            if (type != value)
            {
                this.type = value;
                if (cbTileTypeChange != null)
                {
                    cbTileTypeChange(this);
                }
            }
        }
    }

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
    }

    public Tile(World world,int x, int y)
    {
        this.x = x;
        this.y = y;
        this.world = world;
    }

    public void RegisterTileTypeChange(Action<Tile> callback)
    {
        cbTileTypeChange += callback;
    }

    public void UnRegisterTileTypeChange(Action<Tile> callback)
    {
        cbTileTypeChange -= callback;
    }
}
