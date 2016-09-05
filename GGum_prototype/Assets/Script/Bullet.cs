using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public float moveSpeed = 10;
    public int power = 1;
    public GameObject effect;

    protected void Move()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.fixedDeltaTime);
    }

    protected void OnEffect()
    {
        effect.SetActive(true);
        effect.transform.parent = null;
    }

}
