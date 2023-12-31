using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public Collider ball;
    public GameController gameController;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == ball)
        {
            gameController.StartRound();
        }
    }
}
