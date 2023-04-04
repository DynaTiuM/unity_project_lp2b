using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject brick_pf;
    public Ball ball;
    public AudioSource win;
    public Sprite oneHitSprite;
    public Sprite twoHitsSprite;
    // Start is called before the first frame update
    void Start()
    {
        win = GetComponent<AudioSource>();
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player has destroyed every brick, we recreate a new level and add 1000 points on the player's score
        if (ball.nbBrickTouche == nbBrick)
        {
            win.Play();
            if (ball.transform.position.y < -0.44f)
                {
                // Create new level thanks to this method
                CreateLevel();
                ball.score = ball.score + 1000;
                ball.nbBrickTouche = 0;
                }
            
        }
    }

    public int nbBrick = 0;

    // This method is dedicated to create a level of bricks
    void CreateLevel()
    {  
        // At first, we initialise our variables to 0
        nbBrick = 0;
        ball.nbBrickTouche = 0;
        // Thanks to a double for loop, we create an Array of bricks
        for (float y = 2.8f; y > -0.44f; y = y - 0.7f)
        {
            int n = 0;
            for (float x = -6.52f; x < 0f; x=x+1.63f)
            {
                n++;
                float s = 8.15f-n*1.63f;
                if (Random.Range(0, 100) < 70)
                {
                    // We instanciate the bricks and increase the nbBrick variable
                    GameObject newBrick = Instantiate(brick_pf);
                    newBrick.transform.position = new Vector2(x, y);
                    nbBrick++;
                    // This method is choosing the type of brick, if it will need 1 or 2 strikes in order to destroy it
                    HitGenerator(newBrick);

                    // We do the same process in order to create a symmetry
                    GameObject newSymBrick = Instantiate(brick_pf);
                    newSymBrick.transform.position = new Vector2(s, y);
                    nbBrick++;
                    HitGenerator(newSymBrick);
                }
                    
            }

            // As the array is symmetrically created (the number of columns is odd), we need to add a column at the center of the game
            if (Random.Range(0, 100) < 70)
            {
                GameObject newBrick = Instantiate(brick_pf);
                newBrick.transform.position = new Vector2(0f, y);
                nbBrick++;
            }
        }

        
    }

    void HitGenerator(GameObject brick)
    {
        if (Random.Range(1, 3) == 1)
        {
            brick.gameObject.GetComponent<Brick>().hitsToBreak = 1;
            brick.GetComponent<SpriteRenderer>().sprite = oneHitSprite;
        }
        else
        {
            brick.gameObject.GetComponent<Brick>().hitsToBreak = 2;
            brick.GetComponent<SpriteRenderer>().sprite = twoHitsSprite;
        }
    }

}
