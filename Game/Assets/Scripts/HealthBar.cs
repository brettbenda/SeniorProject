using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private GameObject redBar;
    private GameObject greenBar;
    private int MaxHealth;
    private int CurrentHealth;
    private Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
        redBar = new GameObject("RedBar");
        greenBar = new GameObject("GreenBar");

        redBar.transform.parent = gameObject.transform;
        greenBar.transform.parent = gameObject.transform;

        redBar.transform.localScale = new Vector3(1, 0.1f, 0);
        greenBar.transform.localScale = new Vector3(1, 0.1f, 0);
        offset = new Vector3(0, 0, 0);

        SpriteRenderer rsr = redBar.AddComponent<SpriteRenderer>();
        rsr.sprite = Resources.Load<Sprite>("Square");
        rsr.color = Color.red;

        SpriteRenderer gsr = greenBar.AddComponent<SpriteRenderer>();
        gsr.sprite = Resources.Load<Sprite>("Square");
        gsr.color = Color.green;
    }
    // Update is called once per frame
    void Update()
    {
        redBar.transform.position = gameObject.transform.position + new Vector3(0, 0.3f ,0);
        greenBar.transform.position = gameObject.transform.position + new Vector3(0, 0.3f, 0) - offset;
    }

    public void SetHealth(int max, int current)
    {
        MaxHealth = max;
        CurrentHealth = current;
    }

    public void Hit(int damage)
    {
        CurrentHealth -= damage;
        float percent = (float)CurrentHealth / (float)MaxHealth;
        greenBar.transform.localScale = new Vector3(percent, 0.1f, 0);

        float xOffset = (1.0f - percent) / 4.0f;
        offset = new Vector3(xOffset, 0, 0);
    }
}
