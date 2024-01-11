using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class GiftBoxPool : ObjectPool<GiftBoxPool, GiftBoxObject, Vector2>
{

}

public class GiftBoxObject : PoolObject<GiftBoxPool, GiftBoxObject, Vector2>
{
    public GiftBoxItem giftBoxItem;
    
    protected override void SetReferences()
    {
        giftBoxItem = instance.GetComponent<GiftBoxItem>();
        giftBoxItem.PoolObject = this;
    }
    
    public override void WakeUp(Vector2 info)
    {
        instance.transform.localScale = new Vector3(1, 1, 1);
        instance.transform.position = info;
        instance.SetActive(true);
    }
    
    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
