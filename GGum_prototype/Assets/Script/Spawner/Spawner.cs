using UnityEngine;
using System.Collections;

public abstract class Spawner : MonoBehaviour {


    [Header("BaseSetting")]
	public bool _isAuto = false;
    public bool _isRandomDelay = false;

    public float _minDelay = 0;
    public float _maxDelay = 1.0f;


    [Tooltip("value 0 -> Loop")]
    public int _spawnNum = 0;
    public float _spawnDelay = 1.0f;
    
    public Transform _spawnPoint;
    public GameObject _spawnPrefab;

    public abstract void Spawn();

    void OnEnable()
    {
        if(_isAuto)
            StartCoroutine(SpawnUpdate());
    }

    protected virtual IEnumerator SpawnUpdate()
    {
        float currentDelay = 0;

        int spawnCount = 0;

        bool isLoop = true;

        while (isLoop)
        {
            if (currentDelay <= 0)
            {

                Spawn();

                if (_isRandomDelay)
                    currentDelay = Random.Range(_minDelay, _maxDelay);
                else
                    currentDelay = _spawnDelay;

                if (_spawnNum != 0)
                {
                    spawnCount++;

                    if (spawnCount >= _spawnNum)
                        isLoop = false;
                    
                }

            }
            else
                currentDelay -= Time.deltaTime;

            yield return null;
        }

        yield return null;
    }

}
