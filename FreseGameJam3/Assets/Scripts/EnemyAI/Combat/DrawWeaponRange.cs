using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Display the equipped weapons range as a yellow gizmo to study movement behavior.
/// </summary>
public class DrawWeaponRange : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetComponent<AICombat>().weapon.GetComponent<WeaponData>().data.range);
    }
}
