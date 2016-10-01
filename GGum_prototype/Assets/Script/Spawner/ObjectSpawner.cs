using UnityEngine;
using System.Collections;
using System;

public class ObjectSpawner : Spawner {


    public override void Spawn()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        Instantiate(_spawnPrefab, _spawnPoint.position, _spawnPoint.rotation);
    }

    
}
