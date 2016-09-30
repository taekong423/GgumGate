using UnityEngine;
using System.Collections;

public class Fairy : Character {

    public GameObject playerObject;
    public Player player;

    public float amplitude;
    public bool isFloating = true;
    public Transform targetPos;

    private float targetX;
    private float targetY;

    // Use this for initialization
    void Awake () {
        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<Player>();
        _statePattern = new FairyState(this);
        _statePattern.StartState();
        StartCoroutine(ChangeTargetPosition());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator ChangeTargetPosition()
    {
        while (isFloating)
        {
            float x = Random.Range(-0.5f, 0.5f);
            targetPos.position = new Vector2(player.fairyPoint.position.x + x, player.fairyPoint.position.y + 3.0f);
            yield return new WaitForSeconds(0.5f);
            x = Random.Range(-0.5f, 0.5f);
            targetPos.position = new Vector2(player.fairyPoint.position.x + x, player.fairyPoint.position.y - 3.0f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Follow()
    {
        targetX = Mathf.Lerp(transform.position.x, targetPos.position.x, moveSpeed * Time.deltaTime);
        targetY = Mathf.Lerp(transform.position.y, targetPos.position.y, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
