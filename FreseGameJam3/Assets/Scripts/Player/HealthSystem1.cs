using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthSystem : MonoBehaviour
{
    [SerializeField]
    private float lifePoints = 2;
    [SerializeField]
    private Renderer renderer;

    [Header("Effects relevant to all characters")]
    [SerializeField] AudioSource _takeDamageSound;
    [SerializeField] AudioSource _deathSound;
    [SerializeField] ParticleSystem _takeDamageParticle;

    [Header("Effects relevant to Pagowicz")]
    [SerializeField] ParticleSystem _healUpParticle;
    [SerializeField] AudioSource _healUpSound; 


    public void DecreaseLifePoints(float _damage)
    {
        if(lifePoints - _damage > 0)
        {
            lifePoints -= _damage;

            // juice:
            if(_takeDamageSound != null)
            {
                _takeDamageSound.Play();
            }
            if (_healUpParticle != null)
            {
                _takeDamageParticle.Play();
            }

            StartCoroutine("Flashing");

            Debug.Log(lifePoints);
        }else
        {
            Died();        
        }
    }

    public void IncreaseLifePoints(float _lifePoints)
    {
        // juice:
        if (_healUpSound != null)
        {
            _healUpSound.Play();
        }
        if (_healUpParticle != null)
        {
            _healUpParticle.Play();
        }

        lifePoints += _lifePoints;
    }

    private void Died()
    {
        // juice:
        if (_deathSound != null)
        {
            _deathSound.Play();
        }
        if (_takeDamageParticle != null)
        {
            _takeDamageParticle.Play();
        }
        //play Die Animation

        // Destroy(gameObject);
        gameObject.SetActive(false); // this should be a better hotfix solution
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
