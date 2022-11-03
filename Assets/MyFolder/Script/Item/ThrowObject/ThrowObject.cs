using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [HideInInspector] public bool isThrownObj = false;
    [HideInInspector] public int playerHorizontalKey;
    [HideInInspector] public int playerVerticalKey;
    [HideInInspector] public float itemXSpeed = 10.0f;
    [HideInInspector] public float itemYSpeed = 10.0f;
    private SpriteRenderer sr;
    public Rigidbody2D rb;
    private Item it;

    // Start is called before the first frame update
    void Start()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (it == null)
        {
            it = GetComponent<Item>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ƒtƒ‰ƒO‚ªtrue‚Ìê‡‚ÉˆÚ“®‚·‚é
        if (!isThrownObj)
        {
            return;
        }
        //‰æ–ÊŠO‚Éo‚½ê‡‚Í’â~‚·‚é
        if (!sr.isVisible)
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
            isThrownObj = false;
        }
    }

    /**
     * áŠQ•¨‚É‚Ô‚Â‚©‚Á‚½ê‡“®‚«‚ğ~‚ß‚é
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Forest" || other.tag == "Floor" || other.tag == "Untagged")
        {
            return;
        }
        //“G‚É‚Ô‚Â‚©‚Á‚½ê‡
        if (other.tag == "Enemy" && isThrownObj)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            //enemy.enemyHp -= 10;
            it.collisionItem(enemy);
            it.deleteSelectedItem(it.id);
            Destroy(this.gameObject);
        }
        rb.velocity = new Vector2(0.0f, 0.0f);
        isThrownObj = false;
    }
}
