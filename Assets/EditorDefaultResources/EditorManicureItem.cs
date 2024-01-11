using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class EditorPaintHandFootTexture
{
    public SelectType selectType;
    public List<Texture2D> texture2Ds;
    public List<byte[]> textureBytes;
    public bool have;
}

public class EditorManicureItem : MonoBehaviour
{
    public EditorPaintHandFootTexture handFootTexture;
    public RectTransform rectTransform;
    public Image image;
    public DrawMode mode;
    public string itemType;
    public string itemName;
    public Color itemcolor;
    
   
    public void Init(ManicureInfoData manicureInfoData)
    {
        itemType = manicureInfoData.type;
        image.color = new Color(1, 1, 1, 1);
        mode = manicureInfoData.drawMode;
        itemName = manicureInfoData.spriteName;
        itemcolor = manicureInfoData.baseColor;
        handFootTexture.have = false;
        image.sprite = DataManager.Instance.GetManicureItemSprite(manicureInfoData.spriteName);

        if (itemType == "Manicure")
        {
            rectTransform.sizeDelta = Statics.manicureSize;
        }
        else if (itemType == "Pattern")
        {
            rectTransform.sizeDelta = Statics.patternSize;
            image.color = manicureInfoData.baseColor;
        }
        else
        {
            rectTransform.sizeDelta = Statics.stickerSize;
        }
    }

    public void ClickedItme()
    {
        switch(itemType)
        {
            case "Manicure":
                if(!handFootTexture.have)
                {
                    SetGuidePaint();
                    handFootTexture.have = true;
                }
                EditorPaintSceneManager.Instance.SetUpPaintTexture(handFootTexture, itemName);
                break;
            case "Pattern":
                EditorPaintSceneManager.Instance.SetUpPattern(image, itemName);
                break;
            case "CharacterSticker":
                EditorPaintSceneManager.Instance.SetUpCharacterSticker(image.sprite, itemName);
                break;
            case "Sticker":
                EditorPaintSceneManager.Instance.SetUpSticker(image.sprite, itemName);
                break;
            case "AnimationSticker":
                EditorPaintSceneManager.Instance.SetUpAnimSticker(image.sprite, itemName);
                break;
        }
    }


    private void SetGuidePaint()
    {
        EditorPaintHandFootTexture editorhandFootTexture = new EditorPaintHandFootTexture();
        editorhandFootTexture.selectType = selectedType(EditorPaintSceneManager.Instance.selectedNum);

        editorhandFootTexture.textureBytes = new List<byte[]>();
        editorhandFootTexture.texture2Ds = new List<Texture2D>();
        for (int k = 0; k < 5; k++)
        {
            Sprite sprite = DataManager.Instance.GetManicureItemSprite(itemName);
            Texture2D texture2D = PaintUtils.TextureFromSprite(sprite);
            //QuestFingerData questFingerData = questHandData.questFingerDatas[k];
            if (mode.ToString() == "Cream")
            {
                if (editorhandFootTexture.selectType == SelectType.Hand)
                {
                    Color color = new Color();
                    string clolrhex = ColorToStr(itemcolor);
                    ColorUtility.TryParseHtmlString("#" + clolrhex, out color);
                    Texture2D tex = AutoDrawing.SetDrawingTexture(color, editorhandFootTexture.selectType, Statics.fingerScales[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
                else if (editorhandFootTexture.selectType == SelectType.Foot)
                {
                    Color color = new Color();
                    string clolrhex = ColorToStr(itemcolor);
                    ColorUtility.TryParseHtmlString("#" + clolrhex, out color);
                    Texture2D tex = AutoDrawing.SetDrawingTexture(color, editorhandFootTexture.selectType, Statics.toesScales[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
            }
            else if (mode.ToString() == "PearlRainbow")
            {
                if (editorhandFootTexture.selectType == SelectType.Hand)
                {
                    Texture2D tex = AutoDrawing.SetDrawingPatternTexture(texture2D, editorhandFootTexture.selectType, Statics.fingerScales[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
                else if (editorhandFootTexture.selectType == SelectType.Foot)
                {
                    Texture2D tex = AutoDrawing.SetDrawingPatternTexture(texture2D, editorhandFootTexture.selectType, Statics.toesScales[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
            }
            else if (mode.ToString() == "Glittery")
            {
                if (editorhandFootTexture.selectType == SelectType.Hand)
                {
                    Texture2D tex = AutoDrawing.SetDrawingGlitteryTexture(itemName, editorhandFootTexture.selectType, Statics.fingerScales[k], Statics.fingerAutoDrawingCount[k], Statics.fingerAutoDrawingRatio[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
                else if (editorhandFootTexture.selectType == SelectType.Foot)
                {
                    Texture2D tex = AutoDrawing.SetDrawingGlitteryTexture(itemName, editorhandFootTexture.selectType, Statics.toesScales[k], Statics.toesAutoDrawingCount[k], Statics.toesAutoDrawingRatio[k]);
                    editorhandFootTexture.texture2Ds.Add(tex);
                }
            }
        }
        handFootTexture = editorhandFootTexture;
    }

    public SelectType selectedType(int id)
    {
        if(id == 0 || id == 1 || id == 2 || id == 3 || id == 4 || id == 5)
        {
            return SelectType.Hand;
        }
        else
        {
            return SelectType.Foot;
        }
    }

    public static string ColorToStr(Color color)
    {
        string r = ((int)(color.r * 255)).ToString("X2");
        string g = ((int)(color.g * 255)).ToString("X2");
        string b = ((int)(color.b * 255)).ToString("X2");
        string a = ((int)(color.a * 255)).ToString("X2");


        string result = string.Format("{0}{1}{2}", r, g, b);


        return result;
    }
}
