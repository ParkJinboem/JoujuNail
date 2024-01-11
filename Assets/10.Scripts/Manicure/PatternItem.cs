using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternItem : MonoBehaviour
{
    private PatternObject poolObject;
    public PatternObject PoolObject
    {
        set { poolObject = value; }
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }
}
