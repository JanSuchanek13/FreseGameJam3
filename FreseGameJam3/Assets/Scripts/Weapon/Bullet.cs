using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [Header("DATA")]
    [Tooltip("Fly Direction")]
    public Vector3 direction;
    [Tooltip("Data of the Weapon")]
    public WeaponScriptableObject weaponData;

    private void Start()
    {
        
        transform.DOMove(direction, 2)
            .SetEase(Ease.Linear) // Verwende Linear-Easing für konstante Geschwindigkeit
            .OnComplete(() => Destroy(gameObject));
    }

    private void Destroy()
    {
        this.Destroy();
    }
}
