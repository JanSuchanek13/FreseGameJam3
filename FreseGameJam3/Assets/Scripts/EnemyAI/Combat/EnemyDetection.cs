using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Tooltip("Detection radius around the enemy.")]
    [SerializeField] float detectionRadius = 5.0f;
    [SerializeField] LayerMask detectionLayer; // Layer on which the player is present for efficient searching
    [SerializeField] AudioSource _miauSound;

    private AIMovement _aiMovement;

    // these vars are used to handle AI's interaction with a spotted enemy (player):
    private bool _enemySpotted = false;
    private bool _screamingMiau = false;
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

            if (!_screamingMiau && _miauSound != null)
            {
                StartCoroutine("ScreamMiauAndScareThePlayer");
            }
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
        float _bulletRadius;

        Vector3 directionToEnemy = (_spottedEnemyTransform.position - transform.position).normalized;

        // Get the bullet size (assuming the bullet has a SphereCollider, you can get its radius)
        if(_weaponData.BulletPrefab != null)
        {
            _bulletRadius = _weaponData.BulletPrefab.GetComponent<SphereCollider>().radius;
        }else // Close combat (no bullet):
        {
            _bulletRadius = 0.2f;
        }


        // SphereCast from current position in the direction of the enemy
        if (Physics.SphereCast(transform.position, _bulletRadius, directionToEnemy, out hit, _weaponData.range, _firingLayer))
        {
            // If the SphereCast hits something other than the enemy
            if (hit.transform != _spottedEnemyTransform)
            {
                // Intervening terrain or obstacle was hit
                return false;
            }
        }

        // No intervening terrain in the path up to the given range
        return true;
    }
    /*
    private bool HasLineOfAttack()
    {
        RaycastHit hit;

        // check for intervening terrain:
        if (Physics.Raycast(transform.position, (_spottedEnemyTransform.position - transform.position).normalized, out hit, _weaponData.range, _firingLayer))
        {
            return false;
        }

        return true;
    }*/
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

    IEnumerator ScreamMiauAndScareThePlayer()
    {
        _screamingMiau = true;
        _miauSound.Play();

        float _rngWaittime = Random.Range(3.0f, 15.0f);
        
        yield return new WaitForSeconds(_rngWaittime);
        _screamingMiau = false;
    }

    // Use Unity's Gizmos to draw the wireframe in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
