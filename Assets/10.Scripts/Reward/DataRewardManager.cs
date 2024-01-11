using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private List<RewardData> rewardDatas;
    public List<RewardData> RewardDatas
    {
        get { return rewardDatas; }
    }

    private List<RewardData> rewardInfoDatas;
    public List<RewardData> RewardInfoDatas
    {
        get { return rewardInfoDatas; }
    }

    public void InitRewardData()
    {
        rewardDatas = JsonConvert.DeserializeObject<List<RewardData>>(AddressableManager.Instance.GetAssetByKey<TextAsset>("Reward").ToString());
        GetRewardInfoData();
    }

    public void GetRewardInfoData()
    {
        rewardInfoDatas = new List<RewardData>();
        foreach (var item in rewardDatas)
        {
            RewardData data = new RewardData();
            data.id = item.id;
            data.spriteName = item.spriteName;
            data.level = item.level;
            rewardInfoDatas.Add(data);
        }
    }

    public List<RewardData> GetRewardInfoDatabyLevel(int level)
    {
        return rewardInfoDatas.FindAll(x => x.level == level);
    }

}
