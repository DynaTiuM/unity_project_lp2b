using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    //Initialize variables
    public Paddle paddle;
    public int lives = 3;
    public int score;
    public int HIGHSCORE;
    public int coins;
    private int scoreAmount = 50;
    public int nbBrickTouche = 0;
    public TextMeshPro lives_text;
    public TextMeshPro score_text;
    public AudioSource Music;
    public Transform extraLife;
    private int skin = 0;
    public Transform extraCoins;
    public Sprite dbz, pm, pride, pepsi, mozilla, beachball;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        // Choosing the skin
        skin = PlayerPrefs.GetInt("Sauv_skin");
        // Depending, on the skin, we set a different sprite on the ball
        switch(skin){
            case 1 : GetComponent<SpriteRenderer>().sprite = dbz;
            break;
            case 2 : GetComponent<SpriteRenderer>().sprite = pm;
            break;
            case 3 : GetComponent<SpriteRenderer>().sprite = pride;
            break;
            case 4 : GetComponent<SpriteRenderer>().sprite = mozilla;
            break;
            case 5 : GetComponent<SpriteRenderer>().sprite = beachball;
            break;
            case 6 : GetComponent<SpriteRenderer>().sprite = pepsi;
            break;
        }
    

        score = 0;
        // We get the highscore
        HIGHSCORE = PlayerPrefs.GetInt("Sauv_HIGHSCORE");
        Music = GetComponent<AudioSource>();

        //Displaying the text
        lives_text.SetText("Lives remaining : " + lives);
        score_text.SetText("Score : " + score);
        StartCoroutine(Wait(2f));

    }
    void Update()
    {

// -------------------------------
    // If the angle of the ball in relation to the horizontal vector (0;1) is less than 50, we adjust the vector of the ball
        if(angleCalculator() < 50){
            if(GetComponent<Rigidbody2D>().velocity.y < 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x/5, -1);
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x/5, 1);
        }
    // This is important in order to prevent the ball to have a horizontal vector direction, which can lead to problems 
// -------------------------------            

        //If the ball is falling
        if (transform.position.y <= -5) {
            lives--;
            lives_text.SetText("Lives remaining : " + lives);
            Music.Play();

            //removing 500 points at each fall
            if (lives != 0) {
                if (score > 500)
                {
                        score = score - 4*scoreAmount;
                        score_text.SetText("Score : " + score);
                }
                else
                {
                    score = 0;
                    score_text.SetText("Score : " + score);
                }
            }
            else //if the game is finished with no life
            {
                if (HIGHSCORE < score)
                {
                    HIGHSCORE = score;
                }
                
                //Saving the score and checking if ther's a new highscore
                PlayerPrefs.SetInt("Sauv_score", score);
                PlayerPrefs.SetInt("Sauv_HIGHSCORE", HIGHSCORE);
                PlayerPrefs.SetInt("Sauv_coin", PlayerPrefs.GetInt("Sauv_coin")+coins);

                //Getting into the game over scene
                StartCoroutine(GameOver());
            }

            //Putting the ball at its starting position 
            transform.position = new Vector2(0, -1.5f);
            StartCoroutine(Wait(2f));
            }

        //if the velocity is too low, we increase it manually
        if (GetComponent<Rigidbody2D>().velocity.magnitude != 7)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * 7;
        }
     
    }

    IEnumerator Wait(float sec)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(sec);
        
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -7);
    }

    //Changing the scene into the Game over one
    IEnumerator GameOver()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameOverScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Brick")
        {
            //Is the brick gonna break ?
            if (col.gameObject.GetComponent<Brick>().hitsToBreak > 1)
            {
                // No : we change the sprite to be the broken one
                col.gameObject.GetComponent<Brick>().BreakBrick();
            }
            else
            {
                //Yes : are we gonna have a power up ?
                if (UnityEngine.Random.Range(0, 100) > 90)
                {
                    // Yes : an extra-life !
                    Instantiate(extraLife, col.transform.position, col.transform.rotation);
                } else
                {
                    // Yes : a coin !
                    if (UnityEngine.Random.Range(0, 100) > 70)
                    {
                        Instantiate(extraCoins, col.transform.position, col.transform.rotation);
                    }
                }

                // Yes : we have more points and a brick is broken
                score = score + scoreAmount;
                score_text.SetText("Score : " + score);
                nbBrickTouche++;
            }
        }

        if (col.gameObject.name == "Paddle") {
            angle = angleCalculator();
           
            GetComponent<Rigidbody2D>().velocity += new Vector2((GetX() - paddle.GetX())*3, 0);
           
            // If the ball's angle is included into [0;50] intervall, we modify this angle in order not to have a horizontal vector direction.
            if(angle < 50 && angle >= 0){
                 GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x/4, GetComponent<Rigidbody2D>().velocity.y);
           }
        }
    }

    // This method calculates the angle between two vectors
    private float angleCalculator(){
        Vector2 u, v;
        u = new Vector2(1,0);
        v = GetComponent<Rigidbody2D>().velocity;
        float theta;
        theta = (float)(Math.Acos((ScalarProduct(u,v))/(u.magnitude*v.magnitude)) * 180/Math.PI);
        if(v.x < 0) {
            theta = 180f - theta;
        }
        return theta;
    }
    // this method is coded in order to calculate the scalar product between two vector
    private int ScalarProduct(Vector2 u, Vector2 v){
        return (int)(u.x * v.x + u.y * v.y);
    }

    public float GetX()
    {
        return (float)transform.position.x;
    }
}
