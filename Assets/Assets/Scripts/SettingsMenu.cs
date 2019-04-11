using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public RoomMaker rm;
    public Text size;
    public Text num_rooms;
    public Text num_enemies;
    public Text mode;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        size.text = rm.MaxRoomHeight.ToString();
        num_rooms.text = rm.NumRooms.ToString();
        num_enemies.text = rm.NumEnemies.ToString();
        mode.text = rm.linear ? "Linear" : "Non-Linear";
    }
}
