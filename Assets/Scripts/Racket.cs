using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    private float speed = 50f;
    public Transform ball;

    void Update()
    {
   
    }
    public void RacketRotation(Racket racket)
    {
        Vector3 raketPozisyon = transform.position;
        Vector3 topPozisyon;
        if (racket.name == "Racket_Bot")
        {
            topPozisyon = new Vector3(-7, 5, 0);
        }
        else
        {
            topPozisyon = new Vector3(7, 5, 0);
        }

        Vector3 hedefYonu = topPozisyon - raketPozisyon;

        float hedefYonuAci = Mathf.Atan2(hedefYonu.x, hedefYonu.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, hedefYonuAci + 90, 90);
    }
    public void Move(Vector3 targetPosition)
    {
        Vector3 racketPosition = transform.position;
        transform.position = Vector3.MoveTowards(racketPosition, targetPosition, speed * Time.deltaTime);
    }
    public void ResetPosition()
    {
        transform.position = new Vector3(10, 6, 0);
        transform.rotation = Quaternion.Euler(0, 180, 90);
    }
}