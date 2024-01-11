using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumSceneItem : MonoBehaviour
{
    private AlbumSceneObject poolObject;
    public AlbumSceneObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
