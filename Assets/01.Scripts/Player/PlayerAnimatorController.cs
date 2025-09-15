using System;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Components")]
    private Animator animator;
    private PlayerManager player;

    [Header("Root Motion Settings")] 
    public bool useRootMotion = true;

    [Header("Player State")] 
    public bool isStrafing = false;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        animator = player.animator;
    }
    
    private void OnAnimatorMove()
    {
        if (!useRootMotion) return;
    }

    private void SmoothSetFloat(string parameterName, float targetValue)
    {
        float currentValue = animator.GetFloat(parameterName);
        float lerpedValue = Mathf.Lerp(currentValue, targetValue, 10f * Time.deltaTime);
        animator.SetFloat(parameterName, lerpedValue);
    }

    public void PlayAnimation(string animationName, float duration = 0.2f)
    {
        animator.CrossFadeInFixedTime(animationName, duration);
    }
}