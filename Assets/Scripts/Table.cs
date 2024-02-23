using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public SoundController soundController;
    public Transform ballTransform;
    public Collider ballCollider;
    public int playerAreaBounce = 0;
    public int botAreaBounce = 0;
    
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        float ballX = ballTransform.position.x;
        float ballY = ballTransform.position.y;
        if (ballY > 5 && collision.collider == ballCollider)
        {
            soundController.SoundCollider(ballTransform.position);
            if (ballX > 0.25f && ballX < 7)
            {
                botAreaBounce++;
                // Debug.Log("Top BOT bölümüne düştü. " + botAreaBounce);
            }
            else if (ballX > -7 && ballX < -0.25f)
            {
                playerAreaBounce++;
                // Debug.Log("Top PLAYER bölümüne düştü." + playerAreaBounce);
            }    
        }
    }

    public void ResetBounce(){
        playerAreaBounce = 0;
        botAreaBounce = 0;
    }
}
