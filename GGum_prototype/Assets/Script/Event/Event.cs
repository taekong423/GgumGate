using UnityEngine;
using System.Collections;

namespace New
{
    [System.Serializable]
    public class Event : MonoBehaviour
    {

        #region 변수

        [SerializeField]
        Event_Type _type;

        // HealthChange
        public Character _healthTarget;
        public float _health;

        // OnDialogue
        public DialogueID _dataID;

        // Teleport
        public Transform _teleportTarget;
        public Transform _point;
        public Vector3 _pointVector;

        // Dead
        public Character _deadTarget;

        #endregion

        #region 프로퍼티

        public Event_Type Type { get { return _type; } set { _type = value; } }

        #endregion

        #region 메소드


        // 이벤트를 실행하는 함수
        public virtual void Execute()
        {
            switch (_type)
            {
                case Event_Type.HealthChange:
                    {
                        ChangeHealth();
                    }
                    break;

                case Event_Type.OnDialogue:
                    {
                        OnDialogue();
                    }
                    break;

                case Event_Type.EndDialogue:
                    {
                        EndDialogue();
                    }
                    break;

                case Event_Type.Teleport:
                    {
                        Telelport();
                    }
                    break;

                case Event_Type.Dead:
                    {
                        Dead();
                    }
                    break;
            }
        }

        // 이벤트를 실행하는 함수, 리스너 등록용
        public virtual void Execute(Component sender, object param = null)
        {
            switch (_type)
            {
                case Event_Type.HealthChange:
                    {
                        if (sender != _healthTarget)
                            ChangeHealth();
                    }
                    break;

                case Event_Type.OnDialogue:
                    {
                        OnDialogue();
                    }
                    break;

                case Event_Type.EndDialogue:
                    {
                        EndDialogue();
                    }
                    break;

                case Event_Type.Teleport:
                    {
                        Telelport();
                    }
                    break;

                case Event_Type.Dead:
                    {
                        Dead();
                    }
                    break;
            }
        }

        void ChangeHealth()
        {
            _healthTarget.CurrentHP = _health;
        }

        void OnDialogue()
        {
            Dialogue.Display(_dataID);
        }

        void EndDialogue()
        {
            //Dialogue.SkipDialogue_Class();
        }

        void Telelport()
        {
            if (_point != null)
            {
                _teleportTarget.position = _point.position;
            }
            else
            {
                _teleportTarget.localPosition = _pointVector;
            }
        }

        void Dead()
        {

        }

        #endregion
    }
}