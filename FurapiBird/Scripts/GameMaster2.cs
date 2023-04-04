using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameMaster2 : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public int score;
    private int timer;
    private float timer_eg = 3f;
    private int length;
    public TextMeshPro distance, highscore, bestDistance, eg;
    public GameObject skin;
    public float pipeSpeed;
    private AudioSource music;
    private AudioSource eg_sound;

    private bool running, ext, already_found;

    protected GameObject utbm_prefab;
    private List<GameObject> newUtbm = new List<GameObject>();

    private float timer_utbm = 0.2f;

    private void Start()
    {


        // when we start for the first time we don't want the gameover image
        gameOver.SetActive(false);
        // At the beginning, the easter egg text isn't displayed
        eg.SetText("");

        // we set our highscore and best distance in the texts
        highscore.SetText("Highscore : " + PlayerPrefs.GetInt("Sauv_HIGHSCORE_fb"));
        bestDistance.SetText("Best Distance : " + PlayerPrefs.GetInt("Sauv_LENGTH_fb") + " m");

        utbm_prefab = Resources.Load<GameObject>("utbm");

        // We take the sound of the easter egg
        music = GetComponent<AudioSource>();
        eg_sound = eg.GetComponent<AudioSource>();
    }
    private void Update()
    {
        // If the game isn't running, it means that the game is over :
        if(!running){
            // We can return to the hub if wanted.
             if (Input.GetKey(KeyCode.Escape)){
                Time.timeScale = 1f;
                StartCoroutine(Exit());
            }
        }

        // If the player's position is higher than 6 and the player hasn't found yet that there is an easter egg :
        if(player.transform.position.y > 6 && !already_found){
            already_found = true;
            // We display that the player has found an easter egg !!
            eg.SetText("YOU FOUND AN EASTER EGG");
            ext = true;
            // We play the sound :)
            eg_sound.Play();
        }
        // also, we display many utbm logos
        if(player.transform.position.y > 6){
            timer_utbm -= Time.deltaTime;
            if(timer_utbm <= 0f){
                // We instantiate many UTBM prefab
                GameObject temp = Instantiate(utbm_prefab);
                newUtbm.Add(temp);
                // With random positions
                float randomX = UnityEngine.Random.value * 17f - 8.5f;
                temp.transform.position = new Vector3(randomX, 6.0f, 0);
                
                // And a timer cooldown of 0.2 seconds
                timer_utbm = 0.2f;
            }
        }

        // We display thoses UTBM logos only if the player is at the outside of the game (ext condition)
        if(ext){
            if(Time.timeScale == 1)
                timer_eg -= Time.deltaTime;
            else
                timer_eg -= Time.deltaTime*10000;
            if(timer_eg <= 0f){
                eg.SetText("");
                timer_eg = 3f;
            }
        }

        // Those line is used to destroy the utbm logos if the player dies
        GameObject tmp = null;

        foreach(GameObject u in newUtbm){
            if(u.transform.position.y <= -5){
                tmp = u;
                break;
            }
        }
        if(newUtbm.Count != 0){
            if(tmp != null)
            {
                newUtbm.Remove(tmp);
                Destroy(tmp);
            }
        }

        // at the beginning we want a speed of 6f 
        if (score == 0)
        {
            pipeSpeed = 6f;
        }
        // we set a timer to have the distance : here for each 50 frames, we have 1m
        timer++;
        if (timer >= 50 && player.enabled==true)
        {
            length++;
            distance.SetText("Distance : " + length + " m");
            timer = 0;
        }
    }


    private void Awake()
    {
        Application.targetFrameRate = 60;
        Pause();
    }

    public void Play()
    {
        running = true;
        eg.SetText("");
        skin.SetActive(false);
        // the music starts when the play button is clicked, then the length and score are reset
        music.Play();
        length = 0;
        distance.SetText("Distance : " + length + " m");
        score = 0;
        scoreText.text = score.ToString();

        // we display the replay button and game over text
        playButton.SetActive(false);
        gameOver.SetActive(false);

        Time.timeScale = 1f;

        // we set the normal animation for the bird and then we can play 
        player.anim.SetTrigger("Normal");
        player.enabled = true;

        //but before playing we remove each pipe if there's some remaining
        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for(int i = 0; i<pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void Pause()
    {
        // when we pause, we can't play with the bird and the background isn't moving
        Time.timeScale = 0.0001f;
        player.enabled = false;
        running = false;
        // If the player dies, we remove every UTBM logos from the list and we destroy them.
        while(newUtbm.Count != 0){
            GameObject tmp = newUtbm.ElementAt(0);
            newUtbm.Remove(tmp);
            Destroy(tmp);
        }
    }
    public void GameOver()
    {
        //we stop the main music, change the animation to the died one, and wait for 2 sec before displaying the game over and replay button
        music.Stop();
        player.anim.SetTrigger("Die");
        
        StartCoroutine(Wait(2f/10000));

        //we set new highscores if we need to
        if(score > PlayerPrefs.GetInt("Sauv_HIGHSCORE_fb"))
        {
            PlayerPrefs.SetInt("Sauv_HIGHSCORE_fb",score) ;
            highscore.SetText("Highscore : " + PlayerPrefs.GetInt("Sauv_HIGHSCORE_fb"));
        }
        if(length > PlayerPrefs.GetInt("Sauv_LENGTH_fb"))
        {
            PlayerPrefs.SetInt("Sauv_LENGTH_fb",length) ;
            bestDistance.SetText("Best Distance : " + PlayerPrefs.GetInt("Sauv_LENGTH_fb") + " m");
        }
        
    }
    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
    

    IEnumerator Wait(float sec)
    {   
        Pause();
        yield return new WaitForSeconds(sec);
     
        gameOver.SetActive(true);
        playButton.SetActive(true);
        skin.SetActive(true);
    }

     IEnumerator Exit()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
