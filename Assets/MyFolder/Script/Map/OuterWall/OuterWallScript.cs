using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterWallScript : MonoBehaviour
{
    [Header("”j‰ó•s‰Âƒtƒ‰ƒO")] public bool isIndestructible;
    [Header("•Ç‚Ì‘Ï‹v’l")] public int wallHp;

    // Start is called before the first frame update
    void Start()
    {
        wallHp = wallHp == 0 ?100:wallHp;
    }
}
