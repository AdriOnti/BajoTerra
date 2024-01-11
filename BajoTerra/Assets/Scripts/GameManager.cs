using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int trapType;
    public static GameManager Instance;
    private List<Text> playerHUD = new List<Text>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetHUD();
        trapType = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHUD() 
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject hud = canvas.transform.GetChild(0).gameObject;

        GameObject[] ui = new GameObject[hud.transform.childCount];
        for (int i = 0; i < ui.Length; i++) ui[i] = hud.transform.GetChild(i).gameObject;

        for (int i = 0; i < hud.transform.childCount; i++)
        {
            switch (ui[i].GetComponentInChildren<Text>().gameObject.name)
            {
                case "HealthTXT":
                    playerHUD.Add(ui[i].GetComponentInChildren<Text>());
                    break;
                case "AttackTXT":
                    playerHUD.Add(ui[i].GetComponentInChildren<Text>());
                    break;
                case "SpeedTXT":
                    playerHUD.Add(ui[i].GetComponentInChildren<Text>());
                    break;
                case "WeaponTXT":
                    playerHUD.Add(ui[i].GetComponentInChildren<Text>());
                    break;
            }
        }
    }

    public Text PlayerStat(string stat)
    {
        switch (stat)
        {
            case "health":
            case "Health":
                stat = "HealthTXT";
                break;
            case "attack":
            case "Attack":
                stat = "AttackTXT";
                break;
            case "speed":
            case "Speed":
                stat = "SpeedTXT";
                break;
            case "weapon":
            case "Weapon":
                stat = "WeaponTXT";
                break;
        }

        foreach(Text player in playerHUD)
        {
            if(player.gameObject.name == stat) return player;
        }
        return null;
    }

    //public Text PlayerHP()
    //{
    //    foreach (Text player in playerHUD)
    //    {
    //        if (player.gameObject.name == "HealthTXT")
    //        {
    //            return player;
    //        }
    //    }
    //    return null;
    //}

    //public Text PlayerAttack()
    //{
    //    foreach (Text player in playerHUD)
    //    {
    //        if (player.gameObject.name == "AttackTXT")
    //        {
    //            return player;
    //        }
    //    }
    //    return null;
    //}

    //public Text PlayerSpeed()
    //{
    //    foreach (Text player in playerHUD)
    //    {
    //        if (player.gameObject.name == "SpeedTXT")
    //        {
    //            return player;
    //        }
    //    }
    //    return null;
    //}

    //public Text PlayerWeapon()
    //{
    //    foreach (Text player in playerHUD)
    //    {
    //        if (player.gameObject.name == "WeaponTXT")
    //        {
    //            return player;
    //        }
    //    }
    //    return null;
    //}
}
