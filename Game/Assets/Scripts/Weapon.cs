using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public float rate;
    public float size;
    List<GameObject> bullets;
    private GameObject owner;
    private Sprite sprite;
    private float time;
    private bool playerOwned;

    // Start is called before the first frame update
    void Awake()
    {
        rate = .5f;
        size = 0.1f;
        bullets = new List<GameObject>();
        sprite = Resources.Load<Sprite>("Bullet");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space") && playerOwned)
        {
            if (time > 1.0f / rate)
            {
                Debug.Log("Weapon Active");
                SingleStream();
                time = 0;
            }
        }
        time += Time.deltaTime;
    }

    public void SetPlayer(GameObject player) {
        this.owner = player;
        this.playerOwned = true;
    }

    public void SingleStream()
    {
        Vector2 tragectory = owner.GetComponent<PlayerControls>().GetFacingDir();
        float offset = owner.transform.localScale.x;
        Bullet b = new Bullet(owner, tragectory, size, offset);
        bullets.Add(b.GetObj());
    }

}
