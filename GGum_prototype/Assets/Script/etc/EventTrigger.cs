using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

        
    public void PlayerTeleport(Vector3 teleportPoint)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = teleportPoint;

    }

    public void EnemyActive(string Name)
    {
        GameObject enemy = GameObject.Find(Name);

        enemy.SetActive(true);
    }


}
