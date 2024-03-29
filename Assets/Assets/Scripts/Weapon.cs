﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public float rate;
    public float size;
    public float speed;
    public int n;
    public float theta;
    public float maxBulletAge;
    public int damage;
    List<GameObject> bullets;
    private float time;

    // Start is called before the first frame update
    void Awake()
    {
        rate = 2.0f;
        size = 0.3f;
        speed = 4.0f;
        n = 1;
        theta = 0;
        maxBulletAge = 3.0f;
        damage = 10;
        bullets = new List<GameObject>();
        HitManager man = GameObject.Find("[HitManager]").GetComponent<HitManager>();
        man.AddWeapon(this);
    }

    public void Set(float rate, float size, float speed, int damage, int n, float theta, float maxBulletAge)
    {
        this.rate = rate;
        this.size = size;
        this.speed = speed;
        this.n = n;
        this.theta = theta;
        this.maxBulletAge = maxBulletAge;
        this.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        foreach(GameObject b in bullets.ToArray())
        {
            if(b.GetComponent<Bullet>().GetAge() > maxBulletAge)
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
            NStream(n,theta);
            time = 0;
        }
    }

    //Gets the weapon's owner
    public GameObject GetOwner() { return gameObject; }

    //Bullets in a single stream, sources at the owner
    public void NStream(int n, float theta)
    {
        Vector2 tragectory;
        if (gameObject.GetComponent<PlayerControls>() == null)
            tragectory = gameObject.GetComponent<EnemyBehavior>().GetFacingDir() * speed;
        else
            tragectory = gameObject.GetComponent<PlayerControls>().GetFacingDir().normalized * speed;

        float rad = theta * Mathf.PI / 180.0f;
        float dTheta = rad / (n-1);
        float sTheta = -rad / 2;

        for(int i = 0; i<n; i++)
        {
            GameObject bullet = new GameObject("Bullet");
            Bullet b = bullet.AddComponent<Bullet>();

            float x = Mathf.Cos(sTheta) * tragectory.x - Mathf.Sin(sTheta) * tragectory.y;
            float y = Mathf.Sin(sTheta) * tragectory.x + Mathf.Cos(sTheta) * tragectory.y;
            Vector2 trag = new Vector2(x, y);

            b.Set(gameObject, trag, size, damage);

            bullets.Add(bullet);
            sTheta += dTheta;
        }
        
    }


    public List<GameObject> GetBullets()
    { 
        return bullets;
    }
}
