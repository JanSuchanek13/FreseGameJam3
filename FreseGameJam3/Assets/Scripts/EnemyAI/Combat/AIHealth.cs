using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    [Header("AI Health Settings:")]
    [SerializeField] float _lifePoints = 3;
    [SerializeField] Renderer _renderer;

    [Header("Juice Effects")]
    [SerializeField] AudioSource _takeDamageSound;
    [SerializeField] AudioSource _deathSound;
    [SerializeField] ParticleSystem _takeDamageParticle;

    GameObject _geometry;
    GameObject _collider;

    //[Header("Effects relevant to Pagowicz")]
    //[SerializeField] ParticleSystem _healUpParticle;
    //[SerializeField] AudioSource _healUpSound;

    // Animation vars:
    Animator _animatorBody;
    Animator _animatorArms;

    private void OnEnable()
    {
        _animatorBody = transform.Find("G_KatziBody").GetComponent<Animator>();
        _animatorArms = transform.Find("G_KatziArmsShootingWalking").GetComponent<Animator>();

        _geometry = GameObject.Find("Geometry:");
        _collider = GameObject.Find("Collider:");
    }
    public void DecreaseLifePoints(float _damage)
    {
        if (_lifePoints - _damage > 0)
        {
            _lifePoints -= _damage;

            // juice:
            if (_takeDamageSound != null)
            {
                _takeDamageSound.Play();
            }
            if (_takeDamageParticle != null)
            {
                _takeDamageParticle.Play();
            }

            StartCoroutine("Flashing");

            Debug.Log(gameObject.name + " has: " + _lifePoints + " remaining.");
        }else
        {
            Die();
        }
    }

    // so far irrelevant for AI:
    /*
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
        _lifePoints += _lifePoints;
    }*/

    private void Die()
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
        //gameObject.SetActive(false); // this should be a better hotfix solution
        //_geometry

        _animatorArms.SetBool("isDying", true);
        _animatorBody.SetBool("isDying", true);
        _collider.SetActive(false);
    }

    IEnumerator Flashing()
    {
        for (int i = 0; i < 3; i++)
        {
            _renderer.enabled = false;
            yield return new WaitForSeconds(0.1f);

            _renderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
