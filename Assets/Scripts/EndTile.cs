using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTile : MonoBehaviour
{
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPlayer(GameObject Player)
    {
        this.Player = Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
        {
            Debug.Log("EXIT");
        }
    }

    public bool Triggered()
    {
        if (Player.GetComponent<BoxCollider2D>().IsTouching(this.GetComponent<BoxCollider2D>()))
        {
            return true;
        }
        return false;
    }
}
