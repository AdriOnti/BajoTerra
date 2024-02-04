using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource m_AudioSource;
    public List<AudioClip> audios;
    public static SoundManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(int id)
    {
        m_AudioSource.PlayOneShot(audios[id]);
    }
}
