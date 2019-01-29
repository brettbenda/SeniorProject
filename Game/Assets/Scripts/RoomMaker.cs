using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoomMaker : MonoBehaviour
{
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject RoomGroup;
    public int NumRooms;
    public int MaxRoomWidth;
    public int MaxRoomHeight;
    public Boolean linear;
    public GameObject Player;
    private List<GameObject> tiles;
    private List<GameObject> walls;
    private List<Room> rooms;
    private EndTile end;

    // Use this for initialization
    void Start()
    {
        CreateMap();
    }

    private void Update()
    {
        if (Input.GetKey("u"))
        {
            DestroyMap();
        }
        if (Input.GetKey("m"))
        {
            CreateMap();
        }

        if (end.Triggered())
        {
            DestroyMap();
            CreateMap();
        }
    }

    private void CreateMap()
    {
        WallPrefab.SetActive(true);
        FloorPrefab.SetActive(true);

        CreateRooms();

        CreateCorridors();

        RemoveOverlap();

        DetermineBestStartEndPoints();

        WallPrefab.SetActive(false);
        FloorPrefab.SetActive(false);
    }

    private void DestroyMap()
    {
        foreach(GameObject o in tiles)
        {
            Destroy(o);
        }
        foreach (GameObject o in walls)
        {
            Destroy(o);
        }
        Destroy(RoomGroup);
        RoomGroup = new GameObject("Rooms");
    }


    //Generation methods
    private void DetermineBestStartEndPoints()
    {
        Room BestStartRoom = null;
        Room BestEndRoom = null;
        float LongestDistance = 0;

        for (int i = 0; i < RoomGroup.transform.childCount; i++)
        {
            Room r = RoomGroup.transform.GetChild(i).GetComponent<Room>();
            for (int j = i + 1; j < RoomGroup.transform.childCount; j++)
            {
                Room r2 = RoomGroup.transform.GetChild(j).GetComponent<Room>();
                float dist = Vector2.Distance(new Vector2(r.Xcenter, r.Ycenter),new Vector2(r2.Xcenter,r2.Ycenter));
                if (dist > LongestDistance)
                {
                    BestStartRoom = r;
                    BestEndRoom = r2;
                    LongestDistance = dist;
                }
            }
        }


        GameObject BestStart = null ;
        GameObject BestEnd = null;

        List<GameObject> R1Tiles = BestStartRoom.GetFloors();
        List<GameObject> R2Tiles = BestEndRoom.GetFloors();
        LongestDistance = 0;

        for (int i = 0; i<R1Tiles.Count; i++)
        {
            GameObject o = R1Tiles[i];
            for (int j = 0; j < R2Tiles.Count; j++)
            {
                GameObject o2 = R2Tiles[j];
                float dist = Vector2.Distance(o.transform.position, o2.transform.position);
                if (dist > LongestDistance)
                {
                    BestStart = o;
                    BestEnd = o2;
                    LongestDistance = dist;
                }
            }
        }

        //TODO: add start/end behavior scripts to the selected tiles
        Player.transform.position = (Vector2)BestStart.transform.position;
        end = BestEnd.AddComponent<EndTile>();
        end.SetPlayer(Player);

        SpriteRenderer StartSprite = BestStart.GetComponent<SpriteRenderer>();
        SpriteRenderer EndSprite = BestEnd.GetComponent<SpriteRenderer>();
        StartSprite.color = Color.green;
        EndSprite.color = Color.red;
    }

    private void RemoveOverlap()
    {
        Room[] Rooms = RoomGroup.GetComponentsInChildren<Room>();
        Debug.Log(Rooms.Length);
        tiles = new List<GameObject>();
        walls = new List<GameObject>();

        //collect all walls/floors
        for (int i = 0; i < RoomGroup.transform.childCount; i++)
        {
            List<GameObject> floors = Rooms[i].GetFloors();
            List<GameObject> walls2 = Rooms[i].GetWalls();
            foreach (GameObject o in floors)
            {
                tiles.Add(o);
            }
            foreach (GameObject o in walls2)
            {
                walls.Add(o);
            }
        }

        //remove overlap
        foreach (GameObject o1 in walls)
        {
            foreach (GameObject o2 in tiles)
            {
                if (Vector2.Distance(o1.transform.position, o2.transform.position) < 0.5f)
                {
                    Destroy(o1);
                    break;
                }
            }
        }
        foreach (GameObject o in tiles)
        {
            o.transform.position = new Vector3( o.transform.position.x, o.transform.position.y, 1);
        }
    }

    private void CreateRooms()
    {
        int StartX = (int)Player.transform.position.x;
        int StartY = (int)Player.transform.position.y;

        //generate "real" rooms
        for (int i = 0; i < NumRooms; i++)
        {
            GameObject Room = new GameObject();
            Room r = Room.AddComponent<Room>();
            rooms = new List<Room>();
            rooms.Add(r);
            int Width = UnityEngine.Random.Range(5, MaxRoomWidth);
            int Height = UnityEngine.Random.Range(5, MaxRoomHeight);
            Room.GetComponent<Room>().Initialize("R"+i, Width, Height, StartX, StartY, WallPrefab, FloorPrefab);
            Room.transform.parent = RoomGroup.transform;

            if (linear)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                    StartX += (int)UnityEngine.Random.Range(0, MaxRoomWidth);
                else
                    StartY += (int)UnityEngine.Random.Range(0, MaxRoomHeight);
            }
            else
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                    StartX += (int)UnityEngine.Random.Range(-1.5f*MaxRoomWidth, 1.5f*MaxRoomWidth);
                else
                    StartY += (int)UnityEngine.Random.Range(-1.5f*MaxRoomHeight, 1.5f*MaxRoomHeight);
            }

        }
    }

    private void CreateCorridors()
    {
        //generate "corridor" rooms;
        Room[] Rooms = RoomGroup.GetComponentsInChildren<Room>();
        for (int i = 0; i < NumRooms - 1; i++)
        {
            GameObject Room = new GameObject();
            Room.AddComponent<Room>();

            int X = (Rooms[i].Xcenter + Rooms[i + 1].Xcenter) / 2;
            int Y = (Rooms[i].Ycenter + Rooms[i + 1].Ycenter) / 2;
            int Width = Math.Max(3, Math.Abs(Rooms[i].Xcenter - Rooms[i + 1].Xcenter));
            int Height = Math.Max(3, Math.Abs(Rooms[i].Ycenter - Rooms[i + 1].Ycenter));

            Room.GetComponent<Room>().Initialize("H" + i + "to" + (i+1), Width, Height, X, Y, WallPrefab, FloorPrefab);
            Room.transform.parent = RoomGroup.transform;
        }
    }
}