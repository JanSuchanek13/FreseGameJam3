using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombat : MonoBehaviour
{
    public GameObject weapon; // accessed by the EnemyDetaction-script!
    public bool inRange = false; // accessed by the EnemyDetaction-script!

    private bool _attacking = false;
    private WeaponScriptableObject _weaponData;

    
    private void OnEnable()
    {  
        // check which weapon is being carried:
        _weaponData = weapon.GetComponent<WeaponData>().data;
    }

    private void Update()
    {
        if(inRange && !_attacking)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        _attacking = true;
        
        // create bullet if applicable:
        if(_weaponData.type != 0)
        {
            FireBullet(_weaponData);
        }else
        {
            Strike(_weaponData);
        }

        yield return new WaitForSeconds(_weaponData.attackSpeed);

        // if still in range, attack again:
        _attacking = false; 
    }

    private void FireBullet(WeaponScriptableObject _weaponData)
    {
        // I bet this would be better, firing from the muzzle:
        //GameObject _bullet = Instantiate(_weaponData.BulletPrefab, _weaponData.muzzlePoint, transform.rotation);
        //_bullet.GetComponent<Bullet>().direction = _weaponData.muzzlePoint + transform.forward * _weaponData.range;

        GameObject _bullet = Instantiate(_weaponData.BulletPrefab, transform.position, transform.rotation);
        _bullet.GetComponent<Bullet>().direction = transform.position + transform.forward * _weaponData.range;
        _bullet.GetComponent<Bullet>().weaponData = _weaponData;
    }

    private void Strike(WeaponScriptableObject _weaponData)
    {
        // JUICE:
        // MIAAAAU sound
        // Swipe sound
        StartCoroutine(ShowAttackRangeForDuration(.25f));


        Collider[] hits = Physics.OverlapSphere(transform.position, _weaponData.range);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.gameObject.GetComponent<HealthSystem>().DecreaseLifePoints(_weaponData.damage);

                //JUICE:
                // scratchy Impact sound
                // blood spray
                // stars flying
                StartCoroutine(ShowHitForDuration(.5f));
            }
        }
    }

    // visualize hitting:
    private bool drawAttackSphere = false;
    private bool drawHitSphere = false;


    private IEnumerator ShowAttackRangeForDuration(float duration)
    {
        drawAttackSphere = true;
        yield return new WaitForSeconds(duration);
        drawAttackSphere = false;
    }
    private IEnumerator ShowHitForDuration(float duration)
    {
        drawAttackSphere = true;
        yield return new WaitForSeconds(duration);
        drawAttackSphere = false;
    }

    private void OnDrawGizmos()
    {
        if (drawAttackSphere)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, _weaponData.range); // Replace "YourWeaponDataInstance.range" with your actual range.
        }
        if (drawHitSphere)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _weaponData.range); // Replace "YourWeaponDataInstance.range" with your actual range.
        }
    }
}
