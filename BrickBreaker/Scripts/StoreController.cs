using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StoreController : MonoBehaviour
{
    public TextMeshPro HIGHSCORE;
    public TextMeshPro coins_text;
    public TextMeshPro bought_text_1, bought_text_2, bought_text_3, bought_text_4, bought_text_5, bought_text_6;
    private GameObject cantBuy_prefab;
    private GameObject price_prefab;
    private int price;
    private int[] priceTable = new int[6];

    private List<TextMeshPro> tmp = new List<TextMeshPro>();
    private GameObject[] table = new GameObject[6];

    private AudioSource audioS;

    private float time = 4f;
    private bool displayedCantBuy;
    private GameObject cantBuy_text;
    private GameObject price_text;

    private string ball_name;
    // Start is called before the first frame update
    void Start()
    {

        audioS = GetComponent<AudioSource>();
        cantBuy_prefab = Resources.Load<GameObject>("CantBuy");
        price_prefab = Resources.Load<GameObject>("Price");
       
       // We add our text to a list
        tmp.Add(bought_text_1);
        tmp.Add(bought_text_2);
        tmp.Add(bought_text_3);
        tmp.Add(bought_text_4);
        tmp.Add(bought_text_5);
        tmp.Add(bought_text_6);

        // We put the different prices into an array
        priceTable[0] = 20;
        priceTable[1] = 40;
        priceTable[2] = 100;
        priceTable[3] = 200;
        priceTable[4] = 160;
        priceTable[5] = 120;
    }

    public void setBallName(string name){
        this.ball_name = name;
    }

    // Update is called once per frame
    void Update()
    {
        if(displayedCantBuy) time -= Time.deltaTime;

        // This condition leads to displaying if the player can't buy a skin. The timer is set to 4 seconds
        if(time <= 0 && displayedCantBuy){
            displayedCantBuy = false;
            time = 4f;
            // After 4 seconds, we destroy this text
            Destroy(cantBuy_text);
        }
        HIGHSCORE.SetText("Your current Highscore : " + PlayerPrefs.GetInt("Sauv_HIGHSCORE"));
        coins_text.SetText("Coins : " + PlayerPrefs.GetInt("Sauv_coin"));

        // For every text in the list :
        foreach(TextMeshPro t in tmp){
            // We put the name of the skin into a variable named temp
            string temp = "Sauv_bought_" + (tmp.IndexOf(t) + 1);

            // If this condition is true, it means that the skin is already bought :
            if(PlayerPrefs.GetInt(temp) == 1) {
                // If the skin that we have currently is equal to this one, we put is as selected on the game
                if(PlayerPrefs.GetInt("Sauv_skin") == tmp.IndexOf(t) + 1) t.SetText("Selected");
                // Otherwise, we display the "Select" text
                else t.SetText("Select");

                Destroy(table[tmp.IndexOf(t)]);

                }
            // Else, it means that the skin isn't bought    
            else if(table[tmp.IndexOf(t)] == null){
                // We instantiate a prefab that will display the price of the skin
                table[tmp.IndexOf(t)] = Instantiate(price_prefab);
                // For its position, we take the position of the skin's button but we add an offset of -0,67 on y 
                table[tmp.IndexOf(t)].transform.position = new Vector3((float)(t.transform.position.x),(float)(t.transform.position.y) - 0.67f,0);
                
                // Finally, we take the price of the skin and we modify the text of this text
                price = priceTable[tmp.IndexOf(t)];
                table[tmp.IndexOf(t)].GetComponent<TextMeshPro>().SetText("Price : " + price);
            }
        }
    }

    public void Menu()
    {
        StartCoroutine(LoadGame());
        IEnumerator LoadGame()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }

    public void Buy()
    {
        int i = 0;
        switch(ball_name){
                case "Sauv_bought_1" : 
                    i = 1;
                break;
                case "Sauv_bought_2" : 
                    i = 2;
                break;
                case "Sauv_bought_3" :
                    i = 3;
                break;
                case "Sauv_bought_4" :
                    i = 4;
                break;
                case "Sauv_bought_5" :
                    i = 5;
                break;
                case "Sauv_bought_6" :
                    i = 6;
                break;
            }
        // Depending on the name of the skin, we take the price thanks to the i variable and the priceTable array that was initialized at the start of the script.    
        price = priceTable[i-1];
        // Is the skin already bought ?
        if (PlayerPrefs.GetInt(ball_name) == 1)
        {
            //Yes : we want to select it
            PlayerPrefs.SetInt("Sauv_skin", i);
            audioS.Play();
        }
        else
        {
            // No : can we buy it ?
            if (PlayerPrefs.GetInt("Sauv_coin") > price)
            {
                // Yes : the new selected skin is this one
                audioS.Play();
                PlayerPrefs.SetInt("Sauv_skin", i);
                PlayerPrefs.SetInt("Sauv_coin", PlayerPrefs.GetInt("Sauv_coin") - price);
                PlayerPrefs.SetInt(ball_name, 1);
            }
            else{
                // No : we display a text to the player
                cantBuy_text = Instantiate(cantBuy_prefab);
                displayedCantBuy = true;
            }
        }

    }




}
