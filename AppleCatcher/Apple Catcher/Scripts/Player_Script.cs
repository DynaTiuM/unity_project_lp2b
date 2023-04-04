
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Script : MonoBehaviour
{

    //---------------------------------------------------------------------------------
    // ATTRIBUTES
    //---------------------------------------------------------------------------------

    private bool boosted;
    private bool clock;
    protected Animator ref_animator;
    public SpawnerScript Spawner;
    public GameObject spawner;
    private float SPEED = 8f;
    private float timer_golden = 8f;
    private float timer_clock = 10f;

    //---------------------------------------------------------------------------------
    // METHODS
    //---------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        ref_animator = GetComponent<Animator>();
        Spawner = spawner.GetComponent<SpawnerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Manage movement speed and animations
        float newSpeed = 0;

        // If the player is boosted, it means he grabed a golden apple. If the timer of the golden apple is also >= 0
        if(timer_golden >= 0 && boosted){
            // We modify the player's speed
            SPEED = 15f;
            // We reduce the timer_golden variable
            timer_golden -= Time.deltaTime;
        }
        // Else, it means the timer is over
        else{
            // We set the timer_golden timer to original
            timer_golden = 8f;
            // The player isn't boosted anymore
            boosted = false;
            // We set the player's speed to original
            SPEED = 10f;
        }

        // Same for timer_clock, but we modify the Time.timeScale
        if(timer_clock >=0 && clock){
            Time.timeScale = 0.82f;
            timer_clock -= Time.deltaTime;
        }
        else{
            timer_clock = 10f;
            clock = false;
            Time.timeScale = 1f;
        }

        if(spawner.GetComponent<SpawnerScript>().isRunning()){
            if (Input.GetKey(KeyCode.LeftArrow) && gameObject.transform.position.x > -8f)
            {
                newSpeed = -SPEED;
                ref_animator.SetBool("isForwards", false);
            }
            else if ( Input.GetKey(KeyCode.RightArrow) && gameObject.transform.position.x < 8f)
            {
                newSpeed = SPEED;
                ref_animator.SetBool("isForwards", true);
            }
        }
        //Inform animator : Are we moving?
        ref_animator.SetBool("isMoving", newSpeed != 0);


        //Move with the speed found
        transform.Translate(newSpeed * Time.unscaledDeltaTime, 0, 0);
    }


    public void boostPlayer(){
        boosted = true;
    }

    public void changeClock(){
        clock = true;
        // We add time to the game if the player grabed a clock
        FindObjectOfType<SpawnerScript>().addTime();
    }

}
