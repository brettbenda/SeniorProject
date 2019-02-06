using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    private Rigidbody2D rb;
    private Vector2 left, right, up, down, zero;
    private Vector2 facing;
    private bool active;
    private Weapon weapon;

    public float speed;
	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        left =  new Vector2(-speed, 0);
        right = new Vector2(speed, 0);
        up =    new Vector2(0, speed);
        down =  new Vector2(0, -speed);
        zero =  new Vector2(0, 0);
        active = true;
        weapon = this.gameObject.AddComponent<Weapon>();
        weapon.SetPlayer(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            rb.velocity = zero;
            if (Input.GetKey("w"))
                rb.velocity += up;
            if (Input.GetKey("a"))
                rb.velocity += left;
            if (Input.GetKey("s"))
                rb.velocity += down;
            if (Input.GetKey("d"))
                rb.velocity += right;

            if(Input.anyKeyDown && rb.velocity!=zero)
                facing = rb.velocity.normalized;
        }

        Debug.DrawRay(this.transform.position, facing);
    }

    public void Toggle(){ active = !active; }

    public bool IsActive() { return active; }

    public Vector2 GetFacingDir() { return facing; }
}
