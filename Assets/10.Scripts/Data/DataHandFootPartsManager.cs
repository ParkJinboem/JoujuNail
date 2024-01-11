using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.U2D;

public partial class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] private SpriteAtlas handAtlas;
    [SerializeField] private SpriteAtlas footAtlas;
    [SerializeField] private SpriteAtlas fingerAtlas;
    [SerializeField] private SpriteAtlas toesAtlas;
    [SerializeField] private SpriteAtlas fingerNailMaskAtlas;
    [SerializeField] private SpriteAtlas toesNailMaskAtlas;
    [SerializeField] private SpriteAtlas fingerNailCoverAtlas;
    [SerializeField] private SpriteAtlas toesNailCoverAtlas;
    [SerializeField] private SpriteAtlas bgAtlas;

    private List<HandFootPartsData> handFootPartsDatas;
    public List<HandFootPartsData> HandFootPartsDatas
    {
        get { return handFootPartsDatas; }
    }

    /// <summary>
    /// 데이터 셋팅
    /// </summary>
    public void InitHandFootPartsData()
    {
        handFootPartsDatas = JsonConvert.DeserializeObject<List<HandFootPartsData>>(AddressableManager.Instance.GetAssetByKey<TextAsset>("HandFootParts").ToString());
    }

    /// <summary>
    /// 해당 아이디를 가진 HandFootPartsData 반환
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public HandFootPartsData GetHandFootPartsDataWithId(int id)
    {
        HandFootPartsData handFootPartsData = handFootPartsDatas.Find(x => x.id == id);
        return handFootPartsData;
    }

    /// <summary>
    /// 해당 타입을 가진 HandFootPartsData 반환
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<HandFootPartsData> GetHandFootPartsDataWithType(int type)
    {
        List<HandFootPartsData> handFootPartsDatas = this.handFootPartsDatas.FindAll(x => x.type == type);
        return handFootPartsDatas;
    }

    /// <summary>
    /// 해당 타입과 아이디를 가진 HandFootPartsData 반환
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public HandFootPartsData GetHandFootPartsDataByTypeByid(int type, int id)
    {
        HandFootPartsData handFootPartsData = handFootPartsDatas.FindAll(x => x.type == type).Find(x => x.id == id);
        return handFootPartsData;
    }

    public Sprite GetHandSprite(string handName)
    {
        Sprite sprite = handAtlas.GetSprite(handName);
        return sprite;
    }

    public Sprite GetFootSprite(string handName)
    {
        Sprite sprite = footAtlas.GetSprite(handName);
        return sprite;
    }

    public Sprite GetFingerSprite(string handName)
    {
        Sprite sprite = fingerAtlas.GetSprite(handName);
        return sprite;
    }

    public Sprite GetToesSprite(string handName)
    {
        Sprite sprite = toesAtlas.GetSprite(handName);
        return sprite;
    }

    public Sprite GetFingerNailMaskSprite(string fingerNailMaskName)
    {
        Sprite sprite = fingerNailMaskAtlas.GetSprite(fingerNailMaskName);
        return sprite;
    }

    public Sprite GetToesNailMaskSprite(string toesNailMaskName)
    {
        Sprite sprite = toesNailMaskAtlas.GetSprite(toesNailMaskName);
        return sprite;
    }

    public Sprite GetFingerNailCoverSprite(string fingerNailCoverName)
    {
        Sprite sprite = fingerNailCoverAtlas.GetSprite(fingerNailCoverName);
        return sprite;
    }

    public Sprite GetToesNailCoverSprite(string toesNailCoverName)
    {
        Sprite sprite = toesNailCoverAtlas.GetSprite(toesNailCoverName);
        return sprite;
    }

    public Sprite GetBgSprite(string bgName)
    {
        Sprite sprite = bgAtlas.GetSprite(bgName);
        return sprite;
    }
}
