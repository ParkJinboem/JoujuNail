using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class AlbumScenePool : ObjectPool<AlbumScenePool, AlbumSceneObject, Vector2>
{

}

public class AlbumSceneObject : PoolObject<AlbumScenePool, AlbumSceneObject, Vector2>
{
    public AlbumSceneItem albumSceneItem;

    protected override void SetReferences()
    {
        albumSceneItem = instance.GetComponent<AlbumSceneItem>();
        albumSceneItem.PoolObject = this;
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
