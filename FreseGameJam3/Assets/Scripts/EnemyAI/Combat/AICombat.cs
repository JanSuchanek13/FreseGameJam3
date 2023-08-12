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

        float _maxNegScatter = (1.0f - _weaponData.aimAccuracy) * -1; // eg. -.25 @ 75% accuracy
        float _maxPosScatter = 1.0f - _weaponData.aimAccuracy; // eg .25 @ 75% accuracy
        float _scatter = Random.Range(_maxNegScatter, _maxPosScatter);

        GameObject _bullet = Instantiate(_weaponData.BulletPrefab, (transform.position + Vector3.up * .75f), transform.rotation);
        _bullet.GetComponent<Bullet>().direction = (transform.position + Vector3.up * .75f) + (transform.forward + new Vector3(_scatter, 0, 0)) * _weaponData.range;

        _bullet.GetComponent<Bullet>().weaponData = _weaponData;
    }

    private void Strike(WeaponScriptableObject _weaponData)
    {
        // JUICE:
        // MIAAAAU sound
        // Swipe sound

        //test: visualize melee attack
        StartCoroutine(ShowAttackRangeForDuration(.25f));


        Collider[] hits = Physics.OverlapSphere(transform.position, _weaponData.range * 2);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.gameObject.GetComponent<HealthSystem>().DecreaseLifePoints(_weaponData.damage);

                //JUICE:
                // scratchy Impact sound
                // blood spray
                // stars flying

                //test: visualize melee attack
                StartCoroutine(ShowHitForDuration(.25f));
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
        drawHitSphere = true;
        yield return new WaitForSeconds(duration);
        drawHitSphere = false;
    }

    private void OnDrawGizmos()
    {
        if (drawAttackSphere)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, _weaponData.range * 2f); 
        }
        if (drawHitSphere)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _weaponData.range * 1.5f);
        }
    }
}
