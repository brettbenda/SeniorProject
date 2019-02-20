using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private SpriteRenderer sr;
    private new CircleCollider2D collider;
    private Rigidbody2D rb;
    private Sprite sprite;
    private GameObject parent;
    private float age;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        age = 0;
        damage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        age += Time.deltaTime;
    }

    //Adds the attributes
    public void Set(GameObject parent, Vector2 tragectory, float size)
    {
        transform.parent = parent.transform;
        transform.position = parent.transform.position;

        sprite = Resources.Load<Sprite>("Bullet");
        
        transform.localScale = new Vector2(size, size);

        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        collider = gameObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.velocity = tragectory + parent.GetComponent<Rigidbody2D>().velocity;
    }

    //returns age
    public float GetAge() { return age; }

    public int GetDamage() { return damage; }
}
