using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public float timeSinceLastHit;
    public int experience;
    public int nextLevelXP;
    public int level;
    public int allocatedLevels;
    public TextMeshPro text;
    private float textTimer;

    public float speed;
	// Use this for initialization
	void Start () {
        reset();
    }

    public void reset()
    {
        rb = this.GetComponent<Rigidbody2D>();
        active = true;

        weapon = this.gameObject.AddComponent<Weapon>();
        weapon.Set(2f, 0.2f, 3.0f, 10, 1, 0, 2);

        hb = this.GetComponent<HealthBar>();
        MaxHealth = 100;
        CurrentHealth = 100;
        dead = false;
        hb.SetHealth(MaxHealth, CurrentHealth);
        speed = 1.5f;

        experience = 0;
        nextLevelXP = 500;

        HitManager man = GameObject.Find("[HitManager]").GetComponent<HitManager>();
        man.SetPlayer(this.gameObject);
        text.text = "";
        textTimer = 0;
    }

    // Update is called once per frame
    void Update() {
        textTimer += Time.deltaTime;
        timeSinceLastHit += Time.deltaTime;
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

            //Levelup
            if(experience >= nextLevelXP)
            {
                LevelUp();
                textTimer = 0;
                text.text = "Level Up!";
            }
        }

        if (textTimer >= 3.0f)
        {
            text.text = "";
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
        if (!dead && timeSinceLastHit>1)
        {
            float damage = b.GetDamage();
            if (damage > CurrentHealth)
            {
               hb.Hit(CurrentHealth);
                CurrentHealth = 0;
            }
            else {
                
                CurrentHealth -= b.GetDamage();
                hb.Hit(b.GetDamage());         
            }

            
            timeSinceLastHit = 0;
        }

        if (CurrentHealth <= 0)
        {
            active = false;
            dead = true;
            rb.velocity = Vector2.zero;
        }
    }

    public void Hit(int damage)
    {
        if (!dead && timeSinceLastHit > 1)
        {
            CurrentHealth -= damage;
            hb.Hit(damage);
            timeSinceLastHit = 0;
        }

        if (CurrentHealth <= 0)
        {
            active = false;
            dead = true;
            rb.velocity = Vector2.zero;
        }
    }

    public void LevelUp()
    {
        level++;
        nextLevelXP = (int)(nextLevelXP*1.5f);

        speed += 0.15f;
        MaxHealth += 10;
        CurrentHealth = MaxHealth;
        hb.SetHealth(MaxHealth, CurrentHealth);
    }

    public void AwardExperience(int xp)
    {
        this.experience += xp;
    }

    public void IncreaseRate()
    {
        if (allocatedLevels < level)
        {
            weapon.rate += 0.3f;
            allocatedLevels++;
        }

    }

    public void IncreaseWeaponSpeed()
    {
        if (allocatedLevels < level)
        {
            weapon.speed += 0.3f;
            allocatedLevels++;
        }
    }

    public void IncreaseWeaponDmg()
    {
        if (allocatedLevels < level)
        {
            weapon.damage += 5;
            allocatedLevels++;
        }
    }
    public void IncreaseBullets()
    {
        if (allocatedLevels+weapon.n <= level)
        {
            if(weapon.n == 1)
            {
                weapon.theta = 30;
            }
            allocatedLevels += weapon.n;
            weapon.n++;
            
        }
    }

    public bool isDead()
    {
        return (CurrentHealth <= 0);
    }
}
