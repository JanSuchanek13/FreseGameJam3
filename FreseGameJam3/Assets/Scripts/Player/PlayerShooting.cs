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
        ShootingInput(weapon1Data, playerInput.Player.Shoot1);
        ShootingInput(weapon2Data, playerInput.Player.Shoot2);
    }

    public void UpdateWeaponData()
    {
        weapon1Data = holdingWeapons.Weapon1.GetComponent<WeaponData>().data;
        weapon2Data = holdingWeapons.Weapon2.GetComponent<WeaponData>().data;
    }

    private void ShootingInput(WeaponScriptableObject _weaponData, UnityEngine.InputSystem.InputAction _shootButton)
    {
        switch (_weaponData.fireMode)
        {
            case 0:
                if (_shootButton.triggered)
                {
                    if(Time.time > nextShot)
                    {
                        nextShot = Time.time + _weaponData.attackSpeed;
                        Debug.Log(_weaponData.damage);
                        Shoot(_weaponData);
                    }
                    
                }
                break;

        }
    }

    private void Shoot(WeaponScriptableObject _weaponData)
    {
        //play Sound
        //play Effect
        //play Animation
        switch (_weaponData.type)
        {
            case 0:
                break;

            case 1:

                InstantiateBullet(_weaponData);
                break;

        }
        
    }

    private void InstantiateBullet(WeaponScriptableObject _weaponData)
    {
        GameObject bullet = Instantiate(_weaponData.BulletPrefab, transform.position, transform.rotation);
        Debug.Log(transform.position);
        bullet.GetComponent<Bullet>().direction = transform.position + transform.forward * _weaponData.range;
        bullet.GetComponent<Bullet>().weaponData = _weaponData;
    }
}
