using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerItem : MonoBehaviour
{
    private StickerObject poolObject;
    public StickerObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
