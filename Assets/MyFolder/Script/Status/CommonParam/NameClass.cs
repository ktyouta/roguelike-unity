using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NameClass
{
    [Header("�L�����̖��O")] public string name;

    /**
     * ���O��ԋp
     */
    public string showName()
    {
        return name;
    }
}
