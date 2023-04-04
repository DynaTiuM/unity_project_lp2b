using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    public TextMeshPro SCORE;
    public TextMeshPro HIGHSCORE;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Displaying the scores
        SCORE.SetText("Score : " + PlayerPrefs.GetInt("Sauv_score"));
        HIGHSCORE.SetText("Highscore : " + PlayerPrefs.GetInt("Sauv_HIGHSCORE"));

    }
}
