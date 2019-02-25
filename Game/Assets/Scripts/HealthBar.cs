using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private GameObject redBar;
    private GameObject greenBar;
    public int MaxHealth;
    public int CurrentHealth;
    public Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
        greenBar = new GameObject("GreenBar");
        redBar = new GameObject("RedBar");
        
        greenBar.transform.parent = gameObject.transform;
        redBar.transform.parent = gameObject.transform;
        
        greenBar.transform.localScale = new Vector3(1, 0.1f, 0);
        redBar.transform.localScale = new Vector3(1, 0.1f, 0);

        offset = new Vector3(0, 0, 1);

        SpriteRenderer gsr = greenBar.AddComponent<SpriteRenderer>();
        gsr.sprite = Resources.Load<Sprite>("Square");
        gsr.color = Color.green;
        gsr.sortingOrder = 1;
        SpriteRenderer rsr = redBar.AddComponent<SpriteRenderer>();
        rsr.sprite = Resources.Load<Sprite>("Square");
        rsr.color = Color.red;
        gsr.sortingOrder = 2;

        
    }
    // Update is called once per frame
    void Update()
    {
        greenBar.transform.position = gameObject.transform.position + new Vector3(0, 0.6f*gameObject.transform.localScale.y, 0) - offset;
        redBar.transform.position = gameObject.transform.position + new Vector3(0, 0.6f * gameObject.transform.localScale.y, 0);
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
        offset = new Vector3(xOffset, 0, 1);
    }
}
