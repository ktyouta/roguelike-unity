using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : RecoveryItem
{
    [Header("�����x�񕜗�")] public int foodPoint;
    public Text foodText;
    // Start is called before the first frame update
    protected override void Start()
    {
        foodText = GameObject.Find("Food").GetComponent<Text>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    public override void useItem()
    {
        //�v���C���[�̖����x�����^���̏ꍇ�͉񕜂��Ȃ�
        if (GManager.instance.playerFoodPoint == GManager.instance.playerMaxFoodPoint)
        {
            GManager.instance.wrightLog("�v���C���[�̖����x�����^���ł��B");
            return;
        }
        GManager.instance.recoveryFoodPoint(foodPoint);
        base.useItem();
    }
}
