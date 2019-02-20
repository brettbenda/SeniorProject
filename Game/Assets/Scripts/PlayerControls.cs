using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    private Rigidbody2D rb;
    private HealthBar hb;
    private Vector2 facing;
    private bool active;
    private bool dead;
    private Weapon weapon;

    public int MaxHealth;
    public int CurrentHealth;

    public float speed;
	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        active = true;

        weapon = this.gameObject.AddComponent<Weapon>();

        hb = this.GetComponent<HealthBar>();
        MaxHealth = 100;
        CurrentHealth = 100;
        dead = false;
        hb.SetHealth(MaxHealth, CurrentHealth);


        HitManager man = UnityEngine.GameObject.Find("HitManager").GetComponent<HitManager>();
        man.SetPlayer(this.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (active)
        {
            //Movement
            rb.velocity = Vector2.zero;
            if (Input.GetKey("w"))
                rb.velocity += Vector2.up * speed;
            if (Input.GetKey("a"))
                rb.velocity += Vector2.left * speed;
            if (Input.GetKey("s"))
                rb.velocity += Vector2.down * speed;
            if (Input.GetKey("d"))
                rb.velocity += Vector2.right*speed;

            //Shooting
            if (Input.GetKey("up"))
            {
                facing = Vector2.up;
                weapon.Shoot();
            }
            else if (Input.GetKey("left"))
            {
                facing = Vector2.left;
                weapon.Shoot();
            }
            else if (Input.GetKey("down"))
            {
                facing = Vector2.down;
                weapon.Shoot();
            }
            else if (Input.GetKey("right"))
            {
                facing = Vector2.right;
                weapon.Shoot();
            }
        }

        Debug.DrawRay(this.transform.position, facing);
    }

    //Switches activation state of player
    public void Toggle(){ active = !active; }

    //Determines if player is active
    public bool IsActive() { return active; }

    //Get direction the player is facing
    public Vector2 GetFacingDir() { return facing; }

    public void Hit(Bullet b)
    {
        if (!dead)
        {
            CurrentHealth -= b.GetDamage();
            hb.Hit(b.GetDamage());
        }

        if (CurrentHealth <= 0)
        {
            active = false;
            dead = true;
            rb.velocity = Vector2.zero;
        }
    }
}
