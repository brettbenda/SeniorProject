using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE
{
    SHOOTER, CHASER, BRUTE, NumberOfTypes
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

        MaxHealth = 200;
        CurrentHealth = 200;
        dead = false;
        speed = 1.4f;

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

        MaxHealth = 500;
        CurrentHealth = 500;
        dead = false;
        speed = 0.5f;

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
}
