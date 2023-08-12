using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Tooltip("If this is true, this collectible is a healthpoint.")]
    public bool health = false;

    [Tooltip("0 = Waterpistol, 1 = Pickle Gun, 2 = Bazooka")]
    public int weaponType = 0;
    
    void Start()
    {
        Debug.Log("collectible item initialized.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
    }
}
