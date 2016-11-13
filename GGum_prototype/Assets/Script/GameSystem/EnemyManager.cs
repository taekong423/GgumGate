using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    static EnemyManager _instance;

    List<IEnemy> _EnemyList;

    public static EnemyManager Instance
    {
        get
        {
            return _instance;
        }
    }

    EnemyManager()
    {
        _EnemyList = new List<IEnemy>();
    }

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    void Start()
    {
        NewEnemy[] enemys = Resources.FindObjectsOfTypeAll<NewEnemy>();

        _EnemyList.AddRange(enemys);
    }


    public static void AddEnemy(IEnemy enemy)
    {
        Instance._EnemyList.Add(enemy);
    }

    public static void RemoveEnemy(IEnemy enemy)
    {
        Instance._EnemyList.Remove(enemy);
    }

    public static IEnemy GetEnemy(string ID)
    {

        foreach (IEnemy enemy in Instance._EnemyList)
        {
            if (enemy.GetID == ID)
                return enemy;
        }

        return null;

    }

}
