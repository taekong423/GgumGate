using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

    public Vector2 _teleportPoint;
        
    public void PlayerTeleport()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = _teleportPoint;

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
