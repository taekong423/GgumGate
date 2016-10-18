using UnityEngine;
using System.Collections;

public abstract class Spawner : MonoBehaviour {


    float _currentDelay = 0;

    [Header("BaseSetting")]
	public bool _isAuto = false;
    public bool _isRandomDelay = false;

    public float _startDelay = 0;

    public float _minDelay = 0;
    public float _maxDelay = 1.0f;


    [Tooltip("value 0 -> Loop")]
    public int _spawnNum = 0;
    public float _spawnDelay = 1.0f;
    
    public Transform _spawnPoint;
    public GameObject _spawnPrefab;

    public abstract void Spawn();

    void Awake()
    {
        _currentDelay = _startDelay;
    }

    void OnEnable()
    {
        if(_isAuto)
            StartCoroutine(SpawnUpdate());
    }

    protected virtual IEnumerator SpawnUpdate()
    {
        

        int spawnCount = 0;

        bool isLoop = true;

        while (isLoop)
        {
            if (_currentDelay <= 0)
            {

                Spawn();

                if (_isRandomDelay)
                    _currentDelay = Random.Range(_minDelay, _maxDelay);
                else
                    _currentDelay = _spawnDelay;

                if (_spawnNum != 0)
                {
                    spawnCount++;

                    if (spawnCount >= _spawnNum)
                        isLoop = false;
                    
                }

            }
            else
                _currentDelay -= Time.deltaTime;

            yield return null;
        }

        yield return null;
    }

}
