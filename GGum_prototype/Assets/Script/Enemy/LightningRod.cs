using UnityEngine;
using System.Collections;

public class LightningRod : Bullet {

    public float _fallDelay = 1.0f;

    public Collider2D _collider;

    public bool IsGround { get; set; }


    void OnEnable()
    {
        StartCoroutine(Fall());
    }


    IEnumerator Fall()
    {
        while (!IsGround)
        {
            yield return null;
        }

        yield return new WaitForSeconds(_fallDelay);

        //OnEffect();
        _collider.enabled = true;

        yield return new WaitForSeconds(0.2f);

        Deer owner = pHitData.attacker.GetComponent<Deer>();

        owner.AttackCount++;

        IsGround = false;
        _collider.enabled = false;

        gameObject.SetActive(false);
    }
        	
	
}
