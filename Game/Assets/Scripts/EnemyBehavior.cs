using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject Target;
    public bool targeting;
    public float targetingRange;

    private Rigidbody2D rb;
    private Weapon weapon;
    private HealthBar hb;

    private Vector2 facing;
    public int MaxHealth;
    public int CurrentHealth;
    private bool dead;
    // Start is called before the first frame update
    void Start()
    {
        targeting = false;
        targetingRange = 4;
        facing = new Vector2(0, -1);

        MaxHealth = 100;
        CurrentHealth = 10;
        dead = false;

        rb = this.GetComponent<Rigidbody2D>();
        HitManager man = GameObject.Find("HitManager").GetComponent<HitManager>();
        man.AddEnemy(this.gameObject);

        hb = gameObject.AddComponent<HealthBar>();
        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon = this.gameObject.AddComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targeting)
        {
            CheckIfInRange();
        }
        else if (!dead)
        {
            facing = Target.transform.position - transform.position;
            facing.Normalize();
            if (DistanceRange(0.5f))
            {
                Avoid();
                this.weapon.Shoot();
            }
            else if (DistanceRange(0.7f))
            {
                Stay();
                this.weapon.Shoot();
            }
            else
            {
                Chase();
                this.weapon.Shoot();
            }
        }

    }

    //Switches enemy from inactive to active
    void CheckIfInRange()
    {
        if(Vector2.Distance(this.transform.position, Target.transform.position) < targetingRange)
        {
            targeting = true;
        }
    }

    //Determines if the player is too close to the enemy
    bool DistanceRange(float percent)
    {
        if (Vector2.Distance(this.transform.position, Target.transform.position) < targetingRange*percent)
        {
            return true;
        }
        return false;
    }

    //Enemy moves towards player
    void Chase()
    {
        Vector2 TargetDirection = Target.transform.position - this.transform.position;
        rb.velocity = TargetDirection.normalized;
    }

    //Enemy runs from player
    void Avoid()
    {
        Vector2 TargetDirection =  this.transform.position - Target.transform.position;
        rb.velocity = TargetDirection.normalized;
    }

    void Stay()
    {
        rb.velocity = Vector2.zero;
    }
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
            dead = true;
            rb.velocity = Vector2.zero;
        }
    }
}
