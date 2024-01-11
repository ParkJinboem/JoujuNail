using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnDot.Util;

public class GiftBoxItem : MonoBehaviour
{
    public ParticleSystem clicekdParticle;
    private GiftBoxObject poolObject;
    public GiftBoxObject PoolObject
    {
        set { poolObject = value; }
    }

    /// <summary>
    /// 초기값 설정
    /// </summary>
    public void Init()
    {
        transform.position = Vector3.zero;
    }

    public void Hide()
    {
        poolObject.ReturnToPool();
    }

    public void ParticlePlay()
    {
        clicekdParticle.Play();
    }
}
