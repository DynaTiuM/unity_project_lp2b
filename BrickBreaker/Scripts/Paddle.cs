using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public AudioSource paddleMusic;
    public Ball ball;
    private int coinAmount = 5;
    // Start is called before the first frame update
    void Start()
    {
        paddleMusic = GetComponent<AudioSource>();
    }

    
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x >= 6.39) transform.Translate(0, 0, 0);
            else transform.Translate(10f * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x <= -6.32) transform.Translate(0, 0, 0);
            else transform.Translate(-10f * Time.deltaTime, 0, 0);
        }
    }

    public float GetX()
    {
        return (float) transform.position.x;
    }

    void OnCollisionEnter2D(Collision2D col) {
        paddleMusic.Play();
    }

    // If the paddle touches a heart, a coin :
    void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is a heart :
        if (collision.name == "ExtraLife_pf(Clone)") {
            // We add a life
            ball.lives++;
            ball.lives_text.SetText("Lives remaining : " + ball.lives);
            Destroy(collision.gameObject);
        }
        // Otherwise :
        if (collision.name == "Coin_pf(Clone)") {
            // We add coins into our wallet
            ball.coins+=coinAmount;
            Destroy(collision.gameObject);
        }
    }
}

