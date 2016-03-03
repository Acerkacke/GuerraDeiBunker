using UnityEngine;
using System.Collections;

public class InstalledObject {

    private Tile tile;
    public Tile Tile { get { return tile; } }
    private string objectType;
    private int width;
    private int height;

    protected InstalledObject()
    {

    }

    public static InstalledObject CreateObject(string objectType, int width = 1, int height = 1)
    {
        InstalledObject obj = new InstalledObject();
        obj.objectType = objectType;
        obj.width = width;
        obj.height = height;
        return obj;
    }

    public static InstalledObject PlaceInstance(InstalledObject proto, Tile tile)
    {
        InstalledObject obj = new InstalledObject();
        obj.objectType = proto.objectType;
        obj.width = proto.width;
        obj.height = proto.height;
        obj.tile = tile;

        tile.InstalledObject = obj;
        return obj;
    }
}
