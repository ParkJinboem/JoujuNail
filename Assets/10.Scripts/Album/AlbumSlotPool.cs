using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class AlbumSlotPool : ObjectPool<AlbumSlotPool, AlbumSlotObject, Vector2>
{

}

public class AlbumSlotObject : PoolObject<AlbumSlotPool, AlbumSlotObject, Vector2>
{
    public AlbumSlotItem albumSlotItem;

    protected override void SetReferences()
    {
        albumSlotItem = instance.GetComponent<AlbumSlotItem>();
        albumSlotItem.PoolObject = this;
    }

    public override void WakeUp(Vector2 info)
    {
        instance.transform.localScale = new Vector3(1, 1, 1);
        instance.transform.localRotation = new Quaternion(0, 0, 0, 0);
        instance.transform.position = info;
        instance.SetActive(true);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
