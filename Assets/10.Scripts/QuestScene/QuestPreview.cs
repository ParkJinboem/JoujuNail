using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Root
{
    public Transform parent;
    public List<Image> images;
}

public class QuestPreview : MonoBehaviour
{
    [SerializeField] private Button button;

    private QuestHandData questHandData;
  
    [Header("Quest")]
    [SerializeField] private GameObject handRoot;
    [SerializeField] private GameObject footRoot;
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
    [SerializeField] RectTransform[] fingerClearMarks;
    [SerializeField] RectTransform[] toesClearMarks;
    [SerializeField] private List<Root> fingerCharacterStickerRoots;
    [SerializeField] private List<Root> toesCharacterStickerRoots;
    [SerializeField] private List<Root> fingerStickerRoots;
    [SerializeField] private List<Root> toesStickerRoots;
    [SerializeField] private List<Root> fingerAnimationStickerRoots;
    [SerializeField] private List<Root> toesAnimationStickerRoots;
    [SerializeField] private GameObject stickerItemPrefabs;

    [Header("Animation")]
    [SerializeField] private AnimationCurve preViewAnimation;
    [SerializeField] private AnimationCurve buttonAnimation;
    [SerializeField] private AnimationCurve clearAnimation;
    [SerializeField] private AnimationCurve clearMarkAnimation;
    [SerializeField] private RectTransform preViewRect;
    [SerializeField] private RectTransform panelRect;
    private RectTransform canvasRect;
    private float animPlayTime = 0;
    public bool isBase;

    [Header("Finger")]
    [SerializeField] private GameObject[] previewFinger;
    [SerializeField] private List<Vector3> fingerPoss;
    [SerializeField] private List<Vector3> fingerScales;
    [SerializeField] private GameObject[] previewToes;
    [SerializeField] private List<Vector3> toesPoss;
    [SerializeField] private List<float> rotZs;
    [SerializeField] private List<Vector3> toesScales;
    private List<Vector3> poss;
    private List<Vector3> scales;

    [Header("Collider")]
    [SerializeField] private BoxCollider2D boxCollider2D;
    //[SerializeField] private Vector2 boxColOffsetInitValue;
    //[SerializeField] private Vector2 boxColSizeInitValue;
    private Vector2 boxColoffset;
    private Vector2 boxColsize;

    private int fingerNum;

    private void OnEnable()
    {
        for (int i = 0; i < 5; i++)
        {
            fingerClearMarks[i].localScale = Vector3.zero;
            toesClearMarks[i].localScale = Vector3.zero;
        }

        button.enabled = true;
    }

    private void Awake()
    {
        canvasRect = UIManager.Instance.sceneCanvas.GetComponent<RectTransform>();
        boxColoffset = boxCollider2D.offset;
        boxColsize = boxCollider2D.size;
        boxCollider2D.enabled = false;
    }

