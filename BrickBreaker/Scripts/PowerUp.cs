using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public float speed;

    // Update is called once per frame
    void Update()
    {
        // Giving speed at the power up object
        transform.Translate(new Vector2(0f, -1f)*Time.deltaTime * speed);
        // If the position of the power up is less than -5, we destroy it to avoid overloading of memory
        if(transform.position.y <= -5) {
            Destroy(gameObject);
        }
    }
}
