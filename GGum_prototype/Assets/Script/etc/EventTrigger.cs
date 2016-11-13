using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

        
    public void PlayerTeleport(Transform teleportPoint)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = teleportPoint.position;

    }

    public void EnemyActive(string ID)
    {
        Debug.Log("Acitve");
        IEnemy enemy = EnemyManager.GetEnemy(ID);

        if (enemy == null)
            return;

        enemy.Active(true);
    }
    
}
