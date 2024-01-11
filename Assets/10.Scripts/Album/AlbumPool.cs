using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class AlbumPool : ObjectPool<AlbumPool, AlbumObject, Vector2>
{

}

public class AlbumObject : PoolObject<AlbumPool, AlbumObject, Vector2>
{
    public AlbumItem albumItem;

    protected override void SetReferences()
    {
        albumItem = instance.GetComponent<AlbumItem>();
        albumItem.PoolObject = this;
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
