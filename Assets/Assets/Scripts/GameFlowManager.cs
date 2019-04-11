﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public RoomMaker rm;
    public GameObject startMenu;
    public GameObject statsMenu;
    public GameObject settingsMenu;
    public PlayerControls player;
    public CameraController GameCamera;
    public string state;
    // Start is called before the first frame update
    void Start()
    {
        state = "start_menu";
        GameCamera.SetFocus(startMenu);
        statsMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == "game")
        {
            if (rm.IsOver()){
                ProcessInput("stats_menu");
            }
        }
        else
        {
            if (GameCamera.IsActive())
            {
                GameCamera.Toggle();
            }
        }
    }

    public void ProcessInput(string message)
    {
        Debug.Log(message);
        if (message == "game")
        {
            state = "game";
            GameCamera.SetFocus(player.gameObject);
            GameCamera.Toggle();
            startMenu.SetActive(false);
            statsMenu.SetActive(false);
            settingsMenu.SetActive(false);
            rm.CreateMap();
        }
        if (message == "start_menu")
        {
            state = "start_menu";
            GameCamera.SetFocus(startMenu);
            startMenu.SetActive(true);
            statsMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
        if (message == "stats_menu")
        {
            state = "stats_menu";
            GameCamera.SetFocus(statsMenu);
            startMenu.SetActive(false);
            statsMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }
        if (message == "settings_menu")
        {
            Debug.Log("!");
            state = "settings_menu";
            GameCamera.Toggle();
            GameCamera.SetFocus(settingsMenu);
            startMenu.SetActive(false);
            statsMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }
    }
}
