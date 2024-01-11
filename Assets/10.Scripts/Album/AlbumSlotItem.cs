using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumSlotItem : MonoBehaviour
{
    private AlbumSlotObject poolObject;
    public AlbumSlotObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
