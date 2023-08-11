using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("REFERENCE")]
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;
    private WeaponHandler holdingWeapons;
    private WeaponScriptableObject weapon1Data;
    private WeaponScriptableObject weapon2Data;

    private float nextShot;

    private void Awake()
    {
        playerInput = new PlayerInput();
        holdingWeapons = GetComponent<WeaponHandler>();
        UpdateWeaponData();
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
        UpdateWeaponData();//das später in WeaponData aufrüfen sobald eine Waffe wechselt
        ShootingManager();
        
    }

    public void UpdateWeaponData()
    {
        weapon1Data = holdingWeapons.Weapon1.GetComponent<WeaponData>().data;
        weapon2Data = holdingWeapons.Weapon2.GetComponent<WeaponData>().data;
    }

    private void ShootingManager()
    {
        switch (weapon1Data.fireMode)
        {
            case 0:
                if (playerInput.Player.Shoot.triggered)
                {
                    if(Time.time > nextShot)
                    {
                        nextShot = Time.time + weapon1Data.firingRate;
                        Debug.Log(holdingWeapons.Weapon1.GetComponent<WeaponData>().data.damage);
                    }
                    
                }
                break;

        }
    }
}
