using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionSpawner : MonoBehaviour
{
    public delegate void TriggerDownHandler(CollectionItem collectionItem);
    public static event TriggerDownHandler TriggerDownEvent;

    public GiftBoxPool giftBoxPool;
    public CollectionPool collectionPool;

    /// <summary>
    /// 콜렉션상자 생성
    /// </summary>
    public void CreateGiftBox()
    {
        GiftBoxSpawn();
    }

    public void GiftBoxSpawn()
    {
        GiftBoxItem giftBoxItem = giftBoxPool.Pop(transform.position).instance.GetComponent<GiftBoxItem>();
        giftBoxItem.Init();
        Button button = giftBoxItem.GetComponent<Button>();
        CollectionManager.Instance.GiftBoxBtn = button;
    }

    /// <summary>
    /// 콜렉션아이템 생성
    /// </summary>
    /// <param name="indexs"></param>
    public void CreateCollection(List<int> indexs)
    {
        for(int i = 0; i < indexs.Count; i++)
        {
            ManicureData manicureData = DataManager.Instance.GetManicureDataById(indexs[i]);
            CollectionSpawn(manicureData);
        }
    }

    public void CollectionSpawn(ManicureData manicureData)
    {
        CollectionItem collectionItem = collectionPool.Pop(transform.position).instance.GetComponent<CollectionItem>();
        collectionItem.Init(manicureData);
        CollectionManager.Instance.CollectionItems.Add(collectionItem);

        TriggerDownEvent?.Invoke(collectionItem);
    }
}