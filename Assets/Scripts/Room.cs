using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public int Width, Height, Xcenter, Ycenter;
    public string ID;
    private GameObject FloorPrefab, WallPrefab;
    private GameObject Walls, Floors;
    private List<GameObject> WallList;
    private List<GameObject> FloorList;
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(string ID, int Width, int Height, int Xcenter, int YCenter, GameObject WallPrefab, GameObject FloorPrefab)
    {
        this.ID = ID;
        this.Width = Width;
        this.Height = Height;
        this.Xcenter = Xcenter;
        this.Ycenter = YCenter;
        this.WallPrefab = WallPrefab;
        this.FloorPrefab = FloorPrefab;

        this.name = ID;

        this.transform.position = new Vector3(Xcenter, YCenter, 0);

        Walls = new GameObject("Walls");
        Walls.transform.parent = this.transform;
        Floors = new GameObject("Floors");
        Floors.transform.parent = this.transform;

        WallList = new List<GameObject>();
        FloorList = new List<GameObject>();

        Construct();
    }

    void Construct()
    {
        int xLBound = Xcenter - Width / 2;
        int xUBound = Xcenter + Width / 2;
        int yLBound = Ycenter - Height / 2;
        int yUBound = Ycenter + Height / 2;
        for (int x = xLBound ; x <= xUBound; x++)
        {
            for (int y = yLBound; y <= yUBound; y++)
            {
                GameObject Tile;
                if (x == xLBound || y == yLBound || x == xUBound || y == yUBound)
                {
                    Tile = Instantiate(WallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    Tile.transform.parent = Walls.transform;
                    WallList.Add(Tile);
                }
                else
                {
                    Tile = Instantiate(FloorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    Tile.transform.parent = Floors.transform;
                    FloorList.Add(Tile);
                }
            }
        }
    }

    public void AddFloorTile(Vector2 Location)
    {
        GameObject Tile = Instantiate(FloorPrefab, Location, Quaternion.identity);
        Tile.transform.parent = Floors.transform;
    }

    public List<GameObject> GetFloors()
    {
        return FloorList;
    }

    public List<GameObject> GetWalls()
    {
        return WallList;
    }
}
