using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Tooltip("Detection radius around the enemy.")]
    [SerializeField] float detectionRadius = 5.0f;
    [SerializeField] LayerMask detectionLayer; // Layer on which the player is present for efficient searching

    private AIMovement _aiMovement;

    private void Awake()
    {
        _aiMovement = GetComponent<AIMovement>();
    }

    private void Update()
    {
        ScanForPlayer();
    }

    private void ScanForPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player") && CanSeePlayer(hit.transform))
            {
                // blue line means an enemy has been spotted:
                Debug.DrawLine(transform.position, hit.transform.position, Color.blue);

                _aiMovement.enemySpotted = true;
                _aiMovement.targetEnemyPosition = hit.transform.position;
                return;
            }
        }

        // reset detection if the player is not in the sphere:
        //_aiMovement.enemySpotted = false;
        //_aiMovement.targetEnemyPosition = Vector3.zero;
    }

    private bool CanSeePlayer(Transform _heardSomethingOverThere)
    {
        RaycastHit hit;
        Vector3 _rayOrigin = transform.position + Vector3.up * 1.1f;
        Vector3 _directionToTarget = (_heardSomethingOverThere.position - _rayOrigin).normalized;


        if (Physics.Raycast(_rayOrigin, _directionToTarget, out hit, Mathf.Infinity, detectionLayer))
        {
            Debug.DrawLine(transform.position, _heardSomethingOverThere.position, Color.black);

            // check if the raycast hit the player
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Debug.DrawLine(_rayOrigin, _heardSomethingOverThere.position, Color.green);

                return true;
            }else
            {
                Debug.Log("ray hit " + hit.collider.name);
            }
        }

        Debug.DrawLine(_rayOrigin, _heardSomethingOverThere.position, Color.red);
        return false;
    }



    // Use Unity's Gizmos to draw the wireframe in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
