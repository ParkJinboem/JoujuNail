using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideStickerItem : MonoBehaviour
{
    private GuideStickerObject poolObject;
    public GuideStickerObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
