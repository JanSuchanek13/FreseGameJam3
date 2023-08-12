using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapon", order = 1)]

public class WeaponScriptableObject : ScriptableObject
{
    public string prefabName;
    public int WeaponID;
    public GameObject WeaponPrefab;
    public GameObject BulletPrefab;
    [Tooltip("0 = CC; 1 = Direct; 2 = Indirect")]
    public int type;
    [Tooltip("0 = SingleShot; 1 = Volley; 2 = FullyAutomativ")]
    public int fireMode;
    public float range;
    public float attackSpeed;
    public float damage;
    public float bulletSpeed;
    public float recoil;
    public bool explosive;
    public float aimAccuracy;
}
