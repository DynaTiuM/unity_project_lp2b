using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOMusic : MonoBehaviour
{
    public AudioSource Music;
    // Start is called before the first frame update
    void Start()
    {
        Music = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        // If the game is over, we play the music
        Music.Play();
    }
}
