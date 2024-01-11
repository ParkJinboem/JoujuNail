using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class EditorRoot
{
    public Transform parent;
    public List<Image> images;
}

public class EditorAlbum : MonoBehaviour
{
    private QuestHandData questHandData;

    [SerializeField] private GameObject handRoot;
    [SerializeField] private GameObject footRoot;
    [SerializeField] private Image handImage;
    [SerializeField] private Image footImage;
    [SerializeField] private Image[] fingerImage;
    [SerializeField] private Image[] toesImage;
    [SerializeField] private Image[] fingerNailMaskImage;
    [SerializeField] private Image[] toesNailMaskImage;
    [SerializeField] private Image[] fingerNailCovers;
    [SerializeField] private Image[] toesNailCovers;
    [SerializeField] private RawImage[] fingerNailTexture;
    [SerializeField] private RawImage[] toesNailTexture;
    [SerializeField] private Image[] fingerPatternItems;
    [SerializeField] private Image[] toesPatternItems;
    [SerializeField] private List<EditorRoot> fingerCharacterStickerRoots;
    [SerializeField] private List<EditorRoot> toesCharacterStickerRoots;
    [SerializeField] private List<EditorRoot> fingerStickerRoots;
    [SerializeField] private List<EditorRoot> toesStickerRoots;
    [SerializeField] private List<EditorRoot> fingerAnimationStickerRoots;
    [SerializeField] private List<EditorRoot> toesAnimationStickerRoots;
    [SerializeField] private GameObject editorStickerItemPrefabs;

    public void Init(int level)
    {
        questHandData = DataManager.Instance.GetQuestHandDataWithLevel(level);

        if (questHandData.selectType == SelectType.Hand)
        {
            ResetSticker(fingerCharacterStickerRoots, fingerStickerRoots, fingerAnimationStickerRoots);
            footRoot.SetActive(false);
            for (int i = 0; i < 5; i++)
            {
                QuestFingerData questFingerData = questHandData.questFingerDatas[i];
                QuestPatternData questPatternData = questFingerData.questPatternData;
                List<QuestStickerData> questStickerDatas = questFingerData.questStickerDatas;

                SetFingerToes(questFingerData, fingerNailMaskImage[i], fingerNailCovers[i], fingerNailTexture[i], i);
                SetPattern(questPatternData, fingerPatternItems[i]);
                SetSticker(questStickerDatas, fingerCharacterStickerRoots[i], fingerStickerRoots[i], fingerAnimationStickerRoots[i]);
            }
        }
        else if (questHandData.selectType == SelectType.Foot)
        {
            ResetSticker(toesCharacterStickerRoots, toesStickerRoots, toesAnimationStickerRoots);
            handRoot.SetActive(false);
            for (int i = 0; i < 5; i++)
            {
                QuestFingerData questFingerData = questHandData.questFingerDatas[i];
                QuestPatternData questPatternData = questFingerData.questPatternData;
                List<QuestStickerData> questStickerDatas = questFingerData.questStickerDatas;

                SetFingerToes(questFingerData, toesNailMaskImage[i], toesNailCovers[i], toesNailTexture[i], i);
                SetPattern(questPatternData, toesPatternItems[i]);
                SetSticker(questStickerDatas, toesCharacterStickerRoots[i], toesStickerRoots[i], toesAnimationStickerRoots[i]);
            }
        }
    }


    /// <summary>
    /// 손,발가락 색상 셋팅 
    /// </summary>
    public void SetHandFootImage()
    {
        //Info info = PlayerDataManager.Instance.GetInfo();

        //poss = new List<Vector3>();
        //scales = new List<Vector3>();
        //if (questHandData.selectType == SelectType.Hand)
        //{
        //    handRoot.SetActive(true);
        //    footRoot.SetActive(false);

        //    for (int i = 0; i < 5; i++)
        //    {
        //        fingerImage[i].sprite = DataManager.Instance.GetFingerSprite(info.fingerImagenames[i]);

        //        RectTransform rectTransform = fingerImage[i].GetComponent<RectTransform>();
        //        poss.Add(rectTransform.localPosition);
        //        scales.Add(rectTransform.localScale);
        //    }
        //}
        //else if (questHandData.selectType == SelectType.Foot)
        //{
        //    handRoot.SetActive(false);
        //    footRoot.SetActive(true);

        //    footImage.sprite = DataManager.Instance.GetFootSprite(info.footImageName);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        toesImage[i].sprite = DataManager.Instance.GetToesSprite(info.toesImagenames[i]);

        //        RectTransform rectTransform = toesImage[i].GetComponent<RectTransform>();
        //        poss.Add(rectTransform.localPosition);
        //        scales.Add(rectTransform.localScale);
        //    }
        //}
    }

