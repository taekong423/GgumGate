using UnityEngine;
using System.Collections.Generic;

namespace New
{
    public class EventManager : MonoBehaviour
    {


        #region 변수

        static EventManager _instance;

        Dictionary<Event_Type, List<OnEvent>> _listenerList = new Dictionary<Event_Type, List<OnEvent>>();

        public delegate void OnEvent(Component sender, object param = null);

        #endregion


        #region 프로퍼티

        public static EventManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion


        #region 메소드

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else
                DestroyImmediate(this);
        }

        // 리스너 배열에 리스너 오브젝트를 추가하는 함수
        public static void AddListener(Event_Type eventType, OnEvent listenerEvent)
        {
            // 이벤트를 수신할 리스너의 리스트
            List<OnEvent> listenerList = null;

            // 이벤트 형식의 키가 존재하는지 검사한다. 존재하면 리스트에 추가
            if (Instance._listenerList.TryGetValue(eventType, out listenerList))
            {
                listenerList.Add(listenerEvent);
                return;
            }

            // 키가 존재하지 않으면 새로운 리스트를 만들고 추가한다.
            listenerList = new List<OnEvent>();
            listenerList.Add(listenerEvent);
            Instance._listenerList.Add(eventType, listenerList);

        }

        // 이벤트를 리스너에게 전달하는 함수
        public static void PostNotification(Event_Type eventType, Component sender, object param = null)
        {
            // 이벤트를 수신할 리스너의 리스트
            List<OnEvent> listenerList = null;

            //이벤트 항목이 있는지 검사, 알릴 리스너가 없으면 끝낸다.
            if (!Instance._listenerList.TryGetValue(eventType, out listenerList))
                return;

            for (int i = 0; i < listenerList.Count; i++)
            {
                // 오브젝트가 null이 아니면 함수 호출
                if (!listenerList.Equals(null))
                {
                    listenerList[i](sender, param);
                }
            }

        }

        public static void RemoveEvent(Event_Type eventType)
        {
            Instance._listenerList.Remove(eventType);
        }

        #endregion

    } 
}
