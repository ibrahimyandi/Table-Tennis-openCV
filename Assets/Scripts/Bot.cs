using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Racket
{
    public Vector3 normalPosition;
    public float servisPositionZ;
    public float servisRotationY;
    private float ratio = 0;

    private float MaxX = 10f;
    private float MaxY = 8f;
    private float MaxZ = 5;
    private float MinX = 6f;
    private float MinY = 5f;
    private float MinZ = -5;

    private void Start()
    {
        FirstPosition();
    }

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);
        newPosition.z = Mathf.Clamp(newPosition.z, MinZ, MaxZ);
        transform.position = newPosition;
    }
    public void FirstPositionRatio()
    {
        ratio = Random.Range(0f, 1f);
    }

    public void FirstPosition()
    {
        servisPositionZ = Mathf.Lerp(-3f, 3f, ratio);
        servisRotationY = Mathf.Lerp(160f, 200f, ratio);

        if (gameObject.name == "Racket_Bot")
        {
            transform.position = Vector3.MoveTowards(transform.localPosition, new Vector3(10, 6, servisPositionZ), speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, servisRotationY, 90);
        }
    }

    public void WaitBall(Vector3 ballTransform)
    {
        float step = speed * Time.deltaTime;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, ballTransform.z);
        transform.position = Vector3.MoveTowards(transform.localPosition, targetPosition, step);
    }

    public void ResetPosition()
    {
        float step = speed * Time.deltaTime;
        Vector3 targetPosition = new Vector3(10, 6, 0);
        Vector3 targetRotation = new Vector3(0, 180, 90);

        transform.position = Vector3.MoveTowards(transform.localPosition, targetPosition, step);
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == ballCollider)
        {
            HitBall(collision);
            gameController.botPaddleCollision = true;
        }
    }
}