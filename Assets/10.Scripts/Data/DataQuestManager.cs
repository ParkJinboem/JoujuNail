using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


public partial class DataManager : MonoSingleton<DataManager>
{
    private List<QuestHandData> questDatas;
    public List<QuestHandData> QuestDatas
    {
        get { return questDatas; }
    }

    private List<QuestHandData> questHandDatas;
    public List<QuestHandData> QuestHandDatas
    {
        get { return questHandDatas; }
    }

    private List<QuestFingerData> questFingerDatas;
    public List<QuestFingerData> QuestFingerDatas
    {
        get { return questFingerDatas; }
    }

    private List<QuestPatternData> questPatternDatas;
    public List<QuestPatternData> QuestPatternDatas
    {
        get { return questPatternDatas; }
    }

    private List<QuestStickerData> questStickerDatas;
    public List<QuestStickerData> QuestStickerDatas
    {
        get { return questStickerDatas; }
    }

    /// <summary>
    /// 데이터 셋팅
    /// </summary>
    public void InitQuestData()
    {
        questHandDatas = JsonConvert.DeserializeObject<List<QuestHandData>>(AddressableManager.Instance.GetAssetByKey<TextAsset>("QuestHand").ToString());
        questFingerDatas = JsonConvert.DeserializeObject<List<QuestFingerData>>(AddressableManager.Instance.GetAssetByKey<TextAsset>("QuestFinger").ToString());
        questPatternDatas = JsonConvert.DeserializeObject<List<QuestPatternData>>(AddressableManager.Instance.GetAssetByKey<TextAsset>("QuestPattern").ToString());
        questStickerDatas = JsonConvert.DeserializeObject<List<QuestStickerData>>(AddressableManager.Instance.GetAssetByKey<TextAsset>("QuestSticker").ToString());
        GetQuestData();
    }

    /// <summary>
    /// 현재 퀘스트 반환
    /// </summary>
    private void GetQuestData()
    {
        questDatas = new List<QuestHandData>();

        foreach (var item in questHandDatas)
        {
            //아이템 안에 배경 정보 및 레벨 정보가 들어있음
            List<QuestFingerData> questFingerDataLevel = questFingerDatas.FindAll(x => x.level == item.level);
            List<QuestPatternData> questPatternDataLevel = questPatternDatas.FindAll(x => x.level == item.level);
            List<QuestStickerData> questStickerDataLevel = questStickerDatas.FindAll(x => x.level == item.level);

            //레벨, 배경이름을 저장
            item.selectType = System.Enum.Parse<SelectType>(item.selectType.ToString());
            item.questFingerDatas = questFingerDataLevel;
            for (int i = 0; i < item.questFingerDatas.Count; i++)
            {            
                item.questFingerDatas[i].questPatternData = questPatternDataLevel[i];

                for (int j = 0; j < questStickerDataLevel.FindAll(x=> x.finger == i).Count; j++)
                {
                    item.questFingerDatas[i].questStickerDatas = questStickerDataLevel.FindAll(x => x.finger == i);
                    item.questFingerDatas[i].questStickerDatas[j].stickerType = System.Enum.Parse<StickerType>(item.questFingerDatas[i].questStickerDatas[j].type);
                }
            }

            questDatas.Add(item);
        }       
    }

    /// <summary>
    /// 레벨에 맞는 퀘스트핸드의 모든정보를 가져옴
    /// </summary>
    public QuestHandData GetQuestHandDataWithLevel(int level)
    {
        QuestHandData questHandData = questDatas.Find(x => x.level == level);
        return questHandData;
    }

    /// <summary>
    /// 레, 순서, 타입별 퀘스트스티커 데이터 반환
    /// </summary>
    /// <param name="level"></param>
    /// <param name="finger"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<QuestStickerData> GetQuestStickerDataWithType(int level, int finger, StickerType type)
    {
        return questStickerDatas.FindAll(x => x.level == level).FindAll(x => x.finger == finger).FindAll(x => x.stickerType == type);
    }
}