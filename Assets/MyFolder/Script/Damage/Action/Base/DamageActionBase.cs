using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageActionBase:MonoBehaviour
{
    protected StatusComponentBase statusObj;

    private void Start()
    {
        this.statusObj = GetComponent<StatusComponentBase>();
    }

    public abstract void reciveDamageAction(int hp);
}
