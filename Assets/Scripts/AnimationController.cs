using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator menuAnimator;
    public Animator cameraAnimator;
    public void MenuDownAnimation()
    {
        menuAnimator.Play("MenuDown");
    }

    public void MenuUpAnimation()
    {
        menuAnimator.Play("MenuUp");
    }

    public void CamStartAnimation()
    {
        cameraAnimator.Play("CameraAnimationStart");
        // Invoke("CameraAnimationFinish", cameraAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void CamFinishAnimation()
    {
        cameraAnimator.Play("CameraAnimationFinish");
        // Invoke("CameraAnimationIdle", cameraAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void CamIdleAnimation()
    {
        cameraAnimator.Play("CameraAnimationIdle");
    }
}
