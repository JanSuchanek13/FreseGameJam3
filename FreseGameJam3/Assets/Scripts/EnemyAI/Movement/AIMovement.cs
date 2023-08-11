using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIMovement : MonoBehaviour
{
    //private WeaponData _weaponData;
    private NavMeshAgent _agent;
    private float _walkDuration;
    private bool _isWalking;

    public bool enemySpotted;
    public Transform targetEnemy; // Reference to the target enemy's position

    public WeaponScriptableObject _weaponData;

    // GO weapon.GetComponten<WeaponData>()
    private void Start()
    {
        _weaponData = GetComponent<AICombat>().weapon.GetComponent<WeaponData>().data;
        _agent = GetComponent<NavMeshAgent>();
        StartRandomWalking();
    }

    private void Update()
    {
        if (enemySpotted && targetEnemy != null)
        {
            MoveIntoAttackRange();
        }
        else if (!_isWalking)
        {
            StartRandomWalking();
        }
    }

    private void MoveIntoAttackRange()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetEnemy.position);
        switch (_weaponData.type) // Using the 'type' enum from WeaponScriptableObject
        {
            case 0:
                if (distanceToTarget > _weaponData.range)
                {
                    _agent.SetDestination(targetEnemy.position);
                }
                break;

            case 1:
                if (distanceToTarget > _weaponData.range || !HasClearLineOfSight())
                {
                    _agent.SetDestination(targetEnemy.position);
                }
                break;

            case 2:
                if (distanceToTarget > _weaponData.range || !HasIndirectLineOfSight())
                {
                    _agent.SetDestination(targetEnemy.position);
                }
                break;
        }
    }

    private bool HasClearLineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (targetEnemy.position - transform.position).normalized, out hit, _weaponData.range))
        {
            return hit.transform == targetEnemy;
        }
        return false;
    }
    private bool HasIndirectLineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (targetEnemy.position - transform.position).normalized, out hit, _weaponData.range))
        {
            return hit.transform == targetEnemy;
        }
        return false;
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
