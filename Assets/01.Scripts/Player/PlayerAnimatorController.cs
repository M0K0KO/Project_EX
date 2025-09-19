using System;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Components")] private Animator animator;
    private PlayerManager player;

    [Header("Root Motion Settings")] public bool useRootMotion = true;


    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        animator = player.animator;
    }

    private void OnAnimatorMove()
    {
        if (!useRootMotion) return;

        Vector3 newPosition = transform.position + animator.deltaPosition;
        player.rb.MovePosition(newPosition);
    }

    private void Update()
    {
        UpdateMoveInputParam();
        UpdateSprintInputParam();
    }

    private void UpdateMoveInputParam()
    {
        bool moveInput = PlayerInputManager.instance.moveInput != Vector2.zero;
        animator.SetBool("MoveInput", moveInput);
    }

    private void UpdateSprintInputParam()
    {
        bool sprintInput = PlayerInputManager.instance.sprintInput;
        animator.SetBool("SprintInput", sprintInput);
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
    
    public string GetJumpAnimation()
    {
        string animationToPlay;
        
        if (PlayerInputManager.instance.moveInput != Vector2.zero)
        {
            animationToPlay = "Player_Jump_Start_F_01";
        }
        else
        {
            animationToPlay = "Player_Jump_Start_01";
        }

        return animationToPlay;
    }
}