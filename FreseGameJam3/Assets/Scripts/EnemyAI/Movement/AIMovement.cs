using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private float _walkDuration;
    private bool _hasDestination;
    private bool _isMoving;

    public bool canAttack = false;
    public bool enemySpotted;
    public Vector3 targetEnemyPosition;

    Vector3 _destination;

    // Animation vars:
    Animator _animator;

    private void OnEnable()
    {
        _animator = transform.Find("G_KatziBody").GetComponent<Animator>();

        _agent = GetComponent<NavMeshAgent>();
        StartRandomWalking();
    }

    private void Update()
    {
        if (enemySpotted)
        {
            if (!canAttack) // cannot attack
            {
                MoveIntoAttackRange();
            }else // can attack
            {
                StopMoving();
            }
        }else if (!enemySpotted && !_hasDestination)
        {
            StartRandomWalking();
        }

        CheckForMovement();
    }

    void StopMoving()
    {
        _destination = transform.position;
        _agent.SetDestination(_destination);
        //_agent.SetDestination(transform.position);
    }
    private void MoveIntoAttackRange()
    {
        _destination = targetEnemyPosition;
        _agent.SetDestination(_destination);
        //_agent.SetDestination(targetEnemyPosition);
    }
    private void StartRandomWalking()
    {
        _hasDestination = true;
        StartCoroutine(RandomWalking());
    }

    void CheckForMovement()
    {
        if(transform.position != _destination)
        {
            _isMoving = true;
            _animator.SetBool("isWalking", _isMoving);
            
            return;
        }

        _isMoving = false;
        _animator.SetBool("isWalking", _isMoving);
    }

    private IEnumerator RandomWalking()
    {
        Vector3 _randomDirection = Random.insideUnitSphere * 20; // Random radius around the enemy
        _randomDirection += transform.position;
        NavMeshHit _hit;
        NavMesh.SamplePosition(_randomDirection, out _hit, 20, 1);
        //Vector3 _finalPosition = _hit.position;
        //_agent.SetDestination(_finalPosition);

        _destination = _hit.position;
        _agent.SetDestination(_destination);

        _walkDuration = Random.Range(5f, 15f); // Random duration between 5 and 15 seconds
        yield return new WaitForSeconds(_walkDuration);

        _hasDestination = false;
    }
}
