using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMaker : MonoBehaviour {
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public int NumRooms;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < NumRooms; i++)
        {
            int width = Random.Range(6, 12);
            int height = Random.Range(6, 12);
            for(int x = 0; x<width; x++)
            {
                for(int y = 0; y<height; y++)
                {
                    if(x==0 || y==0 || x==width-1 || y==height-1)
                        Instantiate(WallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        Instantiate(FloorPrefab, new Vector3(x, y, 1), Quaternion.identity);
                }
                
            }
        }
	}
	
}
