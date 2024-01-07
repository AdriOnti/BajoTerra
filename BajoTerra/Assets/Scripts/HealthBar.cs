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


    public void UpdateBar(int current, int max)
    {
        slider.value = (float)current / max;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
