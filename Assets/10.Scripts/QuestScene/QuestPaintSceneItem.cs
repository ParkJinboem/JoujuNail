using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPaintSceneItem : MonoBehaviour
{
    private QuestPaintSceneObject poolObject;
    public QuestPaintSceneObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
