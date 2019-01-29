using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject focus;
    private bool active;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            Vector3 pos = new Vector3(focus.transform.position.x, focus.transform.position.y, -10);
            this.transform.position = pos;
        }

    }

    public void Toggle() { active = !active; }

    public bool IsActive() { return active; }
}
