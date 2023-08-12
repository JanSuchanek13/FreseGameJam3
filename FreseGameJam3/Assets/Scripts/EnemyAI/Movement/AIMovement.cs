using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private float _walkDuration;
    private bool _isWalking;

    public bool canAttack = false;
    public bool enemySpotted;
    public Vector3 targetEnemyPosition;

    private void Start()
    {
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
        }else if (!enemySpotted && !_isWalking)
        {
            StartRandomWalking();
        }
    }

    void StopMoving()
    {
        _agent.SetDestination(transform.position);
    }
    private void MoveIntoAttackRange()
    {
        _agent.SetDestination(targetEnemyPosition);
    }
    private void StartRandomWalking()
    {
        _isWalking = true;
        StartCoroutine(RandomWalking());
    }

    private IEnumerator RandomWalking()
    {
        Vector3 _randomDirection = Random.insideUnitSphere * 20; // Random radius around the enemy
        _randomDirection += transform.position;
        NavMeshHit _hit;
        NavMesh.SamplePosition(_randomDirection, out _hit, 20, 1);
        Vector3 _finalPosition = _hit.position;

        _agent.SetDestination(_finalPosition);

        _walkDuration = Random.Range(5f, 15f); // Random duration between 5 and 15 seconds
        yield return new WaitForSeconds(_walkDuration);

        _isWalking = false;
    }
}
