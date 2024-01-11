using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class StickerPool : ObjectPool<StickerPool, StickerObject, Vector2>
{

}

public class StickerObject : PoolObject<StickerPool, StickerObject, Vector2>
{
    public StickerItem stickerItem;

    protected override void SetReferences()
    {
        stickerItem = instance.GetComponent<StickerItem>();
        stickerItem.PoolObject = this;
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
