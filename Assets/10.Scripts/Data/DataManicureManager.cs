using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.U2D;

public partial class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] private SpriteAtlas manicureAtlas;
    [SerializeField] private SpriteAtlas manicureBottleAtlas;
    //사용하지않음_240108_박진범
    //[SerializeField] private SpriteAtlas brushAtlas;
    [SerializeField] private SpriteAtlas brushIconAtlas;

    private List<ManicureData> manicureDatas;
    public List<ManicureData> ManicureDatas
    {
        get { return manicureDatas; }
    }

    private List<ManicureInfoData> manicureInfoDatas;
    public List<ManicureInfoData> ManicureInfoDatas
    {
        get { return manicureInfoDatas; }
    }

    /// <summary>
    /// 데이터 셋팅
    /// </summary>
    public void InitManicureData()
    {
        manicureDatas = JsonConvert.DeserializeObject<List<ManicureData>>(AddressableManager.Instance.GetAssetByKey<TextAsset>("Manicure").ToString());
        GetManicureInfoData();
    }

    /// <summary>
    /// 전체 메니큐어 데이터 반환 
    /// </summary>
    private void GetManicureInfoData()
    {
        manicureInfoDatas = new List<ManicureInfoData>();
        foreach (var item in manicureDatas)
        {
            if (Enum.TryParse<DrawMode>(item.mode, out var manicureItemMode))
            {
                ManicureInfoData data = new ManicureInfoData();
                data.id = item.id;
                data.type = item.type;
                ColorUtility.TryParseHtmlString(item.baseColor, out data.baseColor);
                data.drawMode = manicureItemMode;
                data.spriteName = item.spriteName;
                data.useOnSpriteName = item.useOnSpriteName;
                data.useOffSpriteName = item.useOffSpriteName;
                data.bigBrushSpriteName = item.bigBrushSpriteName;
                data.smallBrushSpriteName = item.smallBrushSpriteName;
                data.option = item.option;
                manicureInfoDatas.Add(data);
            }
        }
    }

    /// <summary>
    /// 타입별 메니큐어데이터 반환 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<ManicureInfoData> GetManicureInfoDataByType(string type)
    {
        return manicureInfoDatas.FindAll(x => x.type == type);
    }

    /// <summary>
    /// 이름별 메니쿠어데이터 반환
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public ManicureInfoData GetManicureInfoDataByName(string name)
    {
        ManicureInfoData manicureInfoData = manicureInfoDatas.Find(x => x.spriteName == name);
        return manicureInfoData;
    }

    /// <summary>
    /// 콜렉션넘버별 메니큐어데이터 반환 
    /// </summary>
    /// <param name="collectionNumber"></param>
    /// <returns></returns>
    public List<ManicureData> GetManicureDatasBycollectionNumber(int collectionNumber)
    {
        List<ManicureData> manicureDatas = this.manicureDatas.FindAll(x => x.collectionNumber == collectionNumber);
        return manicureDatas;
    }

    /// <summary>
    /// 매니큐어 아이디로 데이터 반환 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ManicureData GetManicureDataById(int id)
    {
        ManicureData manicureData = manicureDatas.Find(x => x.id == id);
        return manicureData;
    }

    /// <summary>
    /// 이름별 메니큐어데이터 반환
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public ManicureData GetManicureDataByName(string name)
    {
        ManicureData manicureData = manicureDatas.Find(x => x.spriteName == name);
        return manicureData;
    }

    public Sprite GetManicureItemSprite(string manicureItemName)
    {
        Sprite sprite = manicureAtlas.GetSprite(manicureItemName);
        return sprite;
    }

    public Sprite GetManicureBottleItemSprite(string manicureOnOffName)
    {
        Sprite sprite = manicureBottleAtlas.GetSprite(manicureOnOffName);
        return sprite;
    }

    public Sprite GetBigBrushIconItemSprite(string brushName)
    {
        Sprite sprite = brushIconAtlas.GetSprite(brushName);
        return sprite;
    }

    public Sprite GetSmallBrushIconItemSprite(string brushName)
    {
        Sprite sprite = brushIconAtlas.GetSprite(brushName);
        return sprite;
    }

    //사용하지않음_240108_박진범
    //public Sprite GetBigBrushItemSprite(string brushName)
    //{
    //    Sprite sprite = brushAtlas.GetSprite(brushName);
    //    return sprite;
    //}

    //사용하지않음_240108_박진범
    //public Sprite GetSmallBrushItemSprite(string brushName)
    //{
    //    Sprite sprite = brushAtlas.GetSprite(brushName);
    //    return sprite;
    //}
}
