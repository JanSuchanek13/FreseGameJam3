using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("WEAPONS")]
    public GameObject Weapon1;
    public GameObject Weapon2;

    [Header("HANDS")]
    public GameObject Hand1;
    public GameObject Hand2;

    private void Awake()
    {
        SetWeaponInHand();
    }

    private void SetWeaponInHand()
    {
        Weapon1.transform.parent = Hand1.transform;
        Weapon1.transform.position = Hand1.transform.position;
        Weapon1.transform.rotation = Hand1.transform.rotation;

        Weapon2.transform.parent = Hand2.transform;
        Weapon2.transform.position = Hand2.transform.position;
        Weapon2.transform.rotation = Hand2.transform.rotation;
    }
}
