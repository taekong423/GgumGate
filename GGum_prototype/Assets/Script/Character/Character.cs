using UnityEngine;

namespace New
{
    public class Character : MonoBehaviour, ICharacter
    {

        #region 변수

        [Header("Character")]
        float _currentHP = 0.0f;
        [SerializeField]
        float _maxHP = 1.0f;

        bool _isDead = false;

        #endregion

        #region 프로퍼티

        public float CurrentHP
        {
            get
            {
                return _currentHP;
            }
            set
            {
                _currentHP = value;
                _currentHP = Mathf.Clamp(_currentHP, 0.0f, _maxHP);

                EventManager.PostNotification(Event_Type.HealthChange, this, _currentHP);
            }
        }
        public float MaxHP { get { return _maxHP; } set { _maxHP = value; } }

        public bool IsDead { get { return _isDead; } set { _isDead = value; } }

        #endregion

        #region 메소드

        void Awake()
        {
            _currentHP = _maxHP;
        }

        public virtual HitResult OnHit(HitData hitdata)
        {
            HitResult result = new HitResult(gameObject, CurrentHP);
            return result;
        }

        #endregion
    } 
}
