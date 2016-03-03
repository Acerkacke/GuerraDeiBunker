using UnityEngine;
using System.Collections.Generic;

public class World {

    

    Tile[,] tiles;
    Dictionary<string, InstalledObject> installed_object_prototypes = new Dictionary<string, InstalledObject>();

    int width;
    int height;

    public int Width
    {
        get
        {
            return width;
        }
    }
    
    public int Height
    {
        get
        {
            return height;
        }
    }

    public World(int width,int height)
    {
        tiles = new Tile[width, height];
        this.width = width;
        this.height = height;
        for(int i = 0; i < width; i++)
        {
            for(int e = 0;e < height; e++)
            {
                tiles[i, e] = new Tile(this,i, e);
            }
        }
        PopulatePrototypes();
    }

    public void PopulatePrototypes()
    {
        installed_object_prototypes.Add("Wall",InstalledObject.CreateObject("Wall"));
    }

    public Tile getTile(int x,int y)
    {
        if ((x >= 0 && x < width) && (y >= 0 && y < width))
        {
            return tiles[x, y];
        }
        else
        {
            return null;
        }
    }

    public void RandomizeTiles(int percentage)
    {
        for (int i = 0; i < width; i++)
        {
            for (int e = 0; e < height; e++)
            {
                if(Random.Range(0,100) >= percentage)
                {
                    tiles[i, e].Type = Tile.TileType.Vuoto;
                }
                else
                {
                    tiles[i, e].Type = Tile.TileType.Pavimento;
                }
            }
        }
    }

    public void PlaceInstalledObject(string objType,Tile tile)
    {
        if (!installed_object_prototypes.ContainsKey(objType))
        {
            Debug.Log("installed_object_prototypes non contiene l'oggetto " + objType);
            return;
        }

        InstalledObject.PlaceInstance(installed_object_prototypes[objType],tile);

    }

}
