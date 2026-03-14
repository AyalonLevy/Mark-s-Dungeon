using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Data/Equipment/Weapon")]
public class WeaponData : EquipmentData
{
    public int MinDamage;
    public int MaxDamage;

    [Header("Range Weapons Only")]
    public float Range;
    public GameObject Ammo;
}
