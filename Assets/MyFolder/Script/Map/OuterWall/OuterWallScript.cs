using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterWallScript : MonoBehaviour
{
    [Header("�j��s�t���O")] public bool isIndestructible;
    [Header("�ǂ̑ϋv�l")] public int wallHp;

    // Start is called before the first frame update
    void Start()
    {
        wallHp = wallHp == 0 ?100:wallHp;
    }
}
