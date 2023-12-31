using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform ballTransform;
    public Collider ballCollider;

    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        float ballX = ballTransform.position.x;
        float ballY = ballTransform.position.y;
        if (ballY > 5 && collision.collider == ballCollider)
        {
            if (ballX > 0.25f && ballX < 7)
            {
                Debug.Log("Top BOT bölümüne düştü.");
            }
            else if (ballX > -7 && ballX < -0.25f)
            {
                Debug.Log("Top PLAYER bölümüne düştü.");
            }    
        }
    }
}
