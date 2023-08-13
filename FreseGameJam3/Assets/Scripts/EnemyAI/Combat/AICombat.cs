using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombat : MonoBehaviour
{
    public GameObject weapon; // accessed by the EnemyDetaction-script!
    public bool inRange = false; // accessed by the EnemyDetaction-script!

    private bool _attacking = false;
    private WeaponScriptableObject _weaponData;
    [SerializeField] AudioSource _attackSoundPlayer;

    // Animation vars:
    Animator _animator;
    Animator _animatorWeapon;

    [Header("Do Not Touch:")]
    public bool isAlive = true;

    private void OnEnable()
    {
        // check which weapon is being carried:
        _weaponData = weapon.GetComponent<WeaponData>().data;

        if (_weaponData.type != 0)
        {
            _animator = transform.Find("Geometry:/G_KatziArmsShootingWalking").GetComponent<Animator>();
            _animatorWeapon = transform.Find("Geometry:/G_KatziArmsShootingWalking/ArmatureKatziArms/Bone/Bone.014/Bone.015/Bone.016/Bone.017/" + weapon.name + "/" + weapon.name).GetComponent<Animator>();
        }else
        {
            _animator = transform.Find("Geometry:/G_KatziArmsShootingWalking").GetComponent<Animator>();
        }

        _attackSoundPlayer.clip = _weaponData.attackSound;
        _attackSoundPlayer.volume = 0.1f;
    }

    private void Update()
    {
        if(inRange && !_attacking && isAlive)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        _attacking = true;


        if (_attackSoundPlayer.clip != null)
        {
            _attackSoundPlayer.Play();
        }

        // create bullet if applicable:
        if (_weaponData.type != 0)
        {
            // start attack animation:
            _animator.SetBool("Shoot", _attacking); // shoot includes close combat
            _animatorWeapon.SetBool("Shoot", _attacking); // animate weapon

            FireBullet(_weaponData);
        }else
        {
            // start attack animation:
            _animator.SetBool("Shoot", _attacking); // shoot includes close combat
            //_animatorWeapon.SetBool("Shoot", _attacking); // animate weapon

            //Strike(_weaponData);
            Strike();
        }

        yield return new WaitForSeconds(_weaponData.attackSpeed);

        // if still in range, attack again:
        _attacking = false;

        if (_weaponData.type != 0)
        {
            // stop attack animation:
            _animator.SetBool("Shoot", _attacking); // stop shoot includes close combat
            _animatorWeapon.SetBool("Shoot", _attacking); // stop animate weapon
        }else
        {
            // stop attack animation:
            _animator.SetBool("Shoot", _attacking); // stop shoot includes close combat
            //_animatorWeapon.SetBool("Shoot", _attacking); // stop animate weapon
        }
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

    //private void Strike(WeaponScriptableObject _weaponData)
    private void Strike()
    {
        StartCoroutine(ShowAttackRangeForDuration(.25f));


        Collider[] hits = Physics.OverlapSphere(transform.position, _weaponData.range * 2);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player") && hit.gameObject.GetComponentInParent<HealthSystem>() != null)
            {
                hit.gameObject.GetComponentInParent<HealthSystem>().DecreaseLifePoints(_weaponData.damage);

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
