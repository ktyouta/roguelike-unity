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
        //�t���O��true�̏ꍇ�Ɉړ�����
        if (!isThrownObj)
        {
            return;
        }
        //��ʊO�ɏo���ꍇ�͒�~����
        if (!sr.isVisible)
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
            isThrownObj = false;
        }
    }

    /**
     * ��Q���ɂԂ������ꍇ�������~�߂�
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Forest" || other.tag == "Floor" || other.tag == "Untagged")
        {
            return;
        }
        if (isThrownObj)
        {
            //�Փˎ��̃A�N�V����
            if (other.tag == "Enemy" || other.tag == "Player")
            {
                it.collisionItem(other.gameObject);
                Destroy(this.gameObject);
            }
        }
        rb.velocity = new Vector2(0.0f, 0.0f);
        isThrownObj = false;
    }
}
