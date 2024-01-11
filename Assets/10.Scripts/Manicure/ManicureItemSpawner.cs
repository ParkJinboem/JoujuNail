using System.Collections.Generic;
using UnityEngine;

public class ManicureItemSpawner : MonoBehaviour
{
    [SerializeField] private ManicurePool manicurePool;

    private List<ManicureConverter> manicureConverters;
    public List<ManicureConverter> ManicureConverters
    {
        get { return manicureConverters; }
    }

    public void Init(Transform itemParent, List<ManicureInfoData> manicureInfoDatas)
    {
        manicureConverters = new List<ManicureConverter>();
        CreateManicure(itemParent, manicureInfoDatas);
    }

    private void CreateManicure(Transform itemParent, List<ManicureInfoData> manicureInfoDatas)
    {
        for(int i = 0; i < manicureInfoDatas.Count; i++)
        {
            ManicureSpawn(itemParent, manicureInfoDatas[i]);
        }
    }

    private void ManicureSpawn(Transform itemParent, ManicureInfoData manicureInfoData)
    {
        ManicureConverter manicureConverter = manicurePool.Pop(itemParent.position).instance.GetComponent<ManicureConverter>();
        manicureConverter.Init(manicureInfoData);
        manicureConverters.Add(manicureConverter);
    }
}
