using UnityEngine;
using System.Collections;
using System;

public class WorldController : MonoBehaviour {

    static WorldController _instance;
    public static WorldController Instance {
        get
        {
            return _instance;
        }
    }

    World world;

    public World World
    {
        get
        {
            return world;
        }
    }

    public Sprite sprite_pavimento;

	void Start () {
        if(_instance != null)
        {
            Debug.LogError("NON CI DOVREBBERO ESSERE DUE WORLDCONTROLLER");
        }

        _instance = this;

        world = new World(100, 100);

        for(int x = 0; x < world.Width; x++)
        {
            for(int y = 0; y< world.Height; y++)
            {
                Tile tile_data = world.getTile(x, y);
                GameObject go = new GameObject();
                go.name = "Tile_" + x + "_" + y;
                go.transform.position = new Vector3(tile_data.X, tile_data.Y);

                SpriteRenderer spr = go.AddComponent<SpriteRenderer>();

                tile_data.RegisterTileTypeChange( (tile) => { OnTileTypeChanged(tile,go); } );
            }
        }

        world.RandomizeTiles(50);
	}

    void OnTileTypeChanged(Tile tile, GameObject go)
    {
        if(tile.Type == Tile.TileType.Vuoto)
        {
            go.GetComponent<SpriteRenderer>().sprite = null;
        }
        if (tile.Type == Tile.TileType.Pavimento)
        {
            go.GetComponent<SpriteRenderer>().sprite = sprite_pavimento;
        }
    }
	
	void Update () {
	
	}
}
