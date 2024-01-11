using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class PatternPool : ObjectPool<PatternPool, PatternObject, Vector2>
{

}

public class PatternObject : PoolObject<PatternPool, PatternObject, Vector2>
{
    public PatternItem patternItem;

    protected override void SetReferences()
    {
        patternItem = instance.GetComponent<PatternItem>();
        patternItem.PoolObject = this;
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
