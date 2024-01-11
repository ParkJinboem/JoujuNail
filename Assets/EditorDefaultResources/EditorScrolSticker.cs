using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorScrolSticker : MonoBehaviour
{
    public List<ManicureInfoData> manicureDatas;
    public List<ManicureInfoData> manicureDatas2;
    public List<ManicureInfoData> AllManicureDatas;
    public GameObject itemPrefabs;
    public Transform itemParent;

    // Start is called before the first frame update
    void Start()
    {
        manicureDatas = DataManager.Instance.GetManicureInfoDataByType("Sticker");
        manicureDatas2 = DataManager.Instance.GetManicureInfoDataByType("AnimationSticker");
        AllManicureDatas.AddRange(manicureDatas);
        AllManicureDatas.AddRange(manicureDatas2);
        CreateItem();
    }

    public void CreateItem()
    {
        for (int i = 0; i < AllManicureDatas.Count; i++)
        {
            GameObject item = Instantiate(itemPrefabs, itemParent);
            item.GetComponent<EditorManicureItem>().Init(AllManicureDatas[i]);
        }
    }
}
