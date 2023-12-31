using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameController gameController;
    void Start()
    {
        
    }

    void Update()
    {
        if (transform.position.y < 0)
        {
            gameController.StartRound();
        }
    }
}
