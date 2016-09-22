using UnityEngine;
using System.Collections.Generic;

public class NewButtonManager : MonoBehaviour {

    static string _prefabPath = "Prefab/Manager/NewButtonManager";

    static NewButtonManager _instance;

    static object synLock = new object();

    public List<NewButtonData> _buttonDatas;


    public static NewButtonManager This
    {
        get
        {
            if (_instance == null)
            {
                lock (synLock)
                {
                    GameObject prefab = Resources.Load<GameObject>(_prefabPath);

                    GameObject obj = GameObject.Instantiate(prefab);

                    obj.name = prefab.name;

                }
            }

            return _instance;

        }
    }

	// Use this for initialization
	void Awake () {

        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

	}

    NewButtonData GetButtonData(EButtonCode buttonCode)
    {
        if (_buttonDatas.Count <= 0)
            return null;

        for (int i = 0; i < _buttonDatas.Count; i++)
        {
            if (_buttonDatas[i]._buttonCode == buttonCode)
            {
                return _buttonDatas[i];
            }
        }

        return null;
    }

    public static void RegisterButtonDownFunc(EButtonCode buttonCode, NewButtonData.ButtonFunction func)
    {
        NewButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RegisterButtonDownFunc(func);
        }

    }

    public static void RemoveButtonDwonFunc(EButtonCode buttonCode, NewButtonData.ButtonFunction func)
    {
        NewButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RemoveButtonDwonFunc(func);
        }
    }

    public static bool ButtonDown(EButtonCode buttonCode)
    {
        NewButtonData data = This.GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return false;
        }

        return data.GetButtonDown();
    }
}
