using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ConfirmFunctioner
{
    /// <summary>
    /// 매니큐어 그리기 검사
    /// </summary>
    /// <param name="finger"></param>
    /// <param name="selectFinger"></param>
    /// <param name="questFingerData"></param>
    /// <returns></returns>
    public static int PaintConfirm(AdvancedMobilePaint finger, int selectFinger, QuestFingerData questFingerData)
    {
        int trueCount = 0;
        string customManicureName = "";

        if (finger == null)
        {
            //return 0;
        }

        trueCount = 0;

        string fingerManicureName = finger.manicureName;
        string questManicureName = questFingerData.manicureName;
        if (questManicureName == fingerManicureName)
        {
            trueCount++;
        }
        else
        {
            //return 0;
        }

        Texture2D questTexture2D = HandFootTextureMaker.Instance.GetTexture2D(questFingerData.level, Statics.selectType, selectFinger);

        //현재 색칠한 매니큐어 정보를 가져옴
        Texture2D fingerTexture2D = null;
        //색칠한 매니큐어가 글리터리일경우
        if (questFingerData.mode != "Glittery")
        {
            fingerTexture2D = finger.tex;
        }
        //글리터리가 아닐경우
        else
        {
            fingerTexture2D = finger.confirmTex;
        }
        Color[] fingerTexture2DColors = fingerTexture2D.GetPixels();
        Color[] questTexture2DColors = questTexture2D.GetPixels();
        List<Color> correctPaintColor = new List<Color>();
        List<Color> alphaZeroColors = new List<Color>();
        for (int i = 0; i < questTexture2DColors.Length; i++)
        {
            //자동완성용 텍스쳐와 현재 색칠된 텍스쳐의 컬러값 배열을 비교하여 같은경우
            if (questTexture2DColors[i] == fingerTexture2DColors[i])
            {
                correctPaintColor.Add(questTexture2DColors[i]);
            }
            //현재 색칠한 텍스쳐의 컬러값 배열의 알파값이 0인경우
            if (fingerTexture2DColors[i].a == 0)
            {
                alphaZeroColors.Add(fingerTexture2DColors[i]);
            }
        }

        if (Statics.brushSize == 30)
        {
            customManicureName = fingerManicureName;
        }
        else if (Statics.brushSize == 15)
        {
            string[] manicureName = fingerManicureName.Split("_");
            customManicureName = manicureName[0] + "_" + manicureName[1];
        }
        //에러나서 수정_231212 박진범
        //ManicureData customManicureData = DataManager.Instance.GetManicureDataByName(customManicureName);
        ManicureData customManicureData;
        if (customManicureName == "")
        {
            int curManicureId = PlayerDataManager.Instance.GetInfo().curSelectedManicureId;
            string curManicureName = DataManager.Instance.GetManicureDataById(curManicureId).spriteName;
            customManicureData = DataManager.Instance.GetManicureDataByName(curManicureName);
        }
        else
        {
            customManicureData = DataManager.Instance.GetManicureDataByName(customManicureName);
        }
        if (customManicureData.mode == "PearlRainbow")
        {
            if (Statics.selectType == SelectType.Hand)
            {
                if (correctPaintColor.Count > Statics.fingerPearlRainbowCutline[selectFinger])
                {
                    trueCount++;
                }
            }
            else if (Statics.selectType == SelectType.Foot)
            {
                if (correctPaintColor.Count > Statics.toesPearlRainbowCutline[selectFinger])
                {
                    trueCount++;
                }
            }
        }
        else if (customManicureData.mode == "Glittery")
        {
            if (Statics.selectType == SelectType.Hand)
            {
                if (alphaZeroColors.Count < Statics.fingerGlitteryCutline[selectFinger])
                {
                    trueCount++;
                }
            }
            else if (Statics.selectType == SelectType.Foot)
            {
                if (alphaZeroColors.Count < Statics.toesGlitteryCutline[selectFinger])
                {
                    trueCount++;
                }
            }
        }
        else
        {
            if (Statics.selectType == SelectType.Hand)
            {
                if (correctPaintColor.Count > Statics.fingerCreamCutline[selectFinger])
                {
                    trueCount++;
                }
            }
            else if (Statics.selectType == SelectType.Foot)
            {
                if (correctPaintColor.Count > Statics.toesCreamCutline[selectFinger])
                {
                    trueCount++;
                }
            }
        }
        return trueCount;
    }

    /// <summary>
    /// 패턴의 색상과 모양 검사
    /// </summary>
    /// <param name="patternData"></param>
    /// <param name="patternConverter"></param>
    /// <param name="questable"></param>
    /// <returns></returns>
    public static bool patternColorConfirm(QuestPatternData patternData, PatternConverter patternConverter, IQuestable questable)
    {
        if (questable.IsPatternScroll == true)
        {
            return false;
        }

        Image image = patternConverter.Image;
        string colorCode = ColorUtility.ToHtmlStringRGBA(image.color);
        if ("#" + colorCode == patternData.patternColorString + "FF")
        {
      
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 캐릭터스티커, 일반스티커, 애니메이션스티커 위치, 회전값, 크기, 이미지 이름 검사
    /// </summary>
    /// <param name="stickerObject"></param>
    /// <param name="guideStickerObject"></param>
    /// <returns></returns>
    public static GameObject StickerConfirm(StickerConverter stickerConverter, List<GameObject> guideStickerObject)
    {
        int trueCount = 0;
        GameObject returnGameObject = null;

        for (int i = 0; i < guideStickerObject.Count; i++)
        {
            returnGameObject = guideStickerObject[i];
            RectTransform stickerObjectRecttransfrom = stickerConverter.GetComponent<RectTransform>();
            RectTransform stickerGuideObjectRecttransfrom = guideStickerObject[i].GetComponent<RectTransform>();

            //이름비교
            if (stickerConverter.stickerType == StickerType.CharacterSticker)
            {
                Image image = stickerConverter.StickerImage;
                string[] characterStickerObjectSpriteName = image.sprite.name.Split("(");
                Image guideCharacterStickerimage = guideStickerObject[i].GetComponent<Image>();
                string[] guideCharacterStickerObjectSpriteName = guideCharacterStickerimage.sprite.name.Split("(");
                if (characterStickerObjectSpriteName[0] == guideCharacterStickerObjectSpriteName[0])
                {
                    trueCount++;
                }
            }
            else if (stickerConverter.stickerType == StickerType.Sticker)
            {
                Image image = stickerConverter.StickerImage;
                string[] stickerObjectSpriteName = image.sprite.name.Split("(");
                Image guideStickerimage = guideStickerObject[i].GetComponent<Image>();
                string[] guideStickerObjectSpriteName = guideStickerimage.sprite.name.Split("(");
                if (stickerObjectSpriteName[0] == guideStickerObjectSpriteName[0])
                {
                    trueCount++;
                }
            }
            else if (stickerConverter.stickerType == StickerType.AnimationSticker)
            {
                if (stickerConverter.AnimationStickerObject != null)
                {
                    Image animationStickerimage = stickerConverter.AnimationStickerObject.GetComponent<Image>();
                    string[] animationStickerObjectSpriteName = animationStickerimage.sprite.name.Split("_");
                    Image guideAnimationStickerimage = guideStickerObject[i].GetComponent<Image>();
                    string[] guideAnimationStickerObjectSpriteName = guideAnimationStickerimage.sprite.name.Split("_");
                    if (animationStickerObjectSpriteName[0] + "_" + animationStickerObjectSpriteName[1] == guideAnimationStickerObjectSpriteName[0] + "_" + guideAnimationStickerObjectSpriteName[1])
                    {
                        trueCount++;
                    }
                }
            }

            //위치값 비교
            Vector2 pos = stickerGuideObjectRecttransfrom.anchoredPosition;
            if (stickerObjectRecttransfrom.anchoredPosition.x - 30.0f < pos.x && pos.x < stickerObjectRecttransfrom.anchoredPosition.x + 30.0f
                && stickerObjectRecttransfrom.anchoredPosition.y - 30.0f < pos.y && pos.y < stickerObjectRecttransfrom.anchoredPosition.y + 30.0f)
            {
                trueCount++;
            }

            //회전값 비교
            //float rotZ = stickerGuideObjectRecttransfrom.localRotation.z;
            float guideStickerrotZ = stickerGuideObjectRecttransfrom.localRotation.eulerAngles.z;
            if (guideStickerrotZ > 180)
            {
                guideStickerrotZ -= 360.0f;
            }
            guideStickerrotZ = Mathf.Abs(guideStickerrotZ);

            float printStickerrotZ = stickerObjectRecttransfrom.localRotation.eulerAngles.z;
            if (printStickerrotZ > 180)
            {
                printStickerrotZ -= 360.0f;
            }
            printStickerrotZ = Mathf.Abs(printStickerrotZ);


            if (printStickerrotZ - 20.0f < guideStickerrotZ && guideStickerrotZ < printStickerrotZ + 20.0f)
            {
                trueCount++;
            }

            //크기 비교
            Vector2 scale = stickerGuideObjectRecttransfrom.sizeDelta;
            if (stickerObjectRecttransfrom.sizeDelta.x - 50.0f < scale.x && scale.x < stickerObjectRecttransfrom.sizeDelta.x + 50.0f
                && stickerObjectRecttransfrom.sizeDelta.y - 50.0f < scale.y && scale.y < stickerObjectRecttransfrom.sizeDelta.y + 50.0f)
            {
                trueCount++;
            }

            //1번데이터가 모두 일치하면 반복문을 종료
            if (trueCount == 4)
            {
                break;
            }
            else if (trueCount != 4)
            {
                trueCount = 0;
            }
        }

        if (trueCount == 4)
        {
            return returnGameObject;
        }
        else
        {
            return null;
        }
    }
}
