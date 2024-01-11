using System.Collections.Generic;
using UnityEngine;

public class CollectionController : MonoBehaviour
{
    [SerializeField] private GameObject collectionGroup;

    public List<ManicureData> manicureDatas;

    [SerializeField] private List<CollectionBehaviour> collectionBehaviours;
    public List<CollectionBehaviour> CollectionBehaviours
    {
        get { return collectionBehaviours; }
    }

    public int pageNumber;
    public int collectionNumber;

    /// <summary>
    /// 초기값 설정
    /// </summary>
    public void Init()
    {
        collectionBehaviours = new List<CollectionBehaviour>();
        collectionBehaviours = GetChildrens();

        manicureDatas = new List<ManicureData>();
        manicureDatas = DataManager.Instance.GetManicureDatasBycollectionNumber(collectionNumber);

        for(int i = 0; i < manicureDatas.Count; i++)
        {
            collectionBehaviours[i].ManicureData = manicureDatas[i];

            bool isHave = PlayerDataManager.Instance.IsHaveCollection(manicureDatas[i].id);
            if (isHave)
            {
                collectionBehaviours[i].Init(true, manicureDatas[i].baseColor);
            }
            else
            {
                collectionBehaviours[i].Init(false, manicureDatas[i].baseColor);
            }
        }
    }

    public CollectionBehaviour GetCollectionBehaviour(int id)
    {
        CollectionBehaviour collectionBehaviour = collectionBehaviours.Find(x => x.ManicureData.id == id);
        return collectionBehaviour;
    }

    private List<CollectionBehaviour> GetChildrens()
    {
        List<CollectionBehaviour> collectionBehaviours = new List<CollectionBehaviour>();
        for (int i = 0; i < collectionGroup.transform.childCount; i++)
        {
            if (collectionGroup.transform.GetChild(i).GetComponent<CollectionBehaviour>() != null)
            {
                collectionBehaviours.Add(collectionGroup.transform.GetChild(i).GetComponent<CollectionBehaviour>());
            }
        }
        return collectionBehaviours;
    }
}