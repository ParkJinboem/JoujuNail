using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManicureItem : MonoBehaviour
{
    private ManicureObject poolObject;
    public ManicureObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
