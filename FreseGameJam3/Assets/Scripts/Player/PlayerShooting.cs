using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("REFERENCE")]
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;
    private WeaponHandler holdingWeapons;


    private void Awake()
    {
        playerInput = new PlayerInput();
        holdingWeapons = GetComponent<WeaponHandler>();
    }

    /// <summary>
    /// enable Player input
    /// </summary>
    private void OnEnable()
    {
        playerInput.Enable();
    }

    /// <summary>
    /// disable Player input
    /// </summary>
    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        if (playerInput.Player.Shoot.triggered)
        {
            Debug.Log(holdingWeapons.Weapon1.GetComponent<WeaponData>().data.damage);
        }
    }

    private void Shoot()
    {

    }
}
