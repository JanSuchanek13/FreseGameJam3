using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private float lifePoints = 2;

    public void DecreaseLifePoints(float _damage)
    {
        if(lifePoints - _damage > 0)
        {
            //play damage Sound
            lifePoints -= _damage;
            Debug.Log(lifePoints);
        }
        else
        {
            //play Death Sound
        }
    }

    
}
