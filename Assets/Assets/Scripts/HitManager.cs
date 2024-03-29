﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> enemies;
    public List<Weapon> weapons;
    public List<GameObject> walls;


    public void Clear()
    {
        foreach (GameObject e in enemies)
        {
            Destroy(e);
        }
        enemies = new List<GameObject>();

        weapons = new List<Weapon>();
        weapons.Add(player.GetComponent<Weapon>());
        player.GetComponent<Weapon>().GetBullets().Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        weapons = new List<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Weapon w in weapons.ToArray())
        {
            foreach(GameObject b in w.GetBullets().ToArray())
            {
                //Ignore self damage
                if (PlayerHit(b) && w.GetOwner()!= player)
                {
                    player.GetComponent<PlayerControls>().Hit(b.GetComponent<Bullet>());
                    w.GetBullets().Remove(b);
                    Destroy(b);
                    break;
                }
                foreach(GameObject e in enemies.ToArray())
                {
                    //ignore self damage
                    if (EnemyHit(e, b) && w.GetOwner() != e && w.GetOwner().GetComponent<EnemyBehavior>() == null)
                    {
                        e.GetComponent<EnemyBehavior>().Hit(b.GetComponent<Bullet>());

                        w.GetBullets().Remove(b);
                        Destroy(b);

                        if(e.GetComponent<EnemyBehavior>().CurrentHealth <= 0)
                        {
                            weapons.Remove(e.GetComponent<Weapon>());
                            enemies.Remove(e);
                            player.GetComponent<PlayerControls>().AwardExperience(e.GetComponent<EnemyBehavior>().MaxHealth);
                            Weapon[] weps = e.GetComponents<Weapon>();
                            for(int i = 0; i<weps.Length; i++)
                            {
                                Destroy(weps[i]);
                            }
                            Destroy(e);

                            break;
                        }
                        break;
                    }
                }
                foreach (GameObject wa in walls.ToArray())
                {
                    if(WallHit(wa, b))
                    {
                        w.GetBullets().Remove(b);
                        Destroy(b);
                        break;
                    }
                }
            }
        }

        foreach (GameObject e in enemies.ToArray())
        {
            //ignore self damage
            Bounds e_b = e.GetComponent<BoxCollider2D>().bounds;
            Bounds p_b = player.GetComponent<BoxCollider2D>().bounds;

            e_b.Expand(new Vector3(0.05f, 0.05f, 0));

            if (e_b.Intersects(p_b))
            {
                player.GetComponent<PlayerControls>().Hit(e.GetComponent<EnemyBehavior>().GetTouchDamage());
            }

            e_b.Expand(new Vector3(-0.05f, -0.05f, 0));
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

    bool WallHit(GameObject wall, GameObject bullet){
        if (bullet.GetComponent<CircleCollider2D>().bounds.Intersects(wall.GetComponent<BoxCollider2D>().bounds))
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

    public void SetWalls(List<GameObject> walls) { this.walls = walls; }

}
