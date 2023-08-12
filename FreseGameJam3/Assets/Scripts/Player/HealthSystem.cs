using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private float lifePoints = 2;
    [SerializeField]
    private Renderer renderer;

    public void DecreaseLifePoints(float _damage)
    {
        if(lifePoints - _damage > 0)
        {
            //play damage Sound
            lifePoints -= _damage;
            Debug.Log(lifePoints);
            StartCoroutine("Flashing");
            
        }
        else
        {
            Died();        
        }
    }

    public void IncreaseLifePoints(float _lifePoints)
    {
        lifePoints += _lifePoints;
    }

    private void Died()
    {
        //play Death Sound
        //play Die Animation
        Destroy(gameObject);
    }

    IEnumerator Flashing()
    {
        for (int i = 0; i < 3; i++)
        {
            renderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            renderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
