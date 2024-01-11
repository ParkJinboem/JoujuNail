using UnityEngine;
using OnDot.Util;

public class ManicurePool : ObjectPool<ManicurePool, ManicureObject, Vector2>
{

}

public class ManicureObject : PoolObject<ManicurePool, ManicureObject, Vector2>
{
    public ManicureItem manicureItem;

    protected override void SetReferences()
    {
        manicureItem = instance.GetComponent<ManicureItem>();
        manicureItem.PoolObject = this;
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
