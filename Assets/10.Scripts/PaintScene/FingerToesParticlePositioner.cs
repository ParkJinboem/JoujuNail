using System.Collections.Generic;
using UnityEngine;

public class FingerToesParticlePositioner : MonoBehaviour
{
    [SerializeField] private List<Transform> fingerParticleParents;
    [SerializeField] private List<Transform> toesParticleParents;

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject startParticle;
    [SerializeField] private GameObject clearParticle;

    private List<GameObject> particles;

    /// <summary>
    /// 초기값 설정
    /// </summary>
    public void Init()
    {
        particles = new List<GameObject>();
        //파티클 생성
        if (Statics.selectType == SelectType.Hand)
        {
            for (int i = 0; i < fingerParticleParents.Count; i++)
            {
                GameObject particle = Instantiate(startParticle, fingerParticleParents[i].transform.position, Quaternion.identity);
                particle.transform.SetParent(parent);
                particles.Add(particle);
            }
        }
        else
        {
            for (int i = 0; i < toesParticleParents.Count; i++)
            {
                GameObject particle = Instantiate(startParticle, toesParticleParents[i].transform.position, Quaternion.identity);
                //particle.transform.SetParent(parent);
                particle.transform.SetParent(toesParticleParents[i].transform);
                particle.GetComponent<Coffee.UIExtensions.UIParticle>().scale = 100;
                particles.Add(particle);
            }
        }
    }

    /// <summary>
    /// startParticle오브젝트 제거
    /// </summary>
    public void DestroyParticle()
    {
        for(int i = 0; i < particles.Count; i++)
        {
            Destroy(particles[i]);
        }
        particles = new List<GameObject>();
    }

    /// <summary>
    /// startParticle오브젝트 활성화 여부
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isAll"></param>
    /// <param name="isActive"></param>
    public void ActiveOnOffParticle(int index, bool isAll, bool isActive)
    {
        if (isAll == true)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].SetActive(isActive);
            }
        }
        else
        {
            particles[index].SetActive(isActive);
        }
    }

    GameObject particle;
    public void CreateCelearParticle(int index)
    {
        if (Statics.selectType == SelectType.Hand)
        {
            particle = Instantiate(clearParticle, fingerParticleParents[index].transform.position, Quaternion.identity);
            particle.transform.SetParent(parent);
        }
        else
        {
            particle = Instantiate(clearParticle, toesParticleParents[index].transform.position, Quaternion.identity);
            particle.transform.SetParent(parent);
        }
    }
}