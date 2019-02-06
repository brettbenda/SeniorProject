using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject Target;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
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
