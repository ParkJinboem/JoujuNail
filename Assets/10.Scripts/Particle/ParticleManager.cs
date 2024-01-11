using OnDot.Util;
using UnityEngine;

public class ParticleManager : PersistentSingleton<ParticleManager>
{
    [SerializeField] private ParticleObjectScriptable particleObjectScriptable;

    private GameObject particleEffect;

    public GameObject CreateParticle(string particleKey)
    {
        particleEffect = Instantiate(particleObjectScriptable.GetParticleObjectByKey(particleKey), transform);       
        return particleEffect;
    }

    public void DestroyParticle()
    {
        Destroy(particleEffect);
    }
}
