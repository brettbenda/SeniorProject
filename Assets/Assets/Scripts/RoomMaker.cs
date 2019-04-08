using System;
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
    public int NumEnemies;
    public int seed;
    public int levelNum;
    private List<GameObject> tiles;
    private List<GameObject> walls;
    private List<Room> rooms;
    private EndTile end;
    public bool ended = false;
    public bool boss = false;
    public EnemyBehavior bossEnemy;

    // Use this for initialization
    void Start()
    {
       // UnityEngine.Random.InitState(seed);
        WallPrefab.SetActive(false);
        FloorPrefab.SetActive(false);
        Player.SetActive(false);
        levelNum = 0;
        EnemyBehavior.IncrementLevel();
        //CreateMap();
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

        if(end != null)
        {
            if (end.Triggered())
            {
                if (boss && bossEnemy == null)
                {
                    DestroyMap();
                    ended = true;
                }
                else if (!boss)
                {
                    DestroyMap();
                    BossMap();
                    boss = true;
                }
            }
        }
        
    }

    public void CreateMap()
    {
        float r = UnityEngine.Random.Range(0.5f, 1.0f);
        float g = UnityEngine.Random.Range(0.5f, 1.0f);
        float b = UnityEngine.Random.Range(0.5f, 1.0f);
        WallPrefab.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
        FloorPrefab.GetComponent<SpriteRenderer>().color = new Color(r, g, b);

        levelNum++;
        
        EnemyBehavior.IncrementLevel();
        boss = false;
        ended = false;
        Player.SetActive(true);
        WallPrefab.SetActive(true);
        FloorPrefab.SetActive(true);
        Player.SetActive(true);
        CreateRooms();

        CreateCorridors();

        RemoveOverlap();

        DetermineBestStartEndPoints();

        AddEnemies();

        WallPrefab.SetActive(false);
        FloorPrefab.SetActive(false);
    }

    public void BossMap()
    {
        Player.SetActive(true);
        WallPrefab.SetActive(true);
        FloorPrefab.SetActive(true);
        Player.SetActive(true);

        int StartX = (int)Player.transform.position.x;
        int StartY = (int)Player.transform.position.y;


        rooms = new List<Room>();
        GameObject Room = new GameObject();
        Room r = Room.AddComponent<Room>();
        rooms.Add(r);

        int Width = 15;
        int Height = 15;
        r.Initialize("Boss", Width, Height, StartX, StartY, WallPrefab, FloorPrefab);
        Room.transform.parent = RoomGroup.transform;

        RemoveOverlap();
        DetermineBestStartEndPoints();


        GameObject enemy = new GameObject("Enemy");
        enemy.transform.position = GameObject.Find("Player").transform.position + new Vector3(7.5f, 7.5f,0) ;
        enemy.AddComponent<HealthBar>();
        bossEnemy = enemy.AddComponent<EnemyBehavior>();
        bossEnemy.SetType(ENEMY_TYPE.BOSS);
        bossEnemy.SetTarget(GameObject.Find("Player"));

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
        foreach (Room r in rooms)
        {
            Destroy(r);
        }
        Destroy(RoomGroup);
        GameObject.Find("[HitManager]").GetComponent<HitManager>().Clear();
        RoomGroup = new GameObject("Rooms");
        Player.SetActive(false);
    }


    //Generation methods
    private void AddEnemies()
    {
        for(int i = 0; i<NumEnemies; i++)
        {
            int rand = UnityEngine.Random.Range(0, tiles.Count);
            Vector2 pos = tiles[rand].gameObject.transform.position;
            if(Vector2.Distance(pos, Player.transform.position) > 10)
            {
                rand = UnityEngine.Random.Range(0,(int)ENEMY_TYPE.NumberOfTypes);
                GameObject enemy = new GameObject("Enemy");
                enemy.transform.position = pos;
                enemy.AddComponent<HealthBar>();
                EnemyBehavior eb = enemy.AddComponent<EnemyBehavior>();
                eb.SetType((ENEMY_TYPE)rand);
                eb.SetTarget(GameObject.Find("Player"));
            }
            else
            {
                i--;
            }
        }
    }

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

        if(RoomGroup.transform.childCount == 1)
        {
            BestStartRoom = RoomGroup.transform.GetChild(0).GetComponent<Room>();
            BestEndRoom = RoomGroup.transform.GetChild(0).GetComponent<Room>();
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

        /*sr = gameObject.AddComponent<SpriteRenderer>();
        sprite = Resources.Load<Sprite>("Enemy");
        sr.sprite = sprite;*/

        SpriteRenderer StartSprite = BestStart.GetComponent<SpriteRenderer>();
        SpriteRenderer EndSprite = BestEnd.GetComponent<SpriteRenderer>();

        Sprite startSprite = Resources.Load<Sprite>("Start");
        StartSprite.sprite = startSprite;

        Sprite endSprite = Resources.Load<Sprite>("Start");
        EndSprite.sprite = endSprite;
        StartSprite.color = new Color(0.75f,1.0f,0.75f);
        EndSprite.color = new Color(1.0f, 0.75f,0.75f);
    }

    private void RemoveOverlap()
    {
        tiles = new List<GameObject>();
        walls = new List<GameObject>();

        //collect all walls/floors
        for (int i = 0; i < rooms.Count; i++)
        {
            List<GameObject> floors = rooms[i].GetFloors();
            List<GameObject> walls2 = rooms[i].GetWalls();
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
        foreach (GameObject o1 in walls.ToArray())
        {
            foreach (GameObject o2 in tiles)
            {
                if (Vector2.Distance(o1.transform.position, o2.transform.position) < 0.5f)
                {
                    
                    foreach (Room r in rooms)
                    {
                        if (r.GetWalls().Contains(o1))
                            r.RemoveWall(o1);
                    }
                    walls.Remove(o1);
                    Destroy(o1);
                    break;
                }
            }
        }
        foreach (GameObject o in tiles)
        {
            o.transform.position = new Vector3( o.transform.position.x, o.transform.position.y, 1);
        }

        GameObject.Find("[HitManager]").GetComponent<HitManager>().SetWalls(walls);
    }

    private void CreateRooms()
    {
        int StartX = (int)Player.transform.position.x;
        int StartY = (int)Player.transform.position.y;
        rooms = new List<Room>();
        //generate "real" rooms
        for (int i = 0; i < NumRooms; i++)
        {
            GameObject Room = new GameObject();
            Room r = Room.AddComponent<Room>();
            rooms.Add(r);

            int Width = UnityEngine.Random.Range(5, MaxRoomWidth);
            int Height = UnityEngine.Random.Range(5, MaxRoomHeight);
            r.Initialize("R"+i, Width, Height, StartX, StartY, WallPrefab, FloorPrefab);
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
        for (int i = 0; i < NumRooms - 1; i++)
        {
            GameObject Room = new GameObject();
            Room r = Room.AddComponent<Room>();
            rooms.Add(r);

            int X = (rooms[i].Xcenter + rooms[i + 1].Xcenter) / 2;
            int Y = (rooms[i].Ycenter + rooms[i + 1].Ycenter) / 2;
            int Width = Math.Max(3, Math.Abs(rooms[i].Xcenter - rooms[i + 1].Xcenter));
            int Height = Math.Max(3, Math.Abs(rooms[i].Ycenter - rooms[i + 1].Ycenter));

            r.Initialize("H" + i + "to" + (i+1), Width, Height, X, Y, WallPrefab, FloorPrefab);
            Room.transform.parent = RoomGroup.transform;
        }
    }

    public bool IsOver() { return ended; }

}