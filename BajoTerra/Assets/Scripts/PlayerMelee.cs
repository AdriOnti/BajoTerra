using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    private Animator m_Animator;
    public static PlayerMelee instance;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
}
