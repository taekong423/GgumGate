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
        GameObject obj = Instantiate(_spawnPrefab, _spawnPoint.position, _spawnPoint.rotation) as GameObject;
        //obj.GetComponent<Bullet>().pHitData = new HitData(gameObject, 1);
    }

    
}
