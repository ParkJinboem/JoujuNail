using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HandFootTexture
{
    public int level;
    public SelectType selectType;
    public List<Texture2D> texture2Ds;
    public List<byte[]> textureBytes;
}

public class HandFootTextureMaker : MonoSingleton<HandFootTextureMaker>
{
    private QuestHandData questHandData;

    public HandFootTexture handFootTexture;

    /// <summary>
    /// 초기값 설정
    /// </summary>
    public void Init()
    {
        int level = PlayerDataManager.Instance.GetQuest().level;
        questHandData = DataManager.Instance.GetQuestHandDataWithLevel(level);
        handFootTexture = new HandFootTexture();
        SetGuidePaint();
    }

    /// <summary>
    /// 가이드 손톱 설정
    /// </summary>
    private void SetGuidePaint()
    {
        HandFootTexture handFootTexture = new HandFootTexture();
        handFootTexture.level = questHandData.level;
        handFootTexture.selectType = questHandData.selectType;
        handFootTexture.textureBytes = new List<byte[]>();

        handFootTexture.texture2Ds = new List<Texture2D>();
        for (int k = 0; k < 5; k++)
        {
            Sprite sprite = DataManager.Instance.GetManicureItemSprite(questHandData.questFingerDatas[k].manicureName);
            Texture2D texture2D = PaintUtils.TextureFromSprite(sprite);
            QuestFingerData questFingerData = questHandData.questFingerDatas[k];
            if (questFingerData.mode == "Cream")
            {
                if (questHandData.selectType == SelectType.Hand)
                {
                    Color color = new Color();
                    UnityEngine.ColorUtility.TryParseHtmlString(questFingerData.baseColor, out color);
                    Texture2D tex = AutoDrawing.SetDrawingTexture(color, questHandData.selectType, Statics.fingerScales[k]);
                    handFootTexture.texture2Ds.Add(tex);
                }
                else if (questHandData.selectType == SelectType.Foot)
                {
                    Color color = new Color();
                    UnityEngine.ColorUtility.TryParseHtmlString(questFingerData.baseColor, out color);
                    Texture2D tex = AutoDrawing.SetDrawingTexture(color, questHandData.selectType, Statics.toesScales[k]);
                    handFootTexture.texture2Ds.Add(tex);
                }
            }
            else if (questHandData.questFingerDatas[k].mode == "PearlRainbow")
            {
                if (questHandData.selectType == SelectType.Hand)
                {
                    Texture2D tex = AutoDrawing.SetDrawingPatternTexture(texture2D, questHandData.selectType, Statics.fingerScales[k]);
                    handFootTexture.texture2Ds.Add(tex);
                }
                else if (questHandData.selectType == SelectType.Foot)
                {
                    Texture2D tex = AutoDrawing.SetDrawingPatternTexture(texture2D, questHandData.selectType, Statics.toesScales[k]);
                    handFootTexture.texture2Ds.Add(tex);
                }
            }
            else if (questHandData.questFingerDatas[k].mode == "Glittery")
            {
                if (questHandData.selectType == SelectType.Hand)
                {
                    Texture2D tex = AutoDrawing.SetDrawingGlitteryTexture(questHandData.questFingerDatas[k].manicureName, questHandData.selectType, Statics.fingerScales[k], Statics.fingerAutoDrawingCount[k], Statics.fingerAutoDrawingRatio[k]);
                    handFootTexture.texture2Ds.Add(tex);
                }
                else if (questHandData.selectType == SelectType.Foot)
                {
                    Texture2D tex = AutoDrawing.SetDrawingGlitteryTexture(questHandData.questFingerDatas[k].manicureName, questHandData.selectType, Statics.toesScales[k], Statics.toesAutoDrawingCount[k], Statics.toesAutoDrawingRatio[k]);
                    handFootTexture.texture2Ds.Add(tex);
                }
            }
        }
        this.handFootTexture = handFootTexture;
    }

    public Texture2D GetTexture2D(int level, SelectType selectType, int index)
    {
        Texture2D texture2D = null;
        if (handFootTexture.level == level && handFootTexture.selectType == selectType)
        {
            texture2D = handFootTexture.texture2Ds[index];
        }
        return texture2D;
    }
}
