using System;
using System.Collections.Generic;

[Serializable]
public class QuestHandData
{
    public int level;
    public string bgSpriteString;
    public SelectType selectType;
    public List<QuestFingerData> questFingerDatas;
}