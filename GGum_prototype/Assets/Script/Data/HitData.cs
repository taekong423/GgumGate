using UnityEngine;

namespace New
{
    public struct HitData
    {

        public GameObject _attacker;
        public float _damage;

        public HitData(GameObject attacker, float damage)
        {
            _attacker = attacker;
            _damage = damage;
        }

    } 
}
