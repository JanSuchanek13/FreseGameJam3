using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Tooltip("Detection radius around the enemy.")]
    [SerializeField] float detectionRadius = 5.0f;
    [SerializeField] LayerMask detectionLayer; // Layer on which the player is present for efficient searching

    private AIMovement _aiMovement;

    // these vars are used to handle AI's interaction with a spotted enemy (player):
    private bool _enemySpotted = false;
    private Transform _spottedEnemyTransform;
    private WeaponScriptableObject _weaponData;
    private LayerMask _firingLayer;

    private void Awake()
    {
        _weaponData = GetComponent<AICombat>().weapon.GetComponent<WeaponData>().data;
        _firingLayer = _weaponData.layerMask;

        _aiMovement = GetComponent<AIMovement>();
    }

    private void Update()
    {
        if (!_enemySpotted)
        {
            ScanForPlayer();
        }else
        {
            // white line = enemy in spotting range and visible:
            Debug.DrawLine((transform.position + Vector3.up * 1.2f), _spottedEnemyTransform.position, Color.white);

            CheckForLineOfAttack();
        }
    }

    private void ScanForPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player") && CanSeePlayer(hit.transform))
            {
                // store the enemy that has been spotted & never forget:
                _enemySpotted = true;
                _spottedEnemyTransform = hit.transform;

                // tell the movement script that an enemy has been spotted and hand over position:
                _aiMovement.enemySpotted = true;
                _aiMovement.targetEnemyPosition = hit.transform.position;
                return;
            }
        }
    }
    private bool CanSeePlayer(Transform _heardSomethingOverThere)
    {
        RaycastHit hit;
        Vector3 _rayOrigin = transform.position + Vector3.up * 1.1f;
        Vector3 _directionToTarget = (_heardSomethingOverThere.position - _rayOrigin).normalized;

        if (Physics.Raycast(_rayOrigin, _directionToTarget, out hit, Mathf.Infinity, detectionLayer))
        {
            // check if a raycast can see/hit the player:
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }

        // black line = enemy in spotting range, but not seen:
        Debug.DrawLine((transform.position + Vector3.up * 1.2f), _heardSomethingOverThere.position, Color.black);
        return false;
    }

    /// <summary>
    /// Once spotted stop checking if an enemy is detected, but instead check if a line of attack can be established.
    /// If not, move into range/LOS.
    /// </summary>
    void CheckForLineOfAttack()
    {
        if (HasLineOfAttack() && IsInRange()) 
        {
            Debug.DrawLine(transform.position, _spottedEnemyTransform.position, Color.green);
            _aiMovement.canAttack = true;
            GetComponent<AICombat>().inRange = true;

            transform.LookAt(_spottedEnemyTransform, Vector3.up);
        }else
        {
            Debug.DrawLine(transform.position, _spottedEnemyTransform.position, Color.red);
            _aiMovement.canAttack = false;
            GetComponent<AICombat>().inRange = false;

            _aiMovement.targetEnemyPosition = _spottedEnemyTransform.position;
        }
    }

    private bool HasLineOfAttack()
    {
        RaycastHit hit;

        // check for intervening terrain:
        if (Physics.Raycast(transform.position, (_spottedEnemyTransform.position - transform.position).normalized, out hit, _weaponData.range, _firingLayer))
        {
            return false;
        }

        return true;
    }
    private bool IsInRange()
    {
        float _distanceToEnemy = Vector3.Distance(transform.position, _spottedEnemyTransform.position);

        if (_distanceToEnemy <= _weaponData.range)
        {
            Debug.DrawLine(transform.position, _spottedEnemyTransform.position, Color.green);
            return true;
        }

        return false;
    }

    // reset detection if the player is not in the sphere:
    void LostEnemyContact()
    {
        //_aiMovement.enemySpotted = false;
        //_aiMovement.targetEnemyPosition = Vector3.zero;
    }

    // Use Unity's Gizmos to draw the wireframe in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
