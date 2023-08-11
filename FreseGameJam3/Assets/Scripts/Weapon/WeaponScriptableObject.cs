using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapon", order = 1)]

public class WeaponScriptableObject : ScriptableObject
{
    public string prefabName;
    public int WeaponID;
    public GameObject WeaponPrefab;
    public GameObject BulletPrefab;
    public enum type
    {
        CC,
        direct,
        indirect
    }
    public enum fireMode
    {
        SingleShot,
        Volley,
        FullyAutomativ
    }
    public float range;
    public float firingRate;
    public float damage;
    public float bulletSpeed;
    public float recoil;
    public bool explosive;
    public float aimAccuracy;
}
