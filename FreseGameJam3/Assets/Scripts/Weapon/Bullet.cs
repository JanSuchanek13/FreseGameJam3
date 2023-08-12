using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private Tween moveTween; // Referenz auf das Tween-Objekt für die Bewegung

    [Header("DATA")]
    [Tooltip("Fly Direction")]
    public Vector3 direction;
    [Tooltip("Data of the Weapon")]
    public WeaponScriptableObject weaponData;
    [Tooltip("Data of the Weapon")]
    public bool playerBullet;

    private void Start()
    {
        switch (weaponData.type)
        {
            case 0:
                break;

            case 1:
                moveTween = transform.DOMove(direction, weaponData.bulletSpeed)
                    .SetEase(Ease.Linear) // Verwende Linear-Easing für konstante Geschwindigkeit
                    .OnComplete(() => Destroy(gameObject));
                break;

            case 2:
                transform.DOJump(direction, 2, 1, weaponData.bulletSpeed )
                    .OnComplete(() => Destroy(gameObject));

                break;

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerBullet)
        {
            Debug.Log("hit");
            other.GetComponent<HealthSystem>().DecreaseLifePoints(weaponData.damage);
            Destroy();
        }
        if (other.CompareTag("Enemy") && playerBullet)
        {
            Debug.Log("hit");
            other.GetComponent<HealthSystem>().DecreaseLifePoints(weaponData.damage);
            Destroy();
        }
    }

    private void Destroy()
    {
        moveTween.Kill();
        Destroy(gameObject);
    }
}
