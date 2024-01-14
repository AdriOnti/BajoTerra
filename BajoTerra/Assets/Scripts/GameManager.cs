using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int trapType;
    public static GameManager Instance;
    private List<Text> playerHUD = new List<Text>();
    private GameObject pause;
    private GameObject death;
    private GameObject inventoryBtn;

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
        GetDeadMenu();
        GetInventoryBtn();
        trapType = 0;
    }

    // Obtain any element whose type is Text
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

    // Obtiene el menu de pausa y lo desactiva
    public void GetPauseMenu()
    {
        GameObject canvas = GameObject.Find("Canvas");
        pause = canvas.transform.GetChild(1).gameObject;
        pause.SetActive(false);
    }

    // Obtiene el menu de muerte y lo desactiva
    public void GetDeadMenu()
    {
        GameObject canvas = GameObject.Find("Canvas");
        death = canvas.transform.GetChild(2).gameObject;
        death.SetActive(false);
    }

    public void GetInventoryBtn()
    {
        GameObject canvas = GameObject.Find("Canvas");
        inventoryBtn = canvas.transform.GetChild(4).gameObject;
    }

    // Funcion que devuelve el TextObject que tiene el nombre que se busca
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

        foreach (Text player in playerHUD)
        {
            if (player.gameObject.name == stat) return player;
        }
        return null;
    }


    // Pausar juego
    public void PauseGame() { pause.SetActive(true); inventoryBtn.SetActive(false); }

    // Reanudar juego
    public void PlayGame() { pause.SetActive(false); inventoryBtn.SetActive(true); }

    // Muerte del jugador
    public void GameOver() { death.SetActive(true); inventoryBtn.SetActive(false); }
}
