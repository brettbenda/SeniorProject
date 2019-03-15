using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE
{
    SHOOTER, CHASER, BRUTE, SNIPER, NumberOfTypes
};

public class EnemyBehavior : MonoBehaviour
{
    public GameObject Target;
    public bool targeting;
    public float targetingRange;
    public ENEMY_TYPE type;

    private Rigidbody2D rb;
    private Weapon weapon;
    private HealthBar hb;
    private SpriteRenderer sr;
    private Sprite sprite;
    private BoxCollider2D collider;

    private Vector2 facing;
    public int MaxHealth;
    public int CurrentHealth;
    private bool dead;
    private float speed;
    private int touchDamage;

    // Start is called before the first frame update
    void Awake()
    {
        //SetShooter();
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case ENEMY_TYPE.SHOOTER:
                ShooterBehavior();
                break;
            case ENEMY_TYPE.CHASER:
                ChaserBehavior();
                break;
            case ENEMY_TYPE.BRUTE:
                BruteBehavior();
                break;
            case ENEMY_TYPE.SNIPER:
                SniperBehavior();
                break;
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



    //BEHAVIORS
    void ShooterBehavior()
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

    void ChaserBehavior()
    {
        if (!targeting)
        {
            CheckIfInRange();
        }
        else if (!dead)
        {
            Chase();
        }
    }

    void BruteBehavior()
    {
        if (!targeting)
        {
            CheckIfInRange();
        }
        else if (!dead)
        {
            facing = Target.transform.position - transform.position;
            facing.Normalize();
            if (DistanceRange(0.7f))
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

    void SniperBehavior()
    {
        if (!targeting)
        {
            CheckIfInRange();
        }
        else if (!dead)
        {
            facing = Target.transform.position - transform.position;
            facing.Normalize();
            Stay();
            weapon.Shoot();
        }
    }
    //Enemy moves towards player
    void Chase()
    {
        Vector2 TargetDirection = Target.transform.position - this.transform.position;
        rb.velocity = TargetDirection.normalized*speed;
    }

    //Enemy runs from player
    void Avoid()
    {
        Vector2 TargetDirection =  this.transform.position - Target.transform.position;
        rb.velocity = TargetDirection.normalized*speed;
    }

    void Stay()
    {
        rb.velocity = Vector2.zero;
    }



    //ACCESSORS
    public Vector2 GetFacingDir() { return facing; }


    //MUTATORS
    public void SetType(ENEMY_TYPE type)
    {
        switch (type)
        {
            case ENEMY_TYPE.SHOOTER:
                SetShooter();
                break;
            case ENEMY_TYPE.CHASER:
                SetChaser();
                break;
            case ENEMY_TYPE.BRUTE:
                SetBrute();
                break;
            case ENEMY_TYPE.SNIPER:
                SetSniper();
                break;
        }
    }

    public void SetShooter()
    {
        type = ENEMY_TYPE.SHOOTER;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        targeting = false;
        targetingRange = 4;
        facing = new Vector2(0, -1);

        MaxHealth = 100;
        CurrentHealth = 100;
        dead = false;
        speed = 1;

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;
        touchDamage = 10;

        collider = gameObject.AddComponent<BoxCollider2D>();

        sr = gameObject.AddComponent<SpriteRenderer>();
        sprite = Resources.Load<Sprite>("Square");
        sr.sprite = sprite;
        sr.color = Color.yellow;

        HitManager man = GameObject.Find("[HitManager]").GetComponent<HitManager>();
        man.AddEnemy(this.gameObject);

        hb = gameObject.GetComponent<HealthBar>();
        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon = this.gameObject.AddComponent<Weapon>();
    }

    public void SetChaser()
    {
        type = ENEMY_TYPE.CHASER;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        targeting = false;
        targetingRange = 8;
        facing = new Vector2(0, -1);

        MaxHealth = 50;
        CurrentHealth = 50;
        dead = false;
        speed = 1.6f;
        touchDamage = 10;

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;

        collider = gameObject.AddComponent<BoxCollider2D>();

        sr = gameObject.AddComponent<SpriteRenderer>();
        sprite = Resources.Load<Sprite>("Square");
        sr.sprite = sprite;
        sr.color = Color.magenta;

        HitManager man = GameObject.Find("[HitManager]").GetComponent<HitManager>();
        man.AddEnemy(this.gameObject);

        hb = gameObject.GetComponent<HealthBar>();
        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon = this.gameObject.AddComponent<Weapon>();
    }

    public void SetBrute()
    {
        type = ENEMY_TYPE.BRUTE;
        gameObject.transform.localScale = new Vector3(1f, 1f, 0);
        targeting = false;
        targetingRange = 4;
        facing = new Vector2(0, -1);

        MaxHealth = 200;
        CurrentHealth = 200;
        dead = false;
        speed = 0.5f;
        touchDamage = 10;

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;

        collider = gameObject.AddComponent<BoxCollider2D>();

        sr = gameObject.AddComponent<SpriteRenderer>();
        sprite = Resources.Load<Sprite>("Square");
        sr.sprite = sprite;
        sr.color = Color.cyan;

        HitManager man = GameObject.Find("[HitManager]").GetComponent<HitManager>();
        man.AddEnemy(this.gameObject);

        hb = gameObject.GetComponent<HealthBar>();
        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon = this.gameObject.AddComponent<Weapon>();
        weapon.Set(1f, 0.5f, 2.0f, 50, 1, 0, 1);
    }

    public void SetSniper()
    {
        type = ENEMY_TYPE.SNIPER;
        gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0);
        targeting = false;
        targetingRange = 10;
        facing = new Vector2(0, -1);

        MaxHealth = 50;
        CurrentHealth = 50;
        dead = false;
        speed = 1.0f;
        touchDamage = 10;

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;

        collider = gameObject.AddComponent<BoxCollider2D>();

        sr = gameObject.AddComponent<SpriteRenderer>();
        sprite = Resources.Load<Sprite>("Square");
        sr.sprite = sprite;
        sr.color = Color.white;

        HitManager man = GameObject.Find("[HitManager]").GetComponent<HitManager>();
        man.AddEnemy(this.gameObject);

        hb = gameObject.GetComponent<HealthBar>();
        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon = this.gameObject.AddComponent<Weapon>();
        weapon.Set(0.33f, 0.2f, 7.0f, 50, 1, 0, 1);
    }

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

    public void SetTarget(GameObject target)
    {
        this.Target = target;
    }

    public int GetTouchDamage()
    {
        return touchDamage;
    }
}
