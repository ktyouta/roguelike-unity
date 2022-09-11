using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : Enemy
{
    BoardManager boardObj;
    // Start is called before the first frame update
    protected override void Start()
    {
        boardObj = GManager.instance.gameObject.GetComponent<BoardManager>();
        base.Start();
    }

    /**
     * “G‚ÌHP‚ª0ˆÈ‰º‚É‚È‚Á‚½Œã‚ÉŠK’i‚ğİ’u‚·‚é
     */
    protected override void enemyDefeat()
    {
        boardObj.LayoutStairsAtRandom(transform.position);
        base.enemyDefeat();
    }
}
