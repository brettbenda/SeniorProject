using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject focus;
    private bool active;
	// Use this for initialization
	void Start () {
        active = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            Vector3 pos = new Vector3(focus.transform.position.x, focus.transform.position.y, -10);
            this.transform.position = pos;

            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if( zoom != 0)
            {
                Camera cam = this.GetComponent<Camera>();
                cam.orthographicSize += zoom*cam.orthographicSize;

                if (cam.orthographicSize < 2)
                    cam.orthographicSize = 2;
                if (cam.orthographicSize > 6)
                    cam.orthographicSize = 6;
            }
        }

    }

    public void Toggle() { active = !active; }

    public bool IsActive() { return active; }

    public void SetFocus(GameObject focus) {
        this.focus = focus;
        this.GetComponent<Camera>().orthographicSize = 4.0f;
        Vector3 pos = new Vector3(focus.transform.position.x, focus.transform.position.y, -10);
        this.transform.position = pos;
    }
}
