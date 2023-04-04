using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleScript : MonoBehaviour
{
    public Sprite classical_sprite;
    public Sprite poisoned_sprite;
    public Sprite golden_sprite;
    public Sprite clock_sprite;
    private bool poisoned;
    private bool golden;
    private bool clock;
    private const int damage = 5;
    private bool destroyed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
        // If the apple's position on y if less than -10, we destroy it
        if (transform.position.y < -10.0f)
        {
            Destroy(gameObject);
        }
    }

    // Setters
    public void setGolden(bool g){
        this.golden = g;
    }
    public void setPoisoned(bool e){
        this.poisoned = e;
    }
    public void setClock(bool c){
        this.clock = c;
    }

    //React to a collision (collision start)
    void OnCollisionEnter2D(Collision2D col)
    {
        // This condition is verifying if the position of the apple is correctly on the intervall [-0.961/3 ; 0,961/3]
        // So that the apple only enter in the basket if the player aims correctly
        if(FindObjectOfType<Player_Script>().transform.position.x + 1.6/2 >= gameObject.transform.position.x + 0.961/3
        && FindObjectOfType<Player_Script>().transform.position.x - 1.6/2 <= gameObject.transform.position.x - 0.961/3){
            // Depending on the type of the apple, we change the properties of the game or the player's score
            if(poisoned){
                if(FindObjectOfType<SpawnerScript>().getScore() >= 5){
                    FindObjectOfType<SpawnerScript>().newScore(-5);
                }
                else FindObjectOfType<SpawnerScript>().newScore(0);
            }
            else if(golden){
                FindObjectOfType<SpawnerScript>().newScore(6);
                FindObjectOfType<Player_Script>().boostPlayer();
            }
            else if(clock){
                FindObjectOfType<Player_Script>().changeClock();
            }
            else{
                FindObjectOfType<SpawnerScript>().newScore(1);  
            }
            Destroy(gameObject);
        }
    }
}