using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public GameController gameController;
    public Transform ballTransform;
    public Collider ballCollider;
    public float hitDistance = 1f;
    private float hitForce = 1f;
    private float speed = 5.0f;
    public void FollowBall()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.localPosition, ballTransform.localPosition, step);

        Vector3 closestPointEnemy = GetComponent<BoxCollider>().ClosestPointOnBounds(ballTransform.position);
        float enemydistance = Vector3.Distance(closestPointEnemy, ballTransform.position);

        if (enemydistance < 0.2)
        {

            Vector3 fromPaddleToBall = ballTransform.position - transform.position;
            if (Vector3.Dot(fromPaddleToBall, transform.up) > 0)
            {
            ballCollider.GetComponent<Rigidbody>().velocity = transform.up * 3 + transform.forward * 3;
            }
            else
            {
                ballCollider.GetComponent<Rigidbody>().velocity = - transform.up * 3 + transform.forward * 2;

            }

            gameController.botPaddleCollision = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == ballCollider)
        {
            HitBall();
        }
    }

    void HitBall()
    {
        ballTransform.GetComponent<Rigidbody>().AddForce(Vector3.right * hitForce, ForceMode.Impulse);
    }
}
