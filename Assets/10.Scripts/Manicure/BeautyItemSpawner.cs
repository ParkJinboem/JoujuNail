using System.Collections.Generic;
using UnityEngine;

public class BeautyItemSpawner : MonoBehaviour
{
    private PatternPool patternPool;
    public PatternPool PatternPool
    {
        get { return patternPool; }
    }
    private StickerPool stickerPool;
    public StickerPool StickerPool
    {
        get { return stickerPool; }
    }
    private GuideStickerPool guideStickerPool;
    public GuideStickerPool GuideStickerPool
    {
        get { return guideStickerPool; }
    }

    GuideStickerConverter guideStickerConverter;
    GameObject guideStickerObject;

    /// <summary>
    /// 패턴 생성 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="sprite"></param>
    /// <param name="patternColor"></param>
    /// <param name="scale"></param>
    /// <param name="questable"></param>
    /// <param name="freeable"></param>
    public void CreatePattern(Transform parent, Sprite sprite, Color patternColor, Vector3 scale, IQuestable questable, IFreeable freeable)
    {
        PatternPool patternPool = parent.GetComponent<PatternPool>();
        this.patternPool = patternPool;

        PatternSpawn(parent, sprite, patternColor, scale, questable, freeable);
    }

    public void PatternSpawn(Transform parent, Sprite sprite, Color patternColor, Vector3 scale, IQuestable questable, IFreeable freeable)
    {
        PatternConverter patternConverter = patternPool.Pop(parent.position).instance.GetComponent<PatternConverter>();
        if(Statics.gameMode == GameMode.Quest)
        {
            questable.PatternItems[questable.SelectFingerIndex].patternConverter = patternConverter;
            questable.PatternClone = patternConverter.gameObject;
        }
        else
        {
            freeable.PatternItems[freeable.SelectFingerIndex].patternConverter = patternConverter;
            freeable.PatternClone = patternConverter.gameObject;
        }
        patternConverter.Init(sprite, patternColor, scale);
    }

    /// <summary>
    /// 스티커 생성
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="advancedMobilePaint"></param>
    /// <param name="manicureInfoData"></param>
    /// <param name="questable"></param>
    /// <param name="freeable"></param>
    public void CreateSticker(Transform parent, AdvancedMobilePaint advancedMobilePaint, ManicureInfoData manicureInfoData, IQuestable questable, IFreeable freeable)
    {
        StickerPool stickerPool = parent.GetComponent<StickerPool>();
        this.stickerPool = stickerPool;

        StickerSpawn(parent, advancedMobilePaint, manicureInfoData, questable, freeable);
    }

    public void StickerSpawn(Transform parent, AdvancedMobilePaint advancedMobilePaint, ManicureInfoData manicureInfoData, IQuestable questable, IFreeable freeable)
    {
        StickerConverter stickerConverter = stickerPool.Pop(parent.position).instance.GetComponent<StickerConverter>();
        stickerConverter.Init(advancedMobilePaint, manicureInfoData, questable, freeable);
        if (Statics.gameMode == GameMode.Quest)
        {
            if (manicureInfoData.type == "CharacterSticker")
            {
                questable.CharacterStickerItems[questable.SelectFingerIndex].stickerConverters.Add(stickerConverter);
            }
            else if (manicureInfoData.type == "Sticker")
            {
                questable.StickerItems[questable.SelectFingerIndex].stickerConverters.Add(stickerConverter);
            }
            else if (manicureInfoData.type == "AnimationSticker")
            {
                questable.AnimationStickerItems[questable.SelectFingerIndex].stickerConverters.Add(stickerConverter);
            }
        }
        else
        {
            if (manicureInfoData.type == "CharacterSticker")
            {
                freeable.CharacterStickerItems[freeable.SelectFingerIndex].stickerConverters.Add(stickerConverter);
            }
            else if (manicureInfoData.type == "Sticker")
            {
                freeable.StickerItems[freeable.SelectFingerIndex].stickerConverters.Add(stickerConverter);
            }
            else if (manicureInfoData.type == "AnimationSticker")
            {
                freeable.AnimationStickerItems[freeable.SelectFingerIndex].stickerConverters.Add(stickerConverter);
            }
        }
    }

    /// <summary>
    /// 가이드 스티커 생성
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="questStickerDatas"></param>
    /// <param name="questable"></param>
    public void CreateGuideSticker(Transform parent, List<QuestStickerData> questStickerDatas, IQuestable questable)
    {
        GuideStickerPool guideStickerPool = parent.GetComponent<GuideStickerPool>();
        this.guideStickerPool = guideStickerPool;

        for(int i = 0; i < questStickerDatas.Count; i++)
        {
            GuideStickerSpawn(parent, questStickerDatas[i], questable);
        }
    }

    //윤정덕 자료
    //public void GuideStickerSpawn(Transform parent, QuestStickerData questStickerData, IQuestable questable)
    //{
    //    GuideStickerConverter guideStickerConverter = guideStickerPool.Pop(parent.position).instance.GetComponent<GuideStickerConverter>();
    //    guideStickerConverter.Init(questStickerData);
    //    questable.GuideStickerObjects.Add(guideStickerConverter.gameObject);
    //}

    //위의 윤정덕 자료에서 수정함_231101 박진범
    public void GuideStickerSpawn(Transform parent, QuestStickerData questStickerData, IQuestable questable)
    {

        ManicureData manicureData = DataManager.Instance.GetManicureDataById(questStickerData.id);
        if(questStickerData.stickerType != StickerType.AnimationSticker)
        { 
            GuideStickerConverter guideStickerConverter = guideStickerPool.Pop(parent.position).instance.GetComponent<GuideStickerConverter>();
            guideStickerConverter.Init(questStickerData);
            questable.GuideStickerObjects.Add(guideStickerConverter.gameObject);
        }
        else
        {
            GameObject guideStickerPrefab = AddressableManager.Instance.GetAssetByKey<GameObject>(manicureData.option);
            guideStickerObject = Instantiate(guideStickerPrefab, parent);
            guideStickerConverter = guideStickerObject.GetComponent<GuideStickerConverter>();
            guideStickerConverter.Init(questStickerData);
            questable.GuideStickerObjects.Add(guideStickerConverter.gameObject);
        }

    }
}