using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Racket
{
    public void PlayerUpdate(float x, float y, float recWidth, float recHeight, float camWidth, float camHeight)
    {
        Vector2 startPoint = new Vector2(x, y);
        Vector2 endPoint = new Vector2(x + recWidth, y + recHeight);

        float recDistance = Vector3.Distance(startPoint, endPoint);
        float camDistance = Vector3.Distance(new Vector2(0, 0), new Vector2(camWidth, camHeight));

        Vector3 racketPosition = new Vector3();
        float ratio = recDistance / camDistance;

        racketPosition.x = (10 - (10 * ratio)) * -1;
        racketPosition.y = Mathf.Clamp(10 - ((y + recHeight / 2) / camHeight) * 10, 5, 8);
        racketPosition.z = Mathf.Clamp(5 - ((x + recWidth / 2) / camWidth) * 10, -5, 5);
        if (racketPosition.x > ballTransform.position.x)
        {
            racketPosition.x = ballTransform.position.x;
        }

        transform.position = Vector3.MoveTowards(transform.localPosition, racketPosition, Time.deltaTime * speed * 10);
        RacketRotation(new Vector3(ballTransform.position.x+5, ballTransform.position.y, ballTransform.position.z));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == ballCollider)
        {
            gameController.playerPaddleCollision = true;
            HitBall(collision);
        }
    }
}
