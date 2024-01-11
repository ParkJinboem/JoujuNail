using System.Collections;
using System.Collections.Generic;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.UI;


public class RecyclableScroller : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField] RecyclableScrollRect recyclableScrollRect;
    [SerializeField] private int dataLength;
    public List<ManicureInfoData> manicureInfoDatas;
    public GameObject content;
    int cycleNumber;
    public Color patternColor;

    private void Awake()
    {
        recyclableScrollRect.DataSource = this;
        InitData();
    }

    /// <summary>
    /// 스크롤바 생성시 데이터를 미리 넣어둔다
    /// </summary>
    public void InitData()
    {
        //매니큐어 목록 초기화
        if (manicureInfoDatas != null)
        {
            manicureInfoDatas.Clear();
        }

        manicureInfoDatas = DataManager.Instance.GetManicureInfoDataByType("Manicure");
        //설정한 데이터의 갯수만큼 매니큐어 리스트에서 데이터를 가져옴
        for (int i = 0; i < dataLength; i++)
        {
            if (cycleNumber > manicureInfoDatas.Count - 1)
            {
                cycleNumber = 0;
            }
            manicureInfoDatas.Add(manicureInfoDatas[cycleNumber]);
            cycleNumber++;
        }
    }


    /// <summary>
    /// 매니큐어 바텀 아이템 리스트를 교체해줌
    /// </summary>
    public void ResetContent(string itemType)
    {
        //for (int i = 0; i < content.transform.childCount; i++)
        //{
        //    Destroy(content.transform.GetChild(i).gameObject);
        //}

        //매니큐어 목록 초기화
        if (manicureInfoDatas != null)
        {
            manicureInfoDatas.Clear();
        }

        manicureInfoDatas = DataManager.Instance.GetManicureInfoDataByType(itemType);
        //설정한 데이터의 갯수만큼 매니큐어 리스트에서 데이터를 가져옴
        for (int i = 0; i < dataLength; i++)
        {
            if (cycleNumber > manicureInfoDatas.Count - 1)
            {
                cycleNumber = 0;
            }
            manicureInfoDatas.Add(manicureInfoDatas[cycleNumber]);
            cycleNumber++;
        }
        recyclableScrollRect.ReloadData();
    }

    public void ChagneContentColor(Color color)
    {
        patternColor = color;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            content.transform.GetChild(i).GetComponent<Image>().color = color;
        }
    }

    /// <summary>
    /// 매니큐어 데이터의 갯수를 리턴시켜줌
    /// </summary>
    public int GetItemCount()
    {
        return manicureInfoDatas.Count;
    }

    /// <summary>
    /// 매니큐어 아이템의 정보를 갱신 시켜줌(처음 생성 및 스크롤 작동시)
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        ManicureConverter manicureConverter = cell as ManicureConverter;
        Image image = manicureConverter.GetComponent<Image>();
        image.color = patternColor;
        //item.ConfigureCell(manicureList[index], index);
    }   
}
