using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject focus;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = new Vector3(focus.transform.position.x, focus.transform.position.y, -10);
        this.transform.position = pos;
	}
}
