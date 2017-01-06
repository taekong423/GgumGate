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
        [SerializeField]
        GameObject _characterColliderObject;

        Collider2D[] _characterColliders;
        protected Collider2D[] _ignoreColliders;

        bool _isDead = false;

        bool _canThrough = false;
        bool _isThroughing = false;

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

        public bool CanThrough { get { return _canThrough; } set { _canThrough = value; } }

        #endregion

        #region 메소드

        void Awake()
        {
            _currentHP = _maxHP;

        }

        void Start()
        {
            _characterColliders = _characterColliderObject.GetComponents<Collider2D>();
        }

        public virtual HitResult OnHit(HitData hitdata)
        {
            HitResult result = new HitResult(gameObject, CurrentHP);
            return result;
        }

        // 콜라이더를 무시 하지 않게 하는 함수
        public virtual void NoIgnoreCollision(Collider2D[] colliders)
        {
            if (!_isThroughing)
                return;

            _isThroughing = false;

            foreach (Collider2D cCol in _characterColliders)
            {
                foreach (Collider2D pCol in colliders)
                {
                    Physics2D.IgnoreCollision(pCol, cCol, false);
                }

            }
        }

        // 콜라이더를 무시하게 하는 함수
        public virtual void IgnoreCollision(Collider2D[] colliders)
        {
            if (_isThroughing)
                return;

            _isThroughing = true;

            foreach (Collider2D cCol in _characterColliders)
            {
                foreach (Collider2D pCol in colliders)
                {
                    Physics2D.IgnoreCollision(pCol, cCol, true);
                }
            }

        }

        public void SetIgnoreColliders(Collider2D[] colliders)
        {
            _ignoreColliders = colliders;
        }

        #endregion
    } 
}
