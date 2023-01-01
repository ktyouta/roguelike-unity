using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemComponentBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //アイテムを使用
    public virtual void use(Item item, StatusComponentMoving statusObj)
    {
        item.useItem(statusObj);
    }
}
