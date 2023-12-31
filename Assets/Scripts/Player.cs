using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameController gameController;
    public Transform ballTransform;
    public Collider ballCollider;
    public float racketSpeed = 1f;
    public float hitDistance = 1f;
    private float hitForce = 1f;

    private Vector3 lastRacketPosition;

    private void Start()
    {
        lastRacketPosition = transform.position;
    }

    private void Update()
    {
        GetComponent<Racket>().RacketRotation(GetComponent<Racket>());
    }

    public void PlayerUpdate(float x, float y, float recWidth, float recHeight, float camWidth, float camHeight)
    {
        Vector2 startPoint = new Vector2(x, y);
        Vector2 endPoint = new Vector2(x + recWidth, y + recHeight);

        float recDistance = Vector3.Distance(startPoint, endPoint);
        float camDistance = Vector3.Distance(new Vector2(0, 0), new Vector2(camWidth, camHeight));

        Vector3 racketPosition = new Vector3();
        float ratio = recDistance / camDistance;

        racketPosition.x = (10 - (10 * ratio)) * -1;
        racketPosition.y = Mathf.Clamp(10 - ((y + recWidth / 2) / camWidth) * 10, 5, 10);
        racketPosition.z = Mathf.Clamp(5 - ((x + recHeight / 2) / camHeight) * 10, -5, 5);
        if (racketPosition.x > ballTransform.position.x)
        {
            racketPosition.x = ballTransform.position.x;
        }

        float currentRacketSpeed = Vector3.Distance(transform.position, lastRacketPosition) / Time.deltaTime;
        lastRacketPosition = transform.position;

        GetComponent<Racket>().Move(racketPosition);

        hitForce = Mathf.Clamp(currentRacketSpeed, 1, 5);
    }

    void HitBall()
    {
        ballTransform.GetComponent<Rigidbody>().AddForce(Vector3.right * hitForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == ballCollider)
        {
            gameController.playerPaddleCollision = true;
            HitBall();
        }
    }
}
