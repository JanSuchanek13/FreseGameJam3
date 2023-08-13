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

    //GameObject _geometry;
    GameObject _collider;

    //[Header("Effects relevant to Pagowicz")]
    //[SerializeField] ParticleSystem _healUpParticle;
    //[SerializeField] AudioSource _healUpSound;

    // Animation vars:
    Animator _animatorBody;
    Animator _animatorArms;

    [SerializeField] GameObject _helmet;
    [SerializeField] GameObject _riggedHelmet;
    [SerializeField] float _helmetThrowingForceMultiplier = 3.0f;

    private void OnEnable()
    {
        _animatorBody = transform.Find("Geometry:/G_KatziBody").GetComponent<Animator>();
        _animatorArms = transform.Find("Geometry:/G_KatziArmsShootingWalking").GetComponent<Animator>();

        //_geometry = GameObject.Find("Geometry:");
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

        LoseHelmet();
        _animatorArms.SetBool("isDying", true);
        _animatorBody.SetBool("isDying", true);

        GetComponent<EnemyDetection>().isAlive = false;
        GetComponent<AICombat>().isAlive = false;
        GetComponent<AIMovement>().isAlive = false;
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

    void LoseHelmet()
    {
        _riggedHelmet.SetActive(false);
        _helmet.SetActive(true);
        _helmet.transform.parent = null;
        
        /*
        _helmet.layer = LayerMask.NameToLayer("IgnoreCharacters");
        _helmet.AddComponent<MeshCollider>();
        _helmet.GetComponent<MeshCollider>().convex = true;
        _helmet.AddComponent<Rigidbody>();*/

        _helmet.GetComponent<Rigidbody>().AddForce(-transform.forward * _helmetThrowingForceMultiplier, ForceMode.Impulse);
    }
}
