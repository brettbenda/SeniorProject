using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public int Width, Height, Xcenter, Ycenter, ID;
    private GameObject FloorPrefab, WallPrefab;
    private GameObject Walls, Floors;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(int ID, int Width, int Height, int Xcenter, int YCenter, GameObject WallPrefab, GameObject FloorPrefab)
    {
        this.ID = ID;
        this.Width = Width;
        this.Height = Height;
        this.Xcenter = Xcenter;
        this.Ycenter = YCenter;
        this.WallPrefab = WallPrefab;
        this.FloorPrefab = FloorPrefab;

        this.name = "R" + ID;

        this.transform.position = new Vector3(Xcenter, YCenter, 0);

        Walls = new GameObject("Walls");
        Walls.transform.parent = this.transform;
        Floors = new GameObject("Floors");
        Floors.transform.parent = this.transform;

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
                }
                else
                {
                    Tile = Instantiate(FloorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    Tile.transform.parent = Floors.transform;
                }
            }
        }
    }

    public void AddFloorTile(Vector2 Location)
    {
        GameObject Tile = Instantiate(FloorPrefab, Location, Quaternion.identity);
        Tile.transform.parent = Floors.transform;
    }

    public GameObject[] GetFloors()
    {
        GameObject[] ReturnArray = new GameObject[Floors.transform.childCount];
        for (int i = 0; i < Floors.transform.childCount; i++)
            ReturnArray[i] = Floors.transform.GetChild(i).gameObject;
        return ReturnArray;
    }

    public GameObject[] GetWalls()
    {
        GameObject[] ReturnArray= new GameObject[Walls.transform.childCount];
        for (int i = 0; i < Walls.transform.childCount; i++)
            ReturnArray[i] = Walls.transform.GetChild(i).gameObject;
        return ReturnArray;
    }
}
