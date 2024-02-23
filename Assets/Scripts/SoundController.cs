using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip soundCollider;

    public void SoundCollider(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(soundCollider, position);
    }
}
