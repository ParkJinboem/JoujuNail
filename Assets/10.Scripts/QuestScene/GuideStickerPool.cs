using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class GuideStickerPool : ObjectPool<GuideStickerPool, GuideStickerObject, Vector2>
{

}

public class GuideStickerObject : PoolObject<GuideStickerPool, GuideStickerObject, Vector2>
{
    public GuideStickerItem guideStickerItem;

    protected override void SetReferences()
    {
        guideStickerItem = instance.GetComponent<GuideStickerItem>();
        guideStickerItem.PoolObject = this;
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
