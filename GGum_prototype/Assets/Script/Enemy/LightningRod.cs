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
        effect.SetActive(true);

        Deer owner = pHitData.attacker.GetComponent<Deer>();

        owner.AttackCount++;

        yield return new WaitForSeconds(0.2f);
                
        IsGround = false;
        _collider.enabled = false;
        effect.SetActive(false);

        gameObject.SetActive(false);
    }
        	
	
}