    /// <summary>
    /// 손가락 또는 발가락에 따른 텍스쳐, 커버, 마스크 생성
    /// </summary>
    /// <param name="questFingerData"></param>
    /// <param name="mask"></param>
    /// <param name="cover"></param>
    /// <param name="nailTexture"></param>
    /// <param name="index"></param>
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
    private void SetSticker(List<QuestStickerData> questStickerDatas, EditorRoot characterStickerRoot, EditorRoot stickerRoot, EditorRoot animationStickerRoot)
    {
        for (int j = 0; j < questStickerDatas.Count; j++)
        {
            QuestStickerData questStickerData = questStickerDatas[j];
            if (questStickerData.isHave == true)
            {
                GameObject stickerObj = Instantiate(editorStickerItemPrefabs);
                if (questStickerData.stickerType == StickerType.CharacterSticker)
                {
                    Image characterSticker = stickerObj.GetComponent<Image>();
                    characterStickerRoot.images.Add(characterSticker);
                    stickerObj.transform.SetParent(characterStickerRoot.parent);
                }
                else if (questStickerData.stickerType == StickerType.Sticker)
                {
                    Image sticker = stickerObj.GetComponent<Image>();
                    stickerRoot.images.Add(sticker);
                    stickerObj.transform.SetParent(stickerRoot.parent);
                }
                else
                {
                    Image sticker = stickerObj.GetComponent<Image>();
                    animationStickerRoot.images.Add(sticker);
                    stickerObj.transform.SetParent(animationStickerRoot.parent);
                }
                Image image = stickerObj.GetComponent<Image>();
                image.sprite = DataManager.Instance.GetManicureItemSprite(questStickerData.stickerSpriteString);
                RectTransform rectTransform = stickerObj.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(questStickerData.stickerPosX, questStickerData.stickerPosY, questStickerData.stickerPosZ);
                rectTransform.localRotation = Quaternion.Euler(questStickerData.stickerRotX, questStickerData.stickerRotY, questStickerData.stickerRotZ);
                rectTransform.sizeDelta = new Vector2(questStickerData.stickerWidth, questStickerData.stickerHeight);
                rectTransform.localScale = Vector3.one;
            }
        }
    }

    /// <summary>
    /// 스티커 이미지 제거
    /// </summary>
    /// <param name="characterStickerRoots"></param>
    /// <param name="stickerRoots"></param>
    /// <param name="animationStickerRoots"></param>
    private void ResetSticker(List<EditorRoot> characterStickerRoots, List<EditorRoot> stickerRoots, List<EditorRoot> animationStickerRoots)
    {
        for (int i = 0; i < 5; i++)
        {
            if (characterStickerRoots[i].images.Count != 0)
            {
                for (int k = 0; k < characterStickerRoots[i].images.Count; k++)
                {
                    Destroy(characterStickerRoots[i].images[k].gameObject);
                    characterStickerRoots[i].images = new List<Image>();
                }
            }
            if (stickerRoots[i].images.Count != 0)
            {
                for (int k = 0; k < stickerRoots[i].images.Count; k++)
                {
                    Destroy(stickerRoots[i].images[k].gameObject);
                    stickerRoots[i].images = new List<Image>();
                }
            }
            if (animationStickerRoots[i].images.Count != 0)
            {
                for (int k = 0; k < animationStickerRoots[i].images.Count; k++)
                {
                    Destroy(animationStickerRoots[i].images[k].gameObject);
                    animationStickerRoots[i].images = new List<Image>();
                }
            }
        }
    }





    public void clickedAlbum()
    {

    }
}
