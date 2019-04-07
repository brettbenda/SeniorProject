using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{
    public PlayerControls player;
    public Text pointsText;
    public Text weaponRateInfo;
    public Text weaponSpeedInfo;
    public Text weaponDmgInfo;
    public Text weaponBulletInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = "Points Left:" + (player.level - player.allocatedLevels);
        weaponRateInfo.text = player.GetComponent<Weapon>().rate.ToString("0.0") + "->" + (player.GetComponent<Weapon>().rate + 0.3f).ToString("0.0") + "[1]";
        weaponSpeedInfo.text = player.GetComponent<Weapon>().speed.ToString("0.0") + "->" + (player.GetComponent<Weapon>().speed + 0.3f).ToString("0.0") + "[1]";
        weaponDmgInfo.text = player.GetComponent<Weapon>().damage.ToString() + "->" + (player.GetComponent<Weapon>().damage + 5).ToString() + "[1]";
        weaponBulletInfo.text = player.GetComponent<Weapon>().n.ToString() + "->" + (player.GetComponent<Weapon>().n + 1).ToString() + "[" + player.GetComponent<Weapon>().n.ToString() + "]";
    }
}
