using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageActionComponentBase:MonoBehaviour
{
    protected StatusComponentBase statusObj;

    protected virtual void Start()
    {
        this.statusObj = GetComponent<StatusComponentBase>();
    }

    public abstract void reciveDamageAction(int hp);
}
