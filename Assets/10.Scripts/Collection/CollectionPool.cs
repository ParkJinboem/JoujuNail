using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class CollectionPool : ObjectPool<CollectionPool, CollectionObject, Vector2>
{

}

public class CollectionObject : PoolObject<CollectionPool, CollectionObject, Vector2>
{
    public CollectionItem collectionItem;
    
    protected override void SetReferences()
    {
        collectionItem = instance.GetComponent<CollectionItem>();
        collectionItem.PoolObject = this;
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
