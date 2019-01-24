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
    public Vector2 StartCoordinate;
    public int MaxRoomWidth;
    public int MaxRoomHeight;
    public Boolean linear;

    // Use this for initialization
    void Start()
    {
        CreateRooms();

        CreateCorridors();

        RemoveOverlap();

        Destroy(WallPrefab);
        Destroy(FloorPrefab);

    }

    private void RemoveOverlap()
    {
        Room[] Rooms = RoomGroup.GetComponentsInChildren<Room>();
        Debug.Log(Rooms.Length);
        List<GameObject> tiles = new List<GameObject>();
        List<GameObject> walls = new List<GameObject>();

        Debug.Log(RoomGroup.transform.childCount);

        //collect all walls/floors
        for (int i = 0; i < RoomGroup.transform.childCount; i++)
        {
            GameObject[] floors = Rooms[i].GetFloors();
            GameObject[] walls2 = Rooms[i].GetWalls();
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
                    Debug.Log("Destroyed");
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
        int StartX = 0;
        int StartY = 0;

        //generate "real" rooms
        for (int i = 0; i < NumRooms; i++)
        {
            GameObject Room = new GameObject();
            Room.AddComponent<Room>();

            int Width = UnityEngine.Random.Range(5, MaxRoomWidth);
            int Height = UnityEngine.Random.Range(5, MaxRoomHeight);
            Room.GetComponent<Room>().Initialize(i, Width, Height, StartX, StartY, WallPrefab, FloorPrefab);
            Room.transform.parent = RoomGroup.transform;

            if (linear)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                    StartX += (int)UnityEngine.Random.Range(0, 2 * MaxRoomWidth);
                else
                    StartY += (int)UnityEngine.Random.Range(0, 2 * MaxRoomHeight);
            }
            else
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                    StartX += (int)UnityEngine.Random.Range(-2 * MaxRoomWidth, 2 * MaxRoomWidth);
                else
                    StartY += (int)UnityEngine.Random.Range(-2 * MaxRoomHeight, 2 * MaxRoomHeight);
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

            Room.GetComponent<Room>().Initialize(100 + i, Width, Height, X, Y, WallPrefab, FloorPrefab);
            Room.transform.parent = RoomGroup.transform;
        }
    }
}