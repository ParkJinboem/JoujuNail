using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EditorPaintSceneManager : MonoSingleton<EditorPaintSceneManager>
{
    private QuestHandData questHandData;
    public int selectedNum;
    public int level;
    [SerializeField] private int nailMaskNum = 1;
    [SerializeField] private int toesMaskNum = 1;
    [SerializeField] private int handFootkNum = 1;
    

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private ManicureItemScrollView manicureItemScrollView;

    [Header("Nail")]
    [SerializeField] private RawImage[] fingertoesNailTexture;
    [SerializeField] private Image[] fingertoesPatternImage;
    [SerializeField] private Image[] fingertoesCharacterStickerImage;
    [SerializeField] private Image[] fingertoesStickerImage;
    [SerializeField] private Image[] fingertoesAnimStickerImage;
    [SerializeField] private Color[] patternBaseColor;
    [SerializeField] private Image handImage;
    [SerializeField] private Image footImage;
    [SerializeField] private Image[] fingerNailMasks;
    [SerializeField] private Image[] toesNailMasks;
    [SerializeField] private Image[] fingerNailCovers;
    [SerializeField] private Image[] toesNailCovers;

    [Header("Editor")]
    [SerializeField] InputField patternSize;
    [SerializeField] InputField characterStickerSize;
    [SerializeField] InputField stickerSize;
    [SerializeField] InputField animStickerSize;
    [SerializeField] InputField selcetedFinger;
    [SerializeField] InputField textLevel;

    [Header("itemName")]
    [SerializeField] string[] manicureName;
    [SerializeField] string[] patternName;
    [SerializeField] string[] characterStickerName;
    [SerializeField] string[] stickerName;
    [SerializeField] string[] animStickerName;




    public void SetUpPaintTexture(EditorPaintHandFootTexture texture, string itemName)
    {
        manicureName[selectedNum] = itemName;
        fingertoesNailTexture[selectedNum].texture = texture.texture2Ds[0];
    }

    public void SetUpPattern(Image image, string itemName)
    {
        patternName[selectedNum] = itemName;
        patternBaseColor[selectedNum] = image.color;
        fingertoesPatternImage[selectedNum].gameObject.SetActive(true);
        fingertoesPatternImage[selectedNum].sprite = image.sprite;
        fingertoesPatternImage[selectedNum].color = image.color;
    }
    public void SetUpPatternColor(int id,Color color)
    {
        if(id== 0)
        {
            fingertoesPatternImage[selectedNum].color = patternBaseColor[selectedNum];
        }
        else
        {
            fingertoesPatternImage[selectedNum].color = color;
        }
    }
    public void SetUpCharacterSticker(Sprite sprite, string itemName)
    {
        characterStickerName[selectedNum] = itemName;
        fingertoesCharacterStickerImage[selectedNum].transform.parent.gameObject.SetActive(true);
        fingertoesCharacterStickerImage[selectedNum].sprite = sprite;
    }
    public void SetUpSticker(Sprite sprite, string itemName)
    {
        stickerName[selectedNum] = itemName;
        fingertoesStickerImage[selectedNum].transform.parent.gameObject.SetActive(true);
        fingertoesStickerImage[selectedNum].sprite = sprite;
    }
    public void SetUpAnimSticker(Sprite sprite, string itemName)
    {
        animStickerName[selectedNum] = itemName;
        fingertoesAnimStickerImage[selectedNum].transform.parent.gameObject.SetActive(true);
        fingertoesAnimStickerImage[selectedNum].sprite = sprite;
    }

    public void SetUpPatternSize()
    {
        float size;
        if(patternSize.text == "")
        {
            size = 512.0f;
        }
        else
        {
            size = float.Parse(patternSize.text);
        }
        fingertoesPatternImage[selectedNum].gameObject.GetComponent<RectTransform>().localScale = new Vector2(size, size);
    }

    public void SetUpCharacterStickerSize()
    {
        int size;
        if (characterStickerSize.text == "")
        {
            size = 256;
        }
        else
        {
            size = int.Parse(characterStickerSize.text);
        }
        fingertoesCharacterStickerImage[selectedNum].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
    }

    public void SetUpStickerSize()
    {
        int size;
        if (stickerSize.text == "")
        {
            size = 256;
        }
        else
        {
            size = int.Parse(stickerSize.text);
        }
        fingertoesStickerImage[selectedNum].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
    }

    public void SetUpAnimStickerSize()
    {
        int size;
        if (animStickerSize.text == "")
        {
            size = 256;
        }
        else
        {
            size = int.Parse(animStickerSize.text);
        }
        fingertoesAnimStickerImage[selectedNum].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
    }

    public void SetUpFinger()
    {
        int num;
        if (selcetedFinger.text == "")
        {
            num = 0;
        }
        else
        {
            num = int.Parse(selcetedFinger.text);
        }
        selectedNum = num;
    }

    public void SetUpLevel()
    {
        if (textLevel.text == "")
        {
            level = 1;
        }
        else
        {
            level = int.Parse(textLevel.text);
        }
    }

    /// <summary>
    /// 손, 발 이미지 변경
    /// </summary>
    public void ChangeHand()
    {
        handFootkNum++;
        if(handFootkNum > 16)
        {
            handFootkNum = 1;
        }

        handImage.sprite = DataManager.Instance.GetHandSprite("Hand" + "_" + handFootkNum.ToString("D3"));
        footImage.sprite = DataManager.Instance.GetFootSprite("Foot" + "_" + handFootkNum.ToString("D3"));
    }

    
    public void ChangeNailMask()
    {
        nailMaskNum++;
        toesMaskNum++;
        if (nailMaskNum > 20)
        {
            nailMaskNum = 1;
        }
        if (toesMaskNum > 4)
        {
            toesMaskNum = 1;
        }

        
        for (int i = 0; i < 5; i++)
        {
            fingerNailMasks[i].sprite = DataManager.Instance.GetFingerNailMaskSprite("FingerNailMask" + nailMaskNum + "_" + (i + 1).ToString("D3"));
            fingerNailCovers[i].sprite = DataManager.Instance.GetFingerNailCoverSprite("FingerNailCover"+ nailMaskNum + "_" + (i + 1).ToString("D3"));
        }
        
        for (int i = 0; i < 5; i++)
        {
            toesNailMasks[i].sprite = DataManager.Instance.GetToesNailMaskSprite("ToesNailMask" + toesMaskNum + "_" + (i + 1).ToString("D3"));
            toesNailCovers[i].sprite = DataManager.Instance.GetToesNailCoverSprite("ToesNailCover" + toesMaskNum + "_" + (i + 1).ToString("D3"));
        }
    }


    public void ShowHandInfomation()
    {

       string[] patternColor = new string[10];

        for (int i = 0; i < 5; i++)
        {
            if (fingertoesPatternImage[i].sprite != null)
                patternName[i] = fingertoesPatternImage[i].sprite.name;
            if (fingertoesCharacterStickerImage[i].sprite != null)
                characterStickerName[i] = fingertoesCharacterStickerImage[i].sprite.name;
            if (fingertoesStickerImage[i].sprite != null)
                stickerName[i] = fingertoesStickerImage[i].sprite.name;
            if (fingertoesAnimStickerImage[i].sprite != null)
                animStickerName[i] = fingertoesAnimStickerImage[i].sprite.name;

            patternColor[i] = EditorManicureItem.ColorToStr(fingertoesPatternImage[i].color);

            Debug.Log($"손가락{i} 매니큐어______________매니큐어이름: {manicureName[i]}, 네일마스크이름: {fingerNailMasks[i].sprite.name}, 네일커버이름: {fingerNailCovers[i].sprite.name}");
            Debug.Log($"손가락{i} 패턴_________________패턴 이름: {patternName[i]}, 패턴 크기: {fingertoesPatternImage[i].transform.localScale}, 패턴색상: #{patternColor[i]}");
            Debug.LogWarning($"손가락{i} 캐릭터스티커____캐릭터스티커이름: {characterStickerName[i]}, 캐릭터스티커 위치: {fingertoesCharacterStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition}, 캐릭터스티커 크기: {fingertoesCharacterStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta}");
            Debug.LogWarning($"손가락{i} 스티커________스티커이름: {stickerName[i]}, 스티커 위치: {fingertoesStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition}, 스티커 크기: {fingertoesStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta}");
            Debug.LogWarning($"손가락{i} 애님스티커_____애니메이션 스티커 이름: {animStickerName[i]}, 애님스티커 위치: {fingertoesAnimStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition}, 애님스티커 크기: {fingertoesAnimStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta}");
        }
    }

    public void ShowFootInfomation()
    {

        string[] patternColor = new string[10];

        for (int i = 5; i < 10; i++)
        {
            //manicureName[i] =
            if(fingertoesPatternImage[i].sprite != null)
                patternName[i] = fingertoesPatternImage[i].sprite.name;
            if (fingertoesCharacterStickerImage[i].sprite != null)
                characterStickerName[i] = fingertoesCharacterStickerImage[i].sprite.name;
            if (fingertoesStickerImage[i].sprite != null)
                stickerName[i] = fingertoesStickerImage[i].sprite.name;
            if (fingertoesAnimStickerImage[i].sprite != null)
                animStickerName[i] = fingertoesAnimStickerImage[i].sprite.name;

            patternColor[i] = EditorManicureItem.ColorToStr(fingertoesPatternImage[i].color);

            Debug.Log($"발가락{i} 매니큐어///       매니큐어이름: {manicureName[i]}, 네일마스크이름: {toesNailMasks[i-5].sprite.name}, 네일커버이름: {toesNailCovers[i-5].sprite.name}");
            Debug.Log($"발가락{i} 패턴///       패턴이름: {patternName[i]}, 패턴 크기: {fingertoesPatternImage[i].transform.localScale}, 패턴색상: #{patternColor[i]}");
            Debug.LogWarning($"발가락{i} 캐릭터스티커/// 캐릭터스티커이름: {characterStickerName[i]}, 캐릭터스티커 위치: {fingertoesCharacterStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition}, 캐릭터스티커 크기: {fingertoesCharacterStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta}");
            Debug.LogWarning($"발가락{i} 스티커///     스티커이름: {stickerName[i]}, 스티커 위치: {fingertoesStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition}, 스티커 크기: {fingertoesStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta}");
            Debug.LogWarning($"발가락{i} 애님스티커///  애니메이션 스티커 이름: {animStickerName[i]}, 애님스티커 위치: {fingertoesAnimStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition}, 애님스티커 크기: {fingertoesAnimStickerImage[i].transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta}");
        }
    }

    public void DeleteData()
    {
        for (int i = 0; i < 10; i++)
        {
            fingertoesNailTexture[i].texture = null;
            fingertoesPatternImage[i].sprite = null;
            fingertoesCharacterStickerImage[i].sprite = null;
            fingertoesStickerImage[i].sprite = null;
            fingertoesAnimStickerImage[i].sprite = null;
            manicureName[i] = "";
            patternName[i]= "";
            characterStickerName[i]= "";
            stickerName[i] = "";
            animStickerName[i]= "";
            fingertoesPatternImage[i].gameObject.SetActive(false);
            fingertoesCharacterStickerImage[i].transform.parent.gameObject.SetActive(false);
            fingertoesStickerImage[i].transform.parent.gameObject.SetActive(false);
            fingertoesAnimStickerImage[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void LevelLoad()
    {
        questHandData = DataManager.Instance.GetQuestHandDataWithLevel(level);

        if (questHandData.selectType == SelectType.Hand)
        {
            for (int i = 0; i < 5; i++)
            {
                QuestFingerData questFingerData = questHandData.questFingerDatas[i];
                QuestPatternData questPatternData = questFingerData.questPatternData;
                List<QuestStickerData> questStickerDatas = questFingerData.questStickerDatas;
                manicureName[i] = questFingerData.manicureName;

                SetFingerToes(questFingerData, fingerNailMasks[i], fingerNailCovers[i], fingertoesNailTexture[i], i);
                SetPattern(questPatternData, fingertoesPatternImage[i]);
                SetSticker(questStickerDatas, fingertoesCharacterStickerImage[i], fingertoesStickerImage[i], fingertoesAnimStickerImage[i]);
            }
        }
        else if (questHandData.selectType == SelectType.Foot)
        {
            for (int i = 5; i < 10; i++)
            {
                QuestFingerData questFingerData = questHandData.questFingerDatas[i-5];
                QuestPatternData questPatternData = questFingerData.questPatternData;
                List<QuestStickerData> questStickerDatas = questFingerData.questStickerDatas;
                manicureName[i] = questFingerData.manicureName;

                SetFingerToes(questFingerData, toesNailMasks[i-5], toesNailCovers[i-5], fingertoesNailTexture[i], i - 5);
                SetPattern(questPatternData, fingertoesPatternImage[i]);
                SetSticker(questStickerDatas, fingertoesCharacterStickerImage[i], fingertoesStickerImage[i], fingertoesAnimStickerImage[i]);
            }
        }
    }

    private void SetFingerToes(QuestFingerData questFingerData, Image mask, Image cover, RawImage nailTexture, int index)
    {
        if (questHandData.selectType == SelectType.Hand)
        {
            mask.sprite = DataManager.Instance.GetFingerNailMaskSprite(questFingerData.nailMaskString);
            cover.sprite = DataManager.Instance.GetFingerNailCoverSprite(questFingerData.nailCoverString);
        }
        else if (questHandData.selectType == SelectType.Foot)
        {
            mask.sprite = DataManager.Instance.GetToesNailMaskSprite(questFingerData.nailMaskString);
            cover.sprite = DataManager.Instance.GetToesNailCoverSprite(questFingerData.nailCoverString);
        }

        Texture2D tex = EditorAlbumData.Instance.GetTexture2D(questHandData.level, questHandData.selectType, index);
        string targetTexture = "_MainTex";
        if (nailTexture.GetComponent<MeshRenderer>() == null)
        {
            MeshRenderer meshRenderer = nailTexture.AddComponent<MeshRenderer>();
            meshRenderer.material.SetTexture(targetTexture, tex);
        }
        nailTexture.texture = tex;
    }

    /// <summary>
    /// 패턴 셋팅
    /// </summary>
    /// <param name="questPatternData"></param>
    /// <param name="fingerPatternItem"></param>
    private void SetPattern(QuestPatternData questPatternData, Image fingerPatternItem)
    {
        if (questPatternData.isHave == true)
        {
            fingerPatternItem.gameObject.SetActive(true);
            fingerPatternItem.sprite = DataManager.Instance.GetManicureItemSprite(questPatternData.patternSpriteString);
            Color color = fingerPatternItem.color;
            UnityEngine.ColorUtility.TryParseHtmlString(questPatternData.patternColorString, out color);
            fingerPatternItem.color = color;
            fingerPatternItem.transform.localPosition = new Vector3(questPatternData.patternPosX, questPatternData.patternPosY, questPatternData.patternPosZ);
            RectTransform rectTransform = fingerPatternItem.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(questPatternData.patternScaleX, questPatternData.patternScaleY, questPatternData.patternScaleZ);
        }
        else
        {
            fingerPatternItem.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 스티커 셋팅
    /// </summary>
    /// <param name="questStickerDatas"></param>
    /// <param name="characterStickerRoot"></param>
    /// <param name="stickerRoot"></param>
    /// <param name="animationStickerRoot"></param>
    private void SetSticker(List<QuestStickerData> questStickerDatas, Image characterStickerRoot, Image stickerRoot, Image animationStickerRoot)
    {
        for (int j = 0; j < questStickerDatas.Count; j++)
        {
            QuestStickerData questStickerData = questStickerDatas[j];
            if (questStickerData.isHave == true)
            {
                if (questStickerData.stickerType == StickerType.CharacterSticker)
                {
                    characterStickerRoot.transform.parent.gameObject.SetActive(true);
                    characterStickerRoot.sprite = DataManager.Instance.GetManicureItemSprite(questStickerData.stickerSpriteString);
                    rectTransform = characterStickerRoot.transform.parent.gameObject.GetComponent<RectTransform>();                
                }
                else if (questStickerData.stickerType == StickerType.Sticker)
                {
                    stickerRoot.transform.parent.gameObject.SetActive(true);
                    stickerRoot.sprite = DataManager.Instance.GetManicureItemSprite(questStickerData.stickerSpriteString);
                    rectTransform = stickerRoot.transform.parent.gameObject.GetComponent<RectTransform>();
                }
                else
                {
                    animationStickerRoot.transform.parent.gameObject.SetActive(true);
                    animationStickerRoot.sprite = DataManager.Instance.GetManicureItemSprite(questStickerData.stickerSpriteString);
                    rectTransform = animationStickerRoot.transform.parent.gameObject.GetComponent<RectTransform>();
                }
                rectTransform.localPosition = new Vector3(questStickerData.stickerPosX, questStickerData.stickerPosY, questStickerData.stickerPosZ);
                rectTransform.localRotation = Quaternion.Euler(questStickerData.stickerRotX, questStickerData.stickerRotY, questStickerData.stickerRotZ);
                rectTransform.sizeDelta = new Vector2(questStickerData.stickerWidth, questStickerData.stickerHeight);
                rectTransform.localScale = Vector3.one;
            }
        }
    }
}
