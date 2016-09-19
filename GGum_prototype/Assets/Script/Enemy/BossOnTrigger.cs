using UnityEngine;
using System.Collections;

public class BossOnTrigger : MonoBehaviour {

    public GameObject _boss;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_boss.activeSelf == false)
            {
                _boss.SetActive(true);

                gameObject.SetActive(false);

            }
        }
    }

}
