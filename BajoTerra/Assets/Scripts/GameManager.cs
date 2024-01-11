using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int trapType;
    public static GameManager Instance;
    private List<Text> playerHUD = new List<Text>();
    private GameObject pause;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetHUD();
        GetPauseMenu();
        trapType = 0;
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

    public void GetPauseMenu()
    {
        GameObject canvas = GameObject.Find("Canvas");
        pause = canvas.transform.GetChild(1).gameObject;

        pause.SetActive(false);
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

    public void PauseGame() { pause.SetActive(true); }

    public void PlayGame() { pause.SetActive(false); }
}
