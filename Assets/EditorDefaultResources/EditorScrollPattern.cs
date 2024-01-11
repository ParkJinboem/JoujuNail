using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorScrollPattern : MonoBehaviour
{
    public List<ManicureInfoData> manicureDatas;
    public GameObject itemPrefabs;
    public Transform itemParent;

    // Start is called before the first frame update
    void Start()
    {
        manicureDatas = DataManager.Instance.GetManicureInfoDataByType("Pattern");
        CreateItem();
    }

    public void CreateItem()
    {
        for (int i = 0; i < manicureDatas.Count; i++)
        {
            GameObject item = Instantiate(itemPrefabs, itemParent);
            item.GetComponent<EditorManicureItem>().Init(manicureDatas[i]);
        }
    }
}
