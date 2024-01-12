using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Camera cam;
    public Transform target;
    public Vector3 offset;

    /// <summary>
    /// Cada vez que se llama a esta funcion, la barra de vida del enemigo se actualiza
    /// </summary>
    /// <param name="current">La vida que tiene el enemigo a lo largo de la partida</param>
    /// <param name="max">La vida maxima que tiene el enemigo</param>
    public void UpdateBar(int current, int max) {  slider.value = (float)current / max; }

    // Que la barra de vida siga al enemigo y que mantenga la rotación de la camara
    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
