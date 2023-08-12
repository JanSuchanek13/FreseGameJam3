using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIMovement : MonoBehaviour
{
    //private WeaponScriptableObject _weaponData;
    private NavMeshAgent _agent;
    private float _walkDuration;
    private bool _isWalking;
    //private LayerMask _firingLayer;

    public bool inAttackRange = false;
    public bool enemySpotted;
    //public Transform targetEnemy; // Reference to the target enemy's position
    public Vector3 targetEnemyPosition; // Reference to the target enemy's position

    private void Start()
    {
        //_weaponData = GetComponent<AICombat>().weapon.GetComponent<WeaponData>().data;
        //_firingLayer = _weaponData.layerMask;

        _agent = GetComponent<NavMeshAgent>();
        StartRandomWalking();
    }

    private void Update()
    {
        //if (enemySpotted && targetEnemyPosition != null)
        if (enemySpotted)
        {
            if (!inAttackRange)
            {
                MoveIntoAttackRange();
            }else
            {
                StopMoving();
                Attack();
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

    void Attack()
    {
        //attk
    }
    private void MoveIntoAttackRange()
    {
        _agent.SetDestination(targetEnemyPosition);
    }

    /*
    private void MoveIntoAttackRange()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetEnemyPosition);

        switch (_weaponData.type) // Using the 'type' enum from WeaponScriptableObject
        {
            case 0:
                if (distanceToTarget > _weaponData.range || !HasClearLineOfSight())
                {
                    _agent.SetDestination(targetEnemyPosition);
                }else if(distanceToTarget <= _weaponData.range || HasClearLineOfSight())
                {
                    _agent.SetDestination(transform.position);
                }
                break;

            case 1:
                if (distanceToTarget > _weaponData.range || !HasClearLineOfSight())
                {
                    _agent.SetDestination(targetEnemyPosition);
                }
                else if (distanceToTarget <= _weaponData.range || HasClearLineOfSight())
                {
                    _agent.SetDestination(transform.position);
                }
                break;

            case 2:
                if (distanceToTarget > _weaponData.range || !HasIndirectLineOfSight())
                {
                    _agent.SetDestination(targetEnemyPosition);
                }
                else if (distanceToTarget <= _weaponData.range || HasIndirectLineOfSight())
                {
                    _agent.SetDestination(transform.position);
                }
                break;
        }
    }
    
    private bool HasClearLineOfSight()
    {
        RaycastHit hit;

        // check for intervening terrain:
        if (Physics.Raycast(transform.position, (targetEnemyPosition - transform.position).normalized, out hit, _weaponData.range, _firingLayer))
        {
            Debug.Log("cannot see the player directly");
            return false;
        }

        // current weapon can reach/see the target 
        return true;
    }
    private bool HasIndirectLineOfSight()
    {
        RaycastHit hit;

        // check for intervening terrain:
        if (Physics.Raycast(transform.position, (targetEnemyPosition - transform.position).normalized, out hit, _weaponData.range, _firingLayer))
        {
            Debug.Log("cannot see the player INdirectly");
            return false;
        }

        // current weapon can reach/see the target 
        return true;
    }*/

    /*private bool HasClearLineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (targetEnemyPosition - transform.position).normalized, out hit, _weaponData.range, _firingLayer))
        {
            return hit.transform.position == targetEnemyPosition;
        }
        return false;
    }
    private bool HasIndirectLineOfSight()
    {
        RaycastHit hit;
        // here we can simply cast a ray and check if it hits something like a wall or pillar which would be on the layer of "indirect blocking" eg

        if (Physics.Raycast(transform.position, (targetEnemyPosition - transform.position).normalized, out hit, _weaponData.range, _firingLayer))
        {
            return hit.transform.position == targetEnemyPosition;
        }
        return false;
    }*/

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
