using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class QuestPaintScenePool : ObjectPool<QuestPaintScenePool, QuestPaintSceneObject, Vector2>
{

}

public class QuestPaintSceneObject : PoolObject<QuestPaintScenePool, QuestPaintSceneObject, Vector2>
{
    public QuestPaintSceneItem questPaintSceneItem;

    protected override void SetReferences()
    {
        questPaintSceneItem = instance.GetComponent<QuestPaintSceneItem>();
        questPaintSceneItem.PoolObject = this;
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
