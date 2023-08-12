using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimationController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerShooting shooting;
    private WeaponHandler weapon;
    private Animator animatorRWeapon;
    private Animator animatorLWeapon;

    [SerializeField]
    private Animator animatorBody;
    [SerializeField]
    private Animator animatorL_Arm;
    [SerializeField]
    private Animator animatorR_Arm;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        shooting = GetComponent<PlayerShooting>();
        weapon = GetComponent<WeaponHandler>();
    }

    private void Update()
    {
        animatorRWeapon = weapon.Weapon1.transform.GetChild(0).GetComponent<Animator>();
        animatorLWeapon = weapon.Weapon2.transform.GetChild(0).GetComponent<Animator>();
        
        Animation();
    }

    private void Animation()
    {
        animatorBody.SetFloat("MoveSpeed", movement.moveinput);
        animatorL_Arm.SetBool("Shoot", shooting.shootInputL);
        animatorR_Arm.SetBool("Shoot", shooting.shootInputR);
        animatorRWeapon.SetBool("Shoot", shooting.shootInputR);
        animatorLWeapon.SetBool("Shoot", shooting.shootInputL);
    }
}
