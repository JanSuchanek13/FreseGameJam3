using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private Tween moveTween; // Referenz auf das Tween-Objekt f�r die Bewegung

    [Header("DATA")]
    [Tooltip("Fly Direction")]
    public Vector3 direction;
    [Tooltip("Data of the Weapon")]
    public WeaponScriptableObject weaponData;
    [Tooltip("Data of the Weapon")]
    public bool playerBullet;

    //keinAwake benutzen weil das vor Instantiate gecalled wird;

    private void Start()
    {
        if(weaponData.layerMask == LayerMask.GetMask("IndirectBullet"))
        {
            gameObject.layer = LayerMask.NameToLayer("IndirectBullet");
        }

        switch (weaponData.type)
        {
            case 0:
                break;

            case 1:
                moveTween = transform.DOMove(direction, weaponData.bulletSpeed)
                    .SetEase(Ease.Linear) // Verwende Linear-Easing f�r konstante Geschwindigkeit
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
            //Debug.Log("hit");
            other.transform.parent.GetComponent<HealthSystem>().DecreaseLifePoints(weaponData.damage);
            Destroy();
        }
        if (other.CompareTag("Enemy") && playerBullet)
        {
            //Debug.Log("hit");
            other.transform.parent.transform.parent.GetComponent<HealthSystem>().DecreaseLifePoints(weaponData.damage);
            Destroy();
        }
        if (other.CompareTag("Boundaries"))
        {
            //Debug.Log("hit");
            Destroy();
        }
    }

    private void Destroy()
    {
        moveTween.Kill();
        Destroy(gameObject);
    }
}
