using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumItem : MonoBehaviour
{
    private AlbumObject poolObject;
    public AlbumObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
