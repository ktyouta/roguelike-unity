using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static ComponentSettingManager;

[System.Serializable]
public class StatusComponentBase : MonoBehaviour
{
    //HP
    [HideInInspector] public HpClass charHp = new HpClass();
    //–¼‘O
    [HideInInspector] public NameClass charName = new NameClass();

    protected virtual void Start()
    {

    }
}