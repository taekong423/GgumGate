using UnityEngine;
using System.Collections;

public class NpcRunningAway : Character {

    private float dist;
    private GameManager gm;
    private Player player;
    private Vector2 initialPos;
    private Collider2D _collider;

	// Use this for initialization
	void Start () {
        _transform = transform;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        initialPos = transform.position;
        _collider = GetComponent<Collider2D>();
        moveSpeed = Random.Range(70.0f, 90.0f);
        dist = 300;
    }
	
	// Update is called once per frame
	void Update () {
        if (gm.flags["NpcRunningAway"] == true)
        {
            if (transform.position.x < (player.gameObject.transform.position.x - dist))
            {
                transform.position = initialPos;
                gameObject.SetActive(false);
            }
            else
            {
                Move(Axis.Horizontal, -1.0f);
            }
        }
    }
}
