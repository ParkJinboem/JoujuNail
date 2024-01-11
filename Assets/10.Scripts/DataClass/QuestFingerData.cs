using System;
using System.Collections.Generic;

[Serializable]
public class QuestFingerData
{
    public int level;
    public int finger;
    public string nailMaskString;
    public string nailCoverString;
    public string manicureName;
    public string baseColor;
    public string mode;
    public QuestPatternData questPatternData;
    public List<QuestStickerData> questStickerDatas;
}