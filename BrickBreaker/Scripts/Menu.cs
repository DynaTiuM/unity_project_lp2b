using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // esc to quit
        if (Input.GetKey(KeyCode.Escape)) {
            StartCoroutine(Load("Menu"));
        }
        // s to go to the score
        else if (Input.GetKeyDown(KeyCode.S)) {
             StartCoroutine(Load("StoreScene"));
        }
        // any key to play
        else if (Input.anyKeyDown) {
                StartCoroutine(Load("GameScene"));
        }
    }

    IEnumerator Load(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        while (!asyncLoad.isDone)
        {
            yield return new WaitForSeconds(10);
        }
    }
}
