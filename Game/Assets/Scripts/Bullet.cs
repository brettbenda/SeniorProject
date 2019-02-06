using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{

    private SpriteRenderer sr;
    private CircleCollider2D collider;
    private Rigidbody2D rb;
    private Sprite sprite;
    private GameObject bullet;
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Bullet(GameObject parent, Vector2 tragectory, float size, float offset)
    {
        bullet = new GameObject("Bullet");
        bullet.transform.parent = parent.transform;
        bullet.transform.position = parent.transform.position+(Vector3)tragectory*offset;

        sprite = Resources.Load<Sprite>("Bullet");
        
        bullet.transform.localScale = new Vector2(size, size);

        sr = bullet.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        collider = bullet.AddComponent<CircleCollider2D>();
        rb = bullet.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.velocity = tragectory + parent.GetComponent<Rigidbody2D>().velocity;
    }

    public GameObject GetObj() { return bullet; }
}
