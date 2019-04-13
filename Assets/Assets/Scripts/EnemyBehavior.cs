using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE
{
    SHOOTER, CHASER, BRUTE, SNIPER,  TURRET, NumberOfTypes, BOSS
};

public class EnemyBehavior : MonoBehaviour
{
    static int levelNum = 0;

    public GameObject Target;
    public bool targeting;
    public float targetingRange;
    public ENEMY_TYPE type;
    public float modifier;

    private Rigidbody2D rb;
    private Weapon weapon;
    private Weapon weapon2;
    private HealthBar hb;
    private SpriteRenderer sr;
    private Sprite sprite;
    private BoxCollider2D collider;

    private Vector2 facing;
    private Vector2[] vectors = { new Vector2(0, -1) , new Vector2(-1,0) , new Vector2(0, 1) , new Vector2(1, 0) };
    private int dirCount = 0;
    public int MaxHealth;
    public int CurrentHealth;
    private bool dead;
    private float speed;
    private int touchDamage;
    public float timer;
    public int state = 0;
    public float timerUp = 1.0f;

    // Start is called before the first frame update
    void Awake()
    {
        targeting = false;
        targetingRange = 4;

        facing = new Vector2(0, -1);
        dead = false;

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;

        collider = gameObject.AddComponent<BoxCollider2D>();

        sr = gameObject.AddComponent<SpriteRenderer>();
        sprite = Resources.Load<Sprite>("Enemy");
        sr.sprite = sprite;

        HitManager man = GameObject.Find("[HitManager]").GetComponent<HitManager>();
        man.AddEnemy(this.gameObject);

        hb = gameObject.GetComponent<HealthBar>();
        weapon = this.gameObject.AddComponent<Weapon>();
        weapon2 = this.gameObject.AddComponent<Weapon>();
        modifier = 1.0f + Random.Range(-levelNum / 5.0f, levelNum / 5.0f);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime ;
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
            case ENEMY_TYPE.TURRET:
                TurretBehavior();
                break;
            case ENEMY_TYPE.BOSS:
                BossBehavior();
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

    void TurretBehavior()
    {
        if (timer < 1.0f)
        {
            weapon.Shoot();
        }
        else
        {
            timer = 0;
            dirCount++;
            if (dirCount == 4)
                dirCount = 0;

            facing = vectors[dirCount];
        }
    }

    void BossBehavior()
    {
        if (!targeting)
        {
            CheckIfInRange();
        }
        else if (!dead)
        {

            facing = Target.transform.position - transform.position;
            facing.Normalize();

            switch (state)
            {
                case 0:
                    timerUp = 3.0f;
                    Stay();
                    weapon.Shoot();
                    break;
                case 1:
                    timerUp = 3.0f;
                    Stay();
                    weapon2.Shoot();
                    break;
                case 2:
                    timerUp = 2.0f;
                    Chase();
                    break;
                case 3:
                    timerUp = 1.0f;
                    Avoid();
                    break;
                case 4:
                    timerUp = 3.0f;
                    Chase();
                    weapon.Shoot();
                    break;
                case 5:
                    timerUp = 3.0f;
                    Chase();
                    weapon2.Shoot();
                    break;
            }

            if(timer >= timerUp)
            {
                state = Random.Range(0, 6);
                timer = 0;
            }

            /*
            if (timer < 2.0f)
            {
                weapon.Shoot();
            }
            else if (timer < 5.0f)
            {
                Chase();
            }
            else if (timer < 7.0f)
            {
                Stay();
                weapon2.Shoot();
            }
            else if (timer < 8.0f)
            {
                Avoid();
            }
            else if (timer > 8.0f)
            {
                Stay();
                timer = 0;
            }*/
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
            case ENEMY_TYPE.TURRET:
                SetTurret();
                break;
            case ENEMY_TYPE.BOSS:
                SetBoss();
                break;
        }
    }

    public void SetShooter()
    {
        type = ENEMY_TYPE.SHOOTER;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);

        MaxHealth = (int)(50*modifier);
        CurrentHealth = MaxHealth;
        speed = 1.0f * modifier;

        touchDamage = (int)(10*modifier);

        sr.color = Color.yellow;

        hb.SetHealth(MaxHealth, CurrentHealth);
        weapon.Set(1.0f*modifier, 0.5f*modifier, 3.0f * modifier, (int)(10 * modifier), 1, 0, 1);
    }

    public void SetChaser()
    {
        type = ENEMY_TYPE.CHASER;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        targetingRange = 8;

        MaxHealth = (int)(50 * modifier);
        CurrentHealth = MaxHealth;
        speed = 1.6f;
        touchDamage = (int)(10 * modifier);

        sr.color = Color.magenta;

        hb.SetHealth(MaxHealth, CurrentHealth);
    }

    public void SetBrute()
    {
        type = ENEMY_TYPE.BRUTE;
        gameObject.transform.localScale = new Vector3(1f, 1f, 0);

        MaxHealth = (int)(200 * modifier);
        CurrentHealth = MaxHealth;
        speed = 0.5f * modifier;
        touchDamage = (int)(10 * modifier);

        sr.color = Color.cyan;

        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon.Set(1f * modifier, 0.5f * modifier, 2.0f * modifier, (int)(50 * modifier), 1, 0, 3);
    }

    public void SetSniper()
    {
        type = ENEMY_TYPE.SNIPER;
        gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0);
        targetingRange = 10;

        MaxHealth = (int)(50 * modifier);
        CurrentHealth = MaxHealth;
        speed = 1.0f * modifier;
        touchDamage = (int)(10 * modifier);

        sr.color = Color.white;

        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon.Set(0.33f * modifier, 0.7f * modifier, 3.0f * modifier, (int)(50 * modifier), 1, 0, 1);
    }

    public void SetTurret()
    {
        type = ENEMY_TYPE.TURRET;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        targetingRange = 1000;

        MaxHealth = (int)(100 * modifier);
        CurrentHealth = MaxHealth;
        speed = 1.0f * modifier;
        touchDamage = (int)(10 * modifier);

        sr.color = Color.white;
        sprite = Resources.Load<Sprite>("Turret");
        sr.sprite = sprite;

        collider.isTrigger = true;

        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon.Set(3 , 0.5f * modifier, 3.0f * modifier, (int)(50 * modifier), 1, 0, 3);
    }

    public void SetBoss()
    {
        type = ENEMY_TYPE.BOSS;
        gameObject.transform.localScale = new Vector3(3f, 3f, 0);

        MaxHealth = (int)(400 * modifier);
        CurrentHealth = MaxHealth;
        speed = 0.75f * modifier;
        touchDamage = (int)(20 * modifier);
        targetingRange = 5.0f;

        float r = UnityEngine.Random.Range(0.5f, 1.0f);
        float g = UnityEngine.Random.Range(0.5f, 1.0f);
        float b = UnityEngine.Random.Range(0.5f, 1.0f);

        sr.color = new Color(r,g,b);
        sprite = Resources.Load<Sprite>("Boss");
        sr.sprite = sprite;

        hb.SetHealth(MaxHealth, CurrentHealth);

        weapon.Set(2.5f * modifier, 0.1f * modifier, 4.0f * modifier, (int)(20 * modifier), 3, 45, 3);
        weapon2.Set(4f * modifier, 0.1f * modifier, 5.0f * modifier, (int)(20 * modifier), 1, 0, 3);
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

    public static void IncrementLevel()
    {
        levelNum++;
    }
}
