using System.Collections.Generic;

public partial class PlayerDataManager : MonoSingleton<PlayerDataManager>
{
    /// <summary>
    /// Info 데이터 반환
    /// </summary>
    /// <returns></returns>
    public Info GetInfo()
    {
        return s.info;
    }

    public void SetCurManicreId(int manicureId)
    {
        s.info.curSelectedManicureId = manicureId;
        isInfoSave = true;
        SaveData();
    }

    public void SetHandImage()
    {
        if (s.info.handfootIndex == 0)
        {
            SaveInfo(1);
        }
    }

    /// <summary>
    /// Info 데이터 저장
    /// </summary>
    /// <param name="index"></param>
    public void SaveInfo(int index)
    {
        Info info = new Info()
        {
            handfootIndex = index,
            curSelectedManicureId = s.info.curSelectedManicureId,
            handImageName = "Hand_" + index.ToString("D3"),
            footImageName = "Foot_" + index.ToString("D3"),
            fingerImagenames = new List<string>(),
            toesImagenames = new List<string>()
        };
        
        for (int i = 0; i < 5; i++)
        {
            info.fingerImagenames.Add("indexFinger" + index + "_" + (i + 1).ToString("D3"));
            info.toesImagenames.Add("indexToes" + index + "_" + (i + 1).ToString("D3"));
        }

        s.info = info;

        isInfoSave = true;
        SaveData();
    }
}
