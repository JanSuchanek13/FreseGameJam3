using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimationController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerShooting shooting;

    [SerializeField]
    private Animator animatorBody;
    [SerializeField]
    private Animator animatorL_Arm;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        shooting = GetComponent<PlayerShooting>();
    }

    private void Update()
    {
        BodyAnimation();
    }

    private void BodyAnimation()
    {
        animatorBody.SetFloat("MoveSpeed", movement.moveinput);
        animatorL_Arm.SetBool("Shoot", shooting.shootInput);
    }
}
