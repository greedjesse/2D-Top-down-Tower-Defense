using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class BulletStats : ScriptableObject
    {
        public float baseBulletSpeed;
        public float bulletLastTime;
    }
}
