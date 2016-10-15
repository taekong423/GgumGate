using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {

    public ItemType type;
    public ItemData itemData;
    public List<ItemEffect> effectList;

    private Player player;
    private GameObject effect;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private bool groundCheck;
    private bool searchOn;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = Vector2.zero;
        groundCheck = false;
        //Invoke("StartGroundCheck", 0.5f);
    }

    void Update()
    {
        if (IsGround(transform.position, _collider.bounds.extents.y + 0.1f) && _rigidbody.velocity.y < 0)
        {
            //_collider.isTrigger = true;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.isKinematic = true;
            searchOn = true;
        }

        if (searchOn)
        {
            Search();
        }
    }

    void OnDestroy()
    {
        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }
    }

    void StartGroundCheck()
    {
        groundCheck = true;
    }

    protected bool IsGround(Vector2 originPos, float rayLength)
    {
        RaycastHit2D hit = Physics2D.Raycast(originPos, Vector2.down, rayLength, LayerMask.GetMask("Ground"));
        return (hit.collider != null);
    }

    protected void Search()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= _collider.bounds.extents.x + 20.0f)
        {
            transform.position = Vector2.Lerp(transform.position, player.transform.position, Time.deltaTime * 10);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(coll.collider, _collider, true);
        }
    }
}
