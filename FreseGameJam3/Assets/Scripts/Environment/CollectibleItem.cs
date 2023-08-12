using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Tooltip("If this is true, this collectible is a healthpoint.")]
    public bool health = false;
    [Tooltip("Amount of LifePoints to gain")]
    public int lifePoints = 1;

    [Tooltip("")]
    public WeaponScriptableObject weaponData;
    
    void Start()
    {
        Debug.Log("collectible item initialized.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (health)
            {
                other.transform.parent.GetComponent<HealthSystem>().IncreaseLifePoints(lifePoints);
                Destroy(gameObject);
            }
            if(weaponData != null)
            {
                other.transform.parent.GetComponent<WeaponHandler>();
                Destroy(gameObject);
            }
        }
    }
}
