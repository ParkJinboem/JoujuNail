using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EditorHandFootTexture
{
    public int level;
    public SelectType selectType;
    public List<Texture2D> texture2Ds;
    public List<byte[]> textureBytes;
}

public class EditorAlbumData : MonoSingleton<EditorAlbumData>
{
    private QuestHandData questHandData;

    public List<EditorHandFootTexture> handFootTexture;

    /// <summary>
    /// 초기값 설정
    /// </summary>
    public void Init(int level)
    {
        questHandData = DataManager.Instance.GetQuestHandDataWithLevel(level);
        //handFootTexture = new List<EditorHandFootTexture>();
        SetGuidePaint();
    }

    /// <summary>
    /// 가이드 손톱 설정
    /// </summary>
    private void SetGuidePaint()
    {
        EditorHandFootTexture editorhandFootTexture = new EditorHandFootTexture();
        editorhandFootTexture.level = questHandData.level;
        editorhandFootTexture.selectType = questHandData.selectType;
        editorhandFootTexture.textureBytes = new List<byte[]>();

        editorhandFootTexture.texture2Ds = new List<Texture2D>();
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
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
                else if (questHandData.selectType == SelectType.Foot)
                {
                    Color color = new Color();
                    UnityEngine.ColorUtility.TryParseHtmlString(questFingerData.baseColor, out color);
                    Texture2D tex = AutoDrawing.SetDrawingTexture(color, questHandData.selectType, Statics.toesScales[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
            }
            else if (questHandData.questFingerDatas[k].mode == "PearlRainbow")
            {
                if (questHandData.selectType == SelectType.Hand)
                {
                    Texture2D tex = AutoDrawing.SetDrawingPatternTexture(texture2D, questHandData.selectType, Statics.fingerScales[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
                else if (questHandData.selectType == SelectType.Foot)
                {
                    Texture2D tex = AutoDrawing.SetDrawingPatternTexture(texture2D, questHandData.selectType, Statics.toesScales[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
            }
            else if (questHandData.questFingerDatas[k].mode == "Glittery")
            {
                if (questHandData.selectType == SelectType.Hand)
                {
                    Texture2D tex = AutoDrawing.SetDrawingGlitteryTexture(questHandData.questFingerDatas[k].manicureName, questHandData.selectType, Statics.fingerScales[k], Statics.fingerAutoDrawingCount[k], Statics.fingerAutoDrawingRatio[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
                else if (questHandData.selectType == SelectType.Foot)
                {
                    Texture2D tex = AutoDrawing.SetDrawingGlitteryTexture(questHandData.questFingerDatas[k].manicureName, questHandData.selectType, Statics.toesScales[k], Statics.toesAutoDrawingCount[k], Statics.toesAutoDrawingRatio[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
            }
        }
        handFootTexture.Add(editorhandFootTexture);
    }

    public Texture2D GetTexture2D(int level, SelectType selectType, int index)
    {
        EditorHandFootTexture levelTexture2Ds = null;
        Texture2D texture2D = null;
        //if (handFootTexture[level - 1].level == level && handFootTexture[level - 1].selectType == selectType)
        //{
        //    texture2D = handFootTexture[level - 1].texture2Ds[index];
        //}
        levelTexture2Ds = handFootTexture.Find(x => x.level == level);
        texture2D = levelTexture2Ds.texture2Ds[index];

        return texture2D;
    }
}
