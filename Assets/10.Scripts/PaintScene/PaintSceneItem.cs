using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSceneItem : MonoBehaviour
{
    private PaintSceneObject poolObject;
    public PaintSceneObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
