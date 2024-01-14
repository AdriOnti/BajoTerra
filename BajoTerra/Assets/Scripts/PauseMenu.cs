using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable() { Time.timeScale = 0.0f; }

    private void OnDisable() { Time.timeScale = 1.0f; }
    private void Update()
    {
        if (this.gameObject.activeSelf == true)
        {
            //Time.timeScale = 0;
            //if (Input.GetKeyUp(KeyCode.Escape)) Resume();
        }
    }

    public void Resume() { GameManager.Instance.PlayGame(); }
    public void Restart() { SceneManager.LoadScene("BajoTerra"); }
    public void Back() { SceneManager.LoadScene("Main"); }
}
