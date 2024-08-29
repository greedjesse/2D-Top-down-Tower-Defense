using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    public float timeBetweenShots;
    public float bulletSpeed;
    public float bulletLastTime;
    public float recoilForce;
    public float playerShrinkage;
}
