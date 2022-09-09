using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    Renderer render;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        render = GetComponent<Renderer>();
        if (render == null)
        {
            Destroy(gameObject);
        }
        render.enabled = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        render.enabled = true;
    }
}
