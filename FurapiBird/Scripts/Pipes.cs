using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    const float despawn_posX = -12f;

    void Update()
    {
        // each 5 points, the speed is slowly increased
        if (FindObjectOfType<GameMaster2>().score%5 == 0 && FindObjectOfType<GameMaster2>().score !=0)
        {
            FindObjectOfType<GameMaster2>().pipeSpeed+=0.005f;
        }
        transform.Translate( -FindObjectOfType<GameMaster2>().pipeSpeed * Time.deltaTime , 0, 0 );

        // when the pipe is outside the screen, we delete it
        if (transform.position.x < despawn_posX)
        {
            Destroy(gameObject);
        }
    }
}
