using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ParticleObjectScriptable", order = 1)]
public class ParticleObjectScriptable : ScriptableObject
{
    [SerializeField] private List<GameObject> particles = new List<GameObject>();
    public GameObject GetParticleObjectByKey(string key)
    {
        return particles.Find(x => x.name == key);
    }
}
