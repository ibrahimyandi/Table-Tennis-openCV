using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    public Transform ballTransform;
    public Collider ballCollider;
    public GameController gameController;
    public SoundController soundController;

    private Vector3 lastRacketPosition;
    
    protected float speed = 5.0f;
    protected float hitForce = 17f;
    private void Start()
    {
        lastRacketPosition = transform.position;
    }

    protected void RacketRotation(Vector3 targetRotation)
    {
        Vector3 raketPozisyon = transform.position;
        Vector3 hedefYonu = targetRotation - raketPozisyon;

        float hedefYonuAci = Mathf.Atan2(hedefYonu.x, hedefYonu.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, hedefYonuAci + 90, 90);
    }
    public void FollowTarget(Transform targetPosition)
    {
        float currentRacketSpeed = Vector3.Distance(transform.position, lastRacketPosition) / Time.deltaTime;
        lastRacketPosition = transform.position;
        hitForce = Mathf.Clamp(currentRacketSpeed, 1 * hitForce, 7 * hitForce);
        if (gameObject.name == "Racket_Bot")
        {
            hitForce = 16f;
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.localPosition, targetPosition.localPosition, step);
    }

    protected void HitBall(Collision collision)
    {
        Vector3 hitPoint = collision.contacts[0].point;
        Vector3 racketCenter = collision.gameObject.transform.position;
        Vector3 hitDirection = (hitPoint - racketCenter).normalized;

        soundController.SoundCollider(ballTransform.position);

        ballCollider.GetComponent<Rigidbody>().AddForce(hitDirection * hitForce, ForceMode.Impulse);
    }
}