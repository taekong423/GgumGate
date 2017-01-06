using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class Global : MonoBehaviour
{
    private const string prefab_path = "Prefab/Manager/GlobalManager";
    private static bool _create_lock = false;
    private static Global _instance = null;

    static object synLock = new object();

    private static Global _this
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Global>();
            }

            if (_instance == null)
            {
                lock (synLock)
                {
                    GameObject prefab = Resources.Load<GameObject>(prefab_path);
                    GameObject new_object = GameObject.Instantiate<GameObject>(prefab);
                    new_object.name = prefab.name;
                }
            }

            return _instance;
        }
    }


    private Dictionary<Type, MonoBehaviour> _shared_objects;
    private Dictionary<Type, MonoBehaviour> _component_objects;

    public Global()
    {
        _shared_objects = new Dictionary<Type, MonoBehaviour>();
        _component_objects = new Dictionary<Type, MonoBehaviour>();
    }

    void Awake()
    {
        
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

        }

        foreach (MonoBehaviour mono in this.GetComponentsInChildren<MonoBehaviour>())
        {
            _component_objects.Add(mono.GetType(), mono);
        }
    }

    void Start()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        _shared_objects.Clear();
        _shared_objects = null;

        _component_objects.Clear();
        _component_objects = null;

        //_instance = null;
    }

    void OnApplicationQuit()
    {
        _create_lock = true;
    }

    private bool have_shared<T>() where T : MonoBehaviour
    {
        return _shared_objects.ContainsKey(typeof(T));
    }

    private GameObject make_object(string prefab_path)
    {
        GameObject new_object = GameObject.Instantiate(Resources.Load<GameObject>(prefab_path)) as GameObject;
        new_object.transform.parent = this.transform;

        return new_object;
    }

    public static bool has_shared<T>() where T : MonoBehaviour
    {
        return _this.have_shared<T>();
    }

    public static void shared<T>(T instance) where T : MonoBehaviour
    {
        if (!_this.have_shared<T>())
        {
            _this._shared_objects.Add(typeof(T), instance);
        }
    }

    public static T shared<T>() where T : MonoBehaviour
    {
        return _this._shared_objects[typeof(T)] as T;
    }

    public static void remove_shared<T>() where T : MonoBehaviour
    {
        if (!_create_lock && _this.have_shared<T>())
        {
            _this._shared_objects.Remove(typeof(T));
        }
    }

    public static T component<T>() where T : MonoBehaviour
    {
        if (!have_component<T>())
        {
            T component = _this.GetComponent<T>();
            _this._component_objects.Add(component.GetType(), component);
            return component;
        }

        return get_component<T>();
    }

    public static T component<T>(string prefab_path) where T : MonoBehaviour
    {
        if (!have_component<T>())
        {
            GameObject new_prefab = _this.make_object(prefab_path);
            T component = new_prefab.GetComponent<T>();
            _this._component_objects.Add(component.GetType(), component);

            return component;
        }

        return get_component<T>();
    }

    public static bool have_component<T>() where T : MonoBehaviour
    {
        return _this._component_objects.ContainsKey(typeof(T));
    }

    public static T get_component<T>() where T : MonoBehaviour
    {
        return _this._component_objects[typeof(T)] as T;
    }

    public static void remove_component<T>() where T : MonoBehaviour
    {
        if (!_create_lock)
        {
            if (_this.GetComponent<T>() != null)
            {
                DestroyImmediate(get_component<T>());
            }
            else
            {
                DestroyImmediate(_this.GetComponentInChildren<T>().gameObject);
            }
        }
    }

    public static void coroutine(IEnumerator service)
    {
        _this.StartCoroutine(service);
    }

    public static void stop_coroutine(IEnumerator service)
    {
        _this.StopCoroutine(service);
    }

    public static bool is_dead()
    {
        return _this == null;
    }

}