using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public float rate;
    public float size;
    List<GameObject> bullets;
    private GameObject owner;
    private float time;

    // Start is called before the first frame update
    void Awake()
    {
        rate = .5f;
        size = 0.1f;
        bullets = new List<GameObject>();
        HitManager man = GameObject.Find("HitManager").GetComponent<HitManager>();
        man.AddWeapon(this);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        foreach(GameObject b in bullets.ToArray())
        {
            if(b.GetComponent<Bullet>().GetAge() > 1.0f)
            {
                Destroy(b);
                bullets.Remove(b);
            }
        }
    }

    //Is called to active the weapon
    public void Shoot()
    {
        if (time > 1.0f / rate)
        {
            Debug.Log("Weapon Active");
            SingleStream();
            time = 0;
        }
    }

    //Sets Owner of the weapon
    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    //Gets the weapon's owner
    public GameObject GetOwner() { return owner; }

    //Bullets in a single stream, sources at the owner
    public void SingleStream()
    {
        GameObject bullet = new GameObject("Bullet");
        Bullet b = bullet.AddComponent<Bullet>();

        Vector2 tragectory;
        if (owner.GetComponent<PlayerControls>() == null)
            tragectory = owner.GetComponent<Rigidbody2D>().velocity;
        else
            tragectory = owner.GetComponent<PlayerControls>().GetFacingDir();

        float offset = owner.transform.localScale.x;
        b.Set(owner, tragectory, size, offset);

        bullets.Add(bullet);
    }


    public List<GameObject> GetBullets()
    { 
        return bullets;
    }
}
