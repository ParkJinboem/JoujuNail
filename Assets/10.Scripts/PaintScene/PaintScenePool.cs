using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class PaintScenePool : ObjectPool<PaintScenePool, PaintSceneObject, Vector2>
{

}

public class PaintSceneObject : PoolObject<PaintScenePool, PaintSceneObject, Vector2>
{
    public PaintSceneItem paintSceneItem;

    protected override void SetReferences()
    {
        paintSceneItem = instance.GetComponent<PaintSceneItem>();
        paintSceneItem.PoolObject = this;
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