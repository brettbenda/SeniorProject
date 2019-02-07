using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject Target;
    private Rigidbody2D rb;
    private Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        HitManager man = GameObject.Find("HitManager").GetComponent<HitManager>();
        man.AddEnemy(this.gameObject);

        weapon = this.gameObject.AddComponent<Weapon>();
        weapon.SetOwner(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
        this.weapon.Shoot();
    }

    void Chase()
    {
        Vector2 TargetDirection = Target.transform.position - this.transform.position;
        rb.velocity = TargetDirection.normalized;
    }

    void Avoid()
    {
        Vector2 TargetDirection =  this.transform.position - Target.transform.position;
        rb.velocity = TargetDirection.normalized;
    }
}
