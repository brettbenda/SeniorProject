using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMaker : MonoBehaviour {
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject RoomGroup;
    public int NumRooms;
    public Vector2 StartCoordinate;
    public int MaxRoomWidth;
    public int MaxRoomHeight;

    // Use this for initialization
    void Start()
    {
        int StartX = 0;
        int StartY = 0;
        for(int i = 0; i < NumRooms; i++)
        {
            GameObject Room = new GameObject();
            Room.AddComponent<Room>();

            int Width = Random.Range(5, MaxRoomWidth);
            int Height = Random.Range(5, MaxRoomHeight);
            Room.GetComponent<Room>().Initialize(i, Width, Height, StartX, StartY, WallPrefab, FloorPrefab);
            Room.transform.parent = RoomGroup.transform;

            if (Random.Range(0, 100) < 50)
                StartX += (int)Random.Range(MaxRoomWidth/2, 2 * MaxRoomWidth);
            else
                StartY += (int)Random.Range(MaxRoomHeight/2, 2 * MaxRoomHeight);
        }

        Room[] Rooms = RoomGroup.GetComponentsInChildren<Room>();
        for (int i = 0; i < NumRooms; i++)
        {
            for (int j = i+1; j < NumRooms; j++)
            {
                RemoveOverlappingFloors(Rooms[i],Rooms[j]);
            }
            for(int j = 0; j < NumRooms; j++)
            {
                if(i!=j)
                    RemoveOverlappingWalls(Rooms[i], Rooms[j]);
            }
        }


        Destroy(WallPrefab);
        Destroy(FloorPrefab);

    }

    void RemoveOverlappingFloors(Room One, Room Two)
    {
        GameObject[] one_floors = One.GetFloors();
        GameObject[] one_walls = One.GetWalls();

        GameObject[] two_floors = Two.GetFloors();
        GameObject[] two_walls = Two.GetWalls();

        foreach (GameObject o1 in one_floors)
        {
            foreach (GameObject o2 in two_floors)
            {
                if (Vector2.Distance(o1.transform.position, o2.transform.position) == 0)
                {
                    Destroy(o2);
                }
            }
        }
    }

    void RemoveOverlappingWalls(Room One, Room Too)
    {
        GameObject[] one_floors = One.GetFloors();
        GameObject[] one_walls = One.GetWalls();

        GameObject[] two_floors = Too.GetFloors();
        GameObject[] two_walls = Too.GetWalls();
        foreach (GameObject o1 in one_walls)
        {
            foreach (GameObject o2 in two_floors)
            {
                if (Vector2.Distance(o1.transform.position, o2.transform.position) == 0)
                {
                    Destroy(o1);
                }
            }
        }
    }
}