    /// <summary>
    /// 초기값 설정 
    /// </summary>
    /// <param name="questHandData"></param>
    public void Init(QuestHandData questHandData)
    {
        this.questHandData = questHandData;
        if (questHandData.selectType == SelectType.Hand)
        {
            ResetSticker(fingerCharacterStickerRoots, fingerStickerRoots, fingerAnimationStickerRoots);

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
        Info info = PlayerDataManager.Instance.GetInfo();

        poss = new List<Vector3>();
        scales = new List<Vector3>();
        if (questHandData.selectType == SelectType.Hand)
        {
            handRoot.SetActive(true);
            footRoot.SetActive(false);

            for (int i = 0; i < 5; i++)
            {
                fingerImage[i].sprite = DataManager.Instance.GetFingerSprite(info.fingerImagenames[i]);

                RectTransform rectTransform = fingerImage[i].GetComponent<RectTransform>();
                poss.Add(rectTransform.localPosition);
                scales.Add(rectTransform.localScale);
            }
        }
        else if (questHandData.selectType == SelectType.Foot)
        {
            handRoot.SetActive(false);
            footRoot.SetActive(true);

            footImage.sprite = DataManager.Instance.GetFootSprite(info.footImageName);
            for (int i = 0; i < 5; i++)
            {
                toesImage[i].sprite = DataManager.Instance.GetToesSprite(info.toesImagenames[i]);

                RectTransform rectTransform = toesImage[i].GetComponent<RectTransform>();
                poss.Add(rectTransform.localPosition);
                scales.Add(rectTransform.localScale);
            }
        }
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

        Texture2D tex = HandFootTextureMaker.Instance.GetTexture2D(questHandData.level, questHandData.selectType, index);
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
    private void SetSticker(List<QuestStickerData> questStickerDatas, Root characterStickerRoot, Root stickerRoot, Root animationStickerRoot)
    {
        for (int j = 0; j < questStickerDatas.Count; j++)
        {
            QuestStickerData questStickerData = questStickerDatas[j];
            if (questStickerData.isHave == true)
            {
                GameObject obj = Instantiate(stickerItemPrefabs);
                if (questStickerData.stickerType == StickerType.CharacterSticker)
                {
                    Image characterSticker = obj.GetComponent<Image>();
                    characterStickerRoot.images.Add(characterSticker);
                    obj.transform.SetParent(characterStickerRoot.parent);
                }
                else if (questStickerData.stickerType == StickerType.Sticker)
                {
                    Image sticker = obj.GetComponent<Image>();
                    stickerRoot.images.Add(sticker);
                    obj.transform.SetParent(stickerRoot.parent);
                }
                else
                {
                    Image sticker = obj.GetComponent<Image>();
                    animationStickerRoot.images.Add(sticker);
                    obj.transform.SetParent(animationStickerRoot.parent);
                }
                Image image = obj.GetComponent<Image>();
                image.sprite = DataManager.Instance.GetManicureItemSprite(questStickerData.stickerSpriteString);
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
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
    private void ResetSticker(List<Root> characterStickerRoots, List<Root> stickerRoots, List<Root> animationStickerRoots)
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

    /// <summary>
    /// 해당 손가락 및 해당 스텝에 맞는 위치, 이미지 셋팅
    /// </summary>
    /// <param name="fingerNum"></param>
    /// <param name="step"></param>
    public void SetImageOn(int fingerNum, int step)
    {
        this.fingerNum = fingerNum;
        StopCoroutine(ISetImageOn(step));
        StartCoroutine(ISetImageOn(step));
    }

    IEnumerator ISetImageOn(int step)
    {
        yield return new WaitForSeconds(1.65f);
        if (Statics.selectType == SelectType.Hand)
        {
            for (int i = 0; i < previewFinger.Length; i++)
            {
                previewFinger[i].SetActive(false);
            }

            previewFinger[fingerNum].SetActive(true);
            RectTransform rectTransform = previewFinger[fingerNum].GetComponent<RectTransform>();
            rectTransform.localScale = fingerScales[fingerNum];
            previewFinger[fingerNum].transform.localPosition = fingerPoss[fingerNum];
        }
        else if (Statics.selectType == SelectType.Foot)
        {
            footImage.gameObject.SetActive(false);
            for (int i = 0; i < previewToes.Length; i++)
            {
                previewToes[i].SetActive(false);
            }

            previewToes[fingerNum].SetActive(true);
            RectTransform rectTransform = previewToes[fingerNum].GetComponent<RectTransform>();
            rectTransform.localScale = toesScales[fingerNum];
            rectTransform.localRotation = Quaternion.identity;
            previewToes[fingerNum].transform.localPosition = toesPoss[fingerNum];
        }
        ShowPreviewStep(step, false);

        QuestSceneManager.Instance.ActiveCheck(true, true);
    }

    /// <summary>
    /// 이전 위치 및 셋팅으로 변경
    /// </summary>
    public void SetImageOff()
    {
        if (Statics.selectType == SelectType.Hand)
        {
            for (int i = 0; i < previewFinger.Length; i++)
            {
                previewFinger[i].SetActive(true);

                RectTransform rectTransform = previewFinger[i].GetComponent<RectTransform>();
                rectTransform.localScale = scales[i];
                rectTransform.localPosition = poss[i];
            }

            fingerPatternItems[fingerNum].color = new Color(fingerPatternItems[fingerNum].color.r, fingerPatternItems[fingerNum].color.g, fingerPatternItems[fingerNum].color.b, 1);
            for(int i = 0; i < fingerCharacterStickerRoots[fingerNum].images.Count; i++)
            {
                Image image = fingerCharacterStickerRoots[fingerNum].images[i];
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
            for (int i = 0; i < fingerStickerRoots[fingerNum].images.Count; i++)
            {
                Image image = fingerStickerRoots[fingerNum].images[i];
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
            for (int i = 0; i < fingerAnimationStickerRoots[fingerNum].images.Count; i++)
            {
                Image image = fingerAnimationStickerRoots[fingerNum].images[i];
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
        }
        else if (Statics.selectType == SelectType.Foot)
        {
            footImage.gameObject.SetActive(true);
            for (int i = 0; i < previewToes.Length; i++)
            {
                previewToes[i].SetActive(true);

                RectTransform rectTransform = previewToes[i].GetComponent<RectTransform>();
                rectTransform.localScale = scales[i];
                rectTransform.rotation = Quaternion.Euler(0, 0, rotZs[i]);
                rectTransform.localPosition = poss[i];
            }

            toesPatternItems[fingerNum].color = new Color(toesPatternItems[fingerNum].color.r, toesPatternItems[fingerNum].color.g, toesPatternItems[fingerNum].color.b, 1);
            for (int i = 0; i < toesCharacterStickerRoots[fingerNum].images.Count; i++)
            {
                Image image = toesCharacterStickerRoots[fingerNum].images[i];
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
            for (int i = 0; i < toesStickerRoots[fingerNum].images.Count; i++)
            {
                Image image = toesStickerRoots[fingerNum].images[i];
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
            for (int i = 0; i < toesAnimationStickerRoots[fingerNum].images.Count; i++)
            {
                Image image = toesAnimationStickerRoots[fingerNum].images[i];
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
        }

        QuestSceneManager.Instance.ActiveCheck(true, false);
    }

    /// <summary>
    /// step별 이미지 보이기
    /// </summary>
    /// <param name="step"></param>
    public void ShowPreviewStep(int step, bool isContinue)
    {
        Image pattern = null;
        List<Image> characterStickers = new List<Image>();
        List<Image> stickers = new List<Image>();
        List<Image> animationStickers = new List<Image>();
        if (Statics.selectType == SelectType.Hand)
        {
            pattern = fingerPatternItems[fingerNum];
            characterStickers = fingerCharacterStickerRoots[fingerNum].images;
            stickers = fingerStickerRoots[fingerNum].images;
            animationStickers = fingerAnimationStickerRoots[fingerNum].images;
        }
        else
        {
            pattern = toesPatternItems[fingerNum];
            characterStickers = toesCharacterStickerRoots[fingerNum].images;
            stickers = toesStickerRoots[fingerNum].images;
            animationStickers = toesAnimationStickerRoots[fingerNum].images;
        }
        switch (step)
        {
            //그리기
            case 0:
                StartCoroutine(PatternStep(pattern, false, 1));
                StartCoroutine(CharacterStickerStep(characterStickers, false, 1));
                StartCoroutine(StickerStep(stickers, false, 1));
                StartCoroutine(AnimationStickerStep(animationStickers, false, 1));
                break;
            //패턴
            case 1:
                StartCoroutine(PatternStep(pattern, true, 0));
                if (isContinue == false)
                {
                    StartCoroutine(CharacterStickerStep(characterStickers, false, 1));
                    StartCoroutine(StickerStep(stickers, false, 1));
                    StartCoroutine(AnimationStickerStep(animationStickers, false, 1));
                }
                break;
            //캐릭터 스티커
            case 2:
                StartCoroutine(CharacterStickerStep(characterStickers, true, 0));
                if (isContinue == false)
                {
                    StartCoroutine(PatternStep(pattern, true, 0));
                    StartCoroutine(StickerStep(stickers, false, 1));
                    StartCoroutine(AnimationStickerStep(animationStickers, false, 1));
                }
                break;
            //스티커
            case 3:
                if (isContinue == false)
                {
                    StartCoroutine(PatternStep(pattern, true, 0));
                    StartCoroutine(CharacterStickerStep(characterStickers, true, 0));
                    StartCoroutine(AnimationStickerStep(animationStickers, false, 1));
                }
                StartCoroutine(StickerStep(stickers, true, 0));
                break;
            //애니메이션스티커
            case 4:
                if (isContinue == false)
                {
                    StartCoroutine(PatternStep(pattern, true, 0));
                    StartCoroutine(CharacterStickerStep(characterStickers, true, 0));
                    StartCoroutine(AnimationStickerStep(stickers, false, 1));
                }
                StartCoroutine(AnimationStickerStep(animationStickers, true, 0));
                break;
        }
    }

    IEnumerator PatternStep(Image pattern, bool isShow, float time)
    {
        yield return new WaitForSeconds(0.25f);
        if (isShow == true)
        {
            while (time < 1)
            {
                time += Time.deltaTime * 2;
                pattern.color = new Color(pattern.color.r, pattern.color.g, pattern.color.b, time);
                yield return null;
            }
            pattern.color = new Color(pattern.color.r, pattern.color.g, pattern.color.b, 1);
        }
        else if (isShow == false)
        {
            while (time > 0)
            {
                time -= Time.deltaTime * 2;
                pattern.color = new Color(pattern.color.r, pattern.color.g, pattern.color.b, time);
                yield return null;
            }
            pattern.color = new Color(pattern.color.r, pattern.color.g, pattern.color.b, 0);
        }
    }

    IEnumerator CharacterStickerStep(List<Image> characterStickers, bool isShow, float time)
    {
        yield return new WaitForSeconds(0.25f);
        if (isShow == true)
        {
            while (time < 1)
            {
                time += Time.deltaTime * 2;
                for(int i = 0; i < characterStickers.Count; i++)
                {
                    characterStickers[i].color = new Color(characterStickers[i].color.r, characterStickers[i].color.g, characterStickers[i].color.b, time);
                }
                yield return null;
            }
            for (int i = 0; i < characterStickers.Count; i++)
            {
                characterStickers[i].color = new Color(characterStickers[i].color.r, characterStickers[i].color.g, characterStickers[i].color.b, 1);
            }
        }
        else if (isShow == false)
        {
            while (time > 0)
            {
                time -= Time.deltaTime * 2;
                for (int i = 0; i < characterStickers.Count; i++)
                {
                    characterStickers[i].color = new Color(characterStickers[i].color.r, characterStickers[i].color.g, characterStickers[i].color.b, time);
                }
                yield return null;
            }
            for (int i = 0; i < characterStickers.Count; i++)
            {
                characterStickers[i].color = new Color(characterStickers[i].color.r, characterStickers[i].color.g, characterStickers[i].color.b, 0);
            }
        }
    }

    IEnumerator StickerStep(List<Image> stickers, bool isShow, float time)
    {
        yield return new WaitForSeconds(0.25f);
        if (isShow == true)
        {
            while (time < 1)
            {
                time += Time.deltaTime * 2;
                for (int i = 0; i < stickers.Count; i++)
                {
                    stickers[i].color = new Color(stickers[i].color.r, stickers[i].color.g, stickers[i].color.b, time);
                }
                yield return null;
            }
            for (int i = 0; i < stickers.Count; i++)
            {
                stickers[i].color = new Color(stickers[i].color.r, stickers[i].color.g, stickers[i].color.b, 1);
            }
        }
        else if (isShow == false)
        {
            while (time > 0)
            {
                time -= Time.deltaTime * 2;
                for (int i = 0; i < stickers.Count; i++)
                {
                    stickers[i].color = new Color(stickers[i].color.r, stickers[i].color.g, stickers[i].color.b, time);
                }
                yield return null;
            }
            for (int i = 0; i < stickers.Count; i++)
            {
                stickers[i].color = new Color(stickers[i].color.r, stickers[i].color.g, stickers[i].color.b, 0);
            }
        }
    }

    IEnumerator AnimationStickerStep(List<Image> animationStickers, bool isShow, float time)
    {
        yield return new WaitForSeconds(0.25f);
        if (isShow == true)
        {
            while (time < 1)
            {
                time += Time.deltaTime * 2;
                for (int i = 0; i < animationStickers.Count; i++)
                {
                    animationStickers[i].color = new Color(animationStickers[i].color.r, animationStickers[i].color.g, animationStickers[i].color.b, time);
                }
                yield return null;
            }
            for (int i = 0; i < animationStickers.Count; i++)
            {
                animationStickers[i].color = new Color(animationStickers[i].color.r, animationStickers[i].color.g, animationStickers[i].color.b, 1);
            }
        }
        else if (isShow == false)
        {
            while (time > 0)
            {
                time -= Time.deltaTime * 2;
                for (int i = 0; i < animationStickers.Count; i++)
                {
                    animationStickers[i].color = new Color(animationStickers[i].color.r, animationStickers[i].color.g, animationStickers[i].color.b, time);
                }
                yield return null;
            }
            for (int i = 0; i < animationStickers.Count; i++)
            {
                animationStickers[i].color = new Color(animationStickers[i].color.r, animationStickers[i].color.g, animationStickers[i].color.b, 0);
            }
        }
    }

    /// <summary>
    /// 프리뷰 패널 사이즈 변경 
    /// </summary>
    public void ChangePreViewSize()
    {
        SoundManager.Instance.PlayEffectSound("Preview");

        //패널사이즈가 원본사이즈일 경우
        if (isBase == false)
        {
            boxCollider2D.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            StopCoroutine(MoveAnimation(true));
            StartCoroutine(MoveAnimation(true));
        }
        //패널사이즈가 원본사이즈가 아닐경우
        else if (isBase == true)
        {
            boxCollider2D.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StopCoroutine(MoveAnimation(false));
            StartCoroutine(MoveAnimation(false));
        }
    }

    IEnumerator MoveAnimation(bool value)
    {
        if (value == true)
        {
            QuestSceneManager.Instance.ActiveCheck(false, false);
            while (animPlayTime <= 0.5f)
            {
                animPlayTime += Time.deltaTime;

                float valuef = preViewAnimation.Evaluate(animPlayTime);
                float sizef = buttonAnimation.Evaluate(animPlayTime);
                preViewRect.localScale = new Vector3(1 + ((((canvasRect.sizeDelta.y - 154) / preViewRect.sizeDelta.y) - 1) * valuef), 1 + ((((canvasRect.sizeDelta.y - 154) / preViewRect.sizeDelta.y) - 1) * valuef));
                panelRect.sizeDelta = new Vector2(canvasRect.sizeDelta.x * sizef, canvasRect.sizeDelta.y * sizef);
                boxCollider2D.offset = new Vector2(boxColoffset.x - (((canvasRect.sizeDelta.x + (boxColoffset.x * 2)) / 2) * valuef), boxColoffset.y - (((canvasRect.sizeDelta.y + (boxColoffset.y * 2)) / 2) * valuef));
                boxCollider2D.size = new Vector3(boxColsize.x + ((canvasRect.sizeDelta.x - boxColsize.x) * valuef), boxColsize.y + ((canvasRect.sizeDelta.y - boxColsize.y) * valuef));
                yield return null;
            }

            animPlayTime = 0.5f;
            isBase = true;
        }
        else if (value == false)
        {
            while (animPlayTime >= 0)
            {
                animPlayTime -= Time.deltaTime;

                float valuef = preViewAnimation.Evaluate(animPlayTime);
                float sizef = buttonAnimation.Evaluate(animPlayTime);
                preViewRect.localScale = new Vector3(1 + ((((canvasRect.sizeDelta.y - 154) / preViewRect.sizeDelta.y) - 1) * valuef), 1 + ((((canvasRect.sizeDelta.y - 154) / preViewRect.sizeDelta.y) - 1) * valuef));
                panelRect.sizeDelta = new Vector2(canvasRect.sizeDelta.x * sizef, canvasRect.sizeDelta.y * sizef);
                boxCollider2D.offset = new Vector2(boxColoffset.x - (((canvasRect.sizeDelta.x + (boxColoffset.x * 2)) / 2) * valuef), boxColoffset.y - (((canvasRect.sizeDelta.y + (boxColoffset.y * 2)) / 2) * valuef));
                boxCollider2D.size = new Vector3(boxColsize.x + ((canvasRect.sizeDelta.x - boxColsize.x) * valuef), boxColsize.y + ((canvasRect.sizeDelta.y - boxColsize.y) * valuef));
                yield return null;
            }
            QuestSceneManager.Instance.ActiveCheck(false, true);
            animPlayTime = 0f;
            isBase = false;
        }
    }

    /// <summary>
    /// 클리어 애니메이션 실행
    /// </summary>
    public void ClearAnim()
    {
        StopCoroutine(IClearAnim());
        StartCoroutine(IClearAnim());
    }

    IEnumerator IClearAnim()
    {
        float time = 0;
        float AllTime = 2f;
        bool rupe = true;
        bool soundCheck = false;
        QuestSceneManager.Instance.ActiveCheck(true, false);
        SoundManager.Instance.PlayEffectSound("Preview");
        while (rupe)
        {
            time += Time.deltaTime;
            if(time > 1.3f && !soundCheck)
            {
                SoundManager.Instance.PlayEffectSound("Preview");
                soundCheck = true;
            }
            float scalef = clearAnimation.Evaluate(time);
            float valuef = clearAnimation.Evaluate(time);
            preViewRect.localScale = new Vector2(scalef, scalef);
            panelRect.sizeDelta = new Vector2(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y);
            if (time >= AllTime)
            {
                rupe = false;
            }
            yield return null;
        }
        preViewRect.localScale = new Vector2(clearAnimation.Evaluate(time), clearAnimation.Evaluate(time));
        panelRect.sizeDelta = new Vector2(0, 0);
        if(boxCollider2D.enabled == true)
        {
            boxCollider2D.offset = boxColoffset;
            boxCollider2D.size = boxColsize;
            boxCollider2D.enabled = false;
        }
        isBase = false;
        this.animPlayTime = 0;
    }

    /// <summary>
    /// 클리어 마크 생성
    /// </summary>
    /// <param name="clearfinger"></param>
    public void ShowClearMark(int clearfinger)
    {
        SetImageOff();
        if (Statics.selectType == SelectType.Hand)
        {
            StartCoroutine(IShowClearMark(fingerClearMarks[clearfinger]));
        }
        else if (Statics.selectType == SelectType.Foot)
        {
            StartCoroutine(IShowClearMark(toesClearMarks[clearfinger]));
        }
    }

    IEnumerator IShowClearMark(RectTransform rectTransform)
    {
        yield return new WaitForSeconds(0.75f);
        float time = 0;
        SoundManager.Instance.PlayEffectSound("Preview2");
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            float scale = clearMarkAnimation.Evaluate(time);
            rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }
}