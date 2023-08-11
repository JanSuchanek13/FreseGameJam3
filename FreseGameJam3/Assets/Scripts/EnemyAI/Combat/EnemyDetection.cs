using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Tooltip("Detection radius around the enemy.")]
    [SerializeField] float detectionRadius = 5.0f;
    [SerializeField] LayerMask detectionLayer; // Layer on which the player is present for efficient searching
    [SerializeField] bool showDetectionRadiusInEditor = true; // Boolean to toggle wireframe drawing in the editor

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
            Debug.Log("this is true 1");

            //if (hit.CompareTag("Player") && CanSeePlayer(hit.transform))
            if (hit.CompareTag("Player"))
            {
                Debug.Log("this is true 2");

                if (CanSeePlayer(hit.transform))
                {
                    Debug.Log("this is true 3");
                    _aiMovement.enemySpotted = true;
                    _aiMovement.targetEnemy = hit.transform;
                    return;
                }
            }
        }

        // Optionally, reset detection if the player is not in the sphere
        // _aiMovement.enemySpotted = false;
        // _aiMovement.targetEnemy = null;
    }

    private bool CanSeePlayer(Transform _heardSomethingOverThere)
    {
        RaycastHit hit;
        Vector3 _rayOrigin = transform.position + Vector3.up * 1.1f; // Adjust the 1.5f value as needed
        Vector3 _directionToTarget = (_heardSomethingOverThere.position - transform.position).normalized;
        if (Physics.Raycast(_rayOrigin, _directionToTarget, out hit, detectionRadius, detectionLayer))
        {
            Debug.Log("this is true 4");

            // Check if the raycast hit the player
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Debug.Log("this is true 6");

                Debug.DrawLine(transform.position, _heardSomethingOverThere.position, Color.green); // Draw a green line

                return true;
            }
            else
            {
                Debug.Log("ray hit " + hit.collider.name);
            }
        }
        Debug.DrawLine(transform.position, _heardSomethingOverThere.position, Color.red); // Draw a red line

        return false;
    }

    // Use Unity's Gizmos to draw the wireframe in the editor
    private void OnDrawGizmos()
    {
        if (showDetectionRadiusInEditor)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
