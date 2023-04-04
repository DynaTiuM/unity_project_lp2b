using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BirdColor : MonoBehaviour
{
    private int color = 0;

    public void changeColor(){
        // Every time the player clicks on the button, we change the bird's color thanks to a switch
        switch(color){
            case 3 :  GetComponent<SpriteRenderer>().color = new Color(1,1,1);
            color = -1;
            break;
            case 0 :  GetComponent<SpriteRenderer>().color = new Color(1,0,0);
            break;
            case 1 :  GetComponent<SpriteRenderer>().color = new Color(0,1,0);
            break;
            case 2 :  GetComponent<SpriteRenderer>().color = new Color(0,0.5f,1);
            break;
        }
        color++;
    }
}
