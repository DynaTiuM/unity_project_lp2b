using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class SpawnerScript : MonoBehaviour
{
    public AudioClip ref_audioClip;
    public SpriteRenderer fader_renderer;

    protected GameObject apple_prefab, gameOver_prefab, playAgain_prefab;

    protected float timer = 3f;
    protected AudioSource ref_audioSource;
    protected float current_alpha = 1;

    private GameObject newApple;

    private float global_time = 120f; 

    public int score;
    public TextMeshPro displayed_text, timer_text, highscore_text;

    private bool running;

    protected AppleScript appleScript;
    public Player_Script playerScript;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Sauv_HIGHSCORE_ac",0);

        gameOver_prefab = Resources.Load<GameObject>("GameOver");
        apple_prefab = Resources.Load<GameObject>("Apple_prefab");
        playAgain_prefab = Resources.Load<GameObject>("PlayAgain");

        playerScript = player.GetComponent<Player_Script>();

        ref_audioSource = gameObject.AddComponent<AudioSource>();
        ref_audioSource.clip = ref_audioClip;

        StartCoroutine(FadeOutFromWhite());

        // We get the highscore that was registered in the Sauv_HIGHSCORE_ac file
        highscore_text.SetText("Highscore : " + PlayerPrefs.GetInt("Sauv_HIGHSCORE_ac"));

        // We consider that the game is running when the scene is loaded
        running = true;
    }

    // Update is called once per frame
    void Update()
    {

        // If the game isn't running, it means that the game is over
        if(!running){
            // If the score is higher than the highscore, we set it as the new Highscore
            if(score > PlayerPrefs.GetInt("Sauv_HIGHSCORE_ac"))
            {
                PlayerPrefs.SetInt("Sauv_HIGHSCORE_ac",score);
                highscore_text.SetText("Highscore : " + PlayerPrefs.GetInt("Sauv_HIGHSCORE_ac"));
            }
        }
        // If the player pess M, we return to the menu
        if (Input.GetKey(KeyCode.M))
            {
                returnToMenu();
            }
        // If the global time is less than 0, it means that the game is over
        if(global_time <= 0 && running){
            running = false;
            // We instantiate gameOver and a playAgain text
            Instantiate(gameOver_prefab);
            Instantiate(playAgain_prefab);
            // We also instantiate another text if there is a new highscore
            GameObject e = Instantiate(playAgain_prefab);
            e.GetComponent<TextMeshPro>().SetText("New Highscore !");
            e.transform.position = new Vector2(0,-3);
        }
        // Otherwise, if the game is running,
        else if (running){
            // We display the clock of the game
            timer -= Time.deltaTime;
            global_time -= Time.deltaTime;
            int min = (int)(global_time/60);
            int sec = (int)(global_time%60);
            if(sec < 10)timer_text.SetText("Time : 0" + min + ":0" + sec);
            else timer_text.SetText("Time : 0" + min + ":" + sec);
        }

        if ( timer <= 0 && running)
        {
            // We instantiate new apples randomly
            float randomX = UnityEngine.Random.value * 17f - 8.5f;

            newApple = Instantiate(apple_prefab);
            appleScript = newApple.GetComponent<AppleScript>();
            newApple.transform.position = new Vector3(randomX, 6.0f, 0);

            int rand = UnityEngine.Random.Range(0,120);
            
            // Depending on rand variable, the apple that is falling can have many properties
            if(rand > 90){
                newApple.GetComponent<AppleScript>().setPoisoned(true);
                newApple.GetComponent<SpriteRenderer>().sprite = appleScript.poisoned_sprite;
            }
            else if(rand < 10){
                newApple.GetComponent<AppleScript>().setGolden(true);
                newApple.GetComponent<SpriteRenderer>().sprite = appleScript.golden_sprite;
            }
            else if(rand < 20){
                newApple.GetComponent<AppleScript>().setClock(true);
                newApple.GetComponent<SpriteRenderer>().sprite = appleScript.clock_sprite;
            }
            else{
                newApple.GetComponent<SpriteRenderer>().sprite = appleScript.classical_sprite;
            }
           
            timer = 0.5f + UnityEngine.Random.value*1f ;
        }
    }

    // We add time if the player grab a clock
    public void addTime(){
        global_time += 5f;
    }

    public bool isRunning(){
        return running;
    }

    // We modify the score
    public void newScore(int s){
        if(s == 0){
            score = 0;
        }
        else{
            score += s;
        }
        ref_audioSource.Play();
        // We modify the score's text
        displayed_text.SetText("Score : " + score);
    }
    public int getScore(){
        return score;
    }

    // If the game isn't running, we return to the menu thanks to this method
    public void returnToMenu(){
        if(!running) StartCoroutine(LoadMenu());
    }


    IEnumerator LoadMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Title");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    //Coroutine to fade out from white/launch music with a delay
    IEnumerator FadeOutFromWhite()
    {
        yield return new WaitForSeconds(0.5f);

        while (current_alpha > 0)
        {
            current_alpha -= Time.deltaTime / 2;
            fader_renderer.color = new Color(1, 1, 1, current_alpha);
            yield return null;
        }

        Destroy(fader_renderer.gameObject);

    }
}
