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
            if (hit.CompareTag("Player"))
            {
                _aiMovement.enemySpotted = true;
                _aiMovement.targetEnemy = hit.transform;
                return;
            }
        }

        // Optionally, reset detection if the player is not in the sphere
        // _aiMovement.enemySpotted = false;
        // _aiMovement.targetEnemy = null;
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
