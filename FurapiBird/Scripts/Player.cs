using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Vector3 direction;
    private float gravity = (float)(-9.8*1.5);
    private float strength = 5*1.5f;
    public GameObject skin;
    private AudioSource music;
    public Animator anim;

    private void Start()
    {
        //we set our components into variables to utilise them
        music = GetComponent<AudioSource>();
        anim = skin.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        // if the bird fall, we die
        if (transform.position.y < -4.5)
        {
            FindObjectOfType<GameMaster2>().GameOver();
            music.Play();
        }

        // if we use the space or up key, the bird fly up. The bird can't go higher than 6,7 on y.
        if (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.Space) && transform.position.y < 6.7)
        {
            direction = Vector3.up * strength;
        }

        // but every time the bird is falling if we don't use the space of up key
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    private void OnEnable()
    {
        // at the beginning, the bird is reset
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if we touch a pipe, we die
        if (collision.gameObject.tag == "Obstacle")
        {
            FindObjectOfType<GameMaster2>().GameOver();
            music.Play();
        }

        // if we touch the scoring collider, our score is increased
        else if (collision.gameObject.tag == "Scoring")
        {
            FindObjectOfType<GameMaster2>().IncreaseScore();
        }
    }
}


