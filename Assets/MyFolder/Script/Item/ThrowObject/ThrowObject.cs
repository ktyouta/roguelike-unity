using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [HideInInspector] public bool isThrownObj = false;
    [HideInInspector] public int playerHorizontalKey;
    [HideInInspector] public int playerVerticalKey;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    [HideInInspector] public float itemXSpeed = 10.0f;
    [HideInInspector] public float itemYSpeed = 10.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrownObj)
        {
            float xSpeed = itemXSpeed;
            float ySpeed = 0.0f;
            if (playerHorizontalKey > 0)
            {
                xSpeed = itemXSpeed;
            }
            else if (playerHorizontalKey < 0)
            {
                xSpeed = (-1) * itemXSpeed;
            }
            if (playerVerticalKey > 0)
            {
                ySpeed = itemYSpeed;
            }
            else if (playerVerticalKey < 0)
            {
                ySpeed = (-1) * itemYSpeed;
            }
            rb.velocity = new Vector2(xSpeed, ySpeed);
            if (!sr.isVisible)
            {
                rb.velocity = new Vector2(0.0f, 0.0f);
                isThrownObj = false;
            }
        }
    }

    /**
     * áŠQ•¨‚É‚Ô‚Â‚©‚Á‚½ê‡“®‚«‚ðŽ~‚ß‚é
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player" && other.tag != "Forest" && other.tag != "Floor")
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
            isThrownObj = false;
        }
    }
}
