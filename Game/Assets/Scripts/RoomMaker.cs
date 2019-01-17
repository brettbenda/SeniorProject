using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMaker : MonoBehaviour {
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject RoomGroup;
    public int NumRooms;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < NumRooms; i++)
        {
            GameObject room = new GameObject("R" + i);
            room.transform.parent = RoomGroup.transform;
            makeRandomRoom(room);
        }
        //removeDuplicateTiles();
	}

    void makeRandomRoom(GameObject room)
    {
        GameObject walls = new GameObject("Walls");
        walls.transform.parent = room.transform;
        GameObject floors = new GameObject("Floors");
        floors.transform.parent = room.transform;
        int width = Random.Range(6, 12);
        int height = Random.Range(6, 12);
        for(int x = 0; x<width; x++){
            for(int y = 0; y<height; y++){
                GameObject tile;
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    tile = Instantiate(WallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.transform.parent = walls.transform;
                }
                else
                {
                    tile = Instantiate(FloorPrefab, new Vector3(x, y, 1), Quaternion.identity);
                    tile.transform.parent = floors.transform;
                }
            }
        }
    }

    //void removeDuplicateTiles()
    //{

    //    for(int i = 0; i<WallGroup.transform.childCount; i++)
    //    {
    //        Debug.Log("Wall");
    //        GameObject wall = WallGroup.transform.GetChild(i).gameObject;
    //        for(int j = 0; j<FloorGroup.transform.childCount; j++)
    //        {
    //            Debug.Log("Floor");
    //            GameObject tile = FloorGroup.transform.GetChild(j).gameObject;
    //            if ((Vector2)wall.transform.position == (Vector2)tile.transform.position)
    //            {
    //                Destroy(wall);
    //                Debug.Log("killed");
    //            }
    //        }
    //    }
    //}
	
}
