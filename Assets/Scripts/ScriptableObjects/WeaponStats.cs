using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class WeaponStats : ScriptableObject
    {
        public float baseTimeBetweenShots;
        public float recoilForce;
    }
}
