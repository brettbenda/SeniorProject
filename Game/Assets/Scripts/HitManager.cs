using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> enemies;
    public List<Weapon> weapons;
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        weapons = new List<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Weapon w in weapons)
        {
            foreach(GameObject b in w.GetBullets().ToArray())
            {
                //Ignore self damage
                if (PlayerHit(b) && w.GetOwner()!= player)
                {
                    Debug.Log("Player hit");
                    player.GetComponent<PlayerControls>().Hit(b.GetComponent<Bullet>());
                    w.GetBullets().Remove(b);
                    Destroy(b);
                }
                foreach(GameObject e in enemies)
                {
                    //ignore self damage
                    if (EnemyHit(e, b) && w.GetOwner() != e)
                    {
                        Debug.Log("Enemy hit");
                        e.GetComponent<EnemyBehavior>().Hit(b.GetComponent<Bullet>());
                        w.GetBullets().Remove(b);
                        Destroy(b);
                    }
                }
            }
        }
    }

    //Determines if a buller hits the player
    bool PlayerHit(GameObject bullet)
    {
        //bullet hits player
        if (bullet.GetComponent<CircleCollider2D>().bounds.Intersects(player.GetComponent<BoxCollider2D>().bounds))
        {
            return true;
        }
        return false;
    }

    //Determines if a bullet hits the given enemy
    bool EnemyHit(GameObject enemy, GameObject bullet)
    {
        if (bullet.GetComponent<CircleCollider2D>().bounds.Intersects(enemy.GetComponent<BoxCollider2D>().bounds))
        {
            return true;
        }
        return false;
    }


    public void AddEnemy(GameObject Obj)
    {
        enemies.Add(Obj);
    }

    public void AddWeapon(Weapon Obj)
    {
        weapons.Add(Obj);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
