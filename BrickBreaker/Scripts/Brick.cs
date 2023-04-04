using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public AudioSource Music;
    public int hitsToBreak = 1;
    public Sprite hitSprite;
    // Start is called before the first frame update
    void Start()
    {
        Music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Music.Play();
        // If the break going to be destroyed, we destroy it when the ball enters in collision with it
        if (hitsToBreak == 1)
        {
            Destroy(gameObject);
        }
    }

    public void BreakBrick()
    {
        hitsToBreak--;
        GetComponent<SpriteRenderer>().sprite = hitSprite;
    }

}
