using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRecycle : MonoBehaviour
{
    public Transform returnParent { set; get; }

    public void OnParticleSystemStopped()
    {
        Clear();
    }

    public void Clear()
    {
        Destroy(this);
        //this.transform.SetParent(returnParent);
    }
}
