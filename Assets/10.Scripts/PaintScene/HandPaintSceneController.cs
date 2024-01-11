using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandPaintSceneController : MonoBehaviour, IFreeable
{
    [Header("Behaviour")]
    [SerializeField] private ManicureItemScrollView manicureItemScrollView;
    [SerializeField] private FreeAnimController freeAnimController;
    [SerializeField] private BeautyItemSpawner beautyItemSpawner;
    [SerializeField] private PatternColorScroll patternColorScroll;
    [SerializeField] private PaintBrushCreater paintBrushCreater;
    [SerializeField] private FingerToesParticlePositioner fingerToesParticlePositioner;

    [Header("PaintEngine")]
    public AdvancedMobilePaint[] fingerPaintEngines;
    public AdvancedMobilePaint[] PaintEngines
    {
        get { return fingerPaintEngines; }
    }
    private AdvancedMobilePaint selectPaintEngine;
    public AdvancedMobilePaint SelectPaintEngine
    {
        get { return selectPaintEngine; }
    }
    private int selectFingerIndex;
    public int SelectFingerIndex
    {
        get { return selectFingerIndex; }
    }

    [Header("Nail")]
    [SerializeField] private Button[] fingerNailButtons;
    public int[] paintSteps;

    [Header("Pattern")]
    public List<PatternItems> patternItems;
    public List<PatternItems> PatternItems
    {
        get { return patternItems; }
        set { patternItems = value; }
    }
    public GameObject patternSlider;
    private GameObject patternClone;
    public GameObject PatternClone
    {
        set { patternClone = value; }
    }
    private Color patternColor = Color.white;
    public float[] patternSizes;
    public float[] PatternSizes
    {
        get { return patternSizes; }
        set { patternSizes = value; }
    }
    public int[] patternAlphaNumbers;
    public int[] PatternAlphaNumbers
    {
        get { return patternAlphaNumbers; }
        set { patternAlphaNumbers = value; }
    }

    [Header("StickerItem")]
    [SerializeField] private List<CharacterStickerItems> characterStickerItems;
    public List<CharacterStickerItems> CharacterStickerItems
    {
        get { return characterStickerItems; }
        set { characterStickerItems = value; }
    }
    [SerializeField] private List<StickerItems> stickerItems;
    public List<StickerItems> StickerItems
    {
        get { return stickerItems; }
        set { stickerItems = value; }
    }
    [SerializeField] private List<AnimationStickerItems> animationStickerItems;
    public List<AnimationStickerItems> AnimationStickerItems
    {
        get { return animationStickerItems; }
        set { animationStickerItems = value; }
    }
    [SerializeField] private int[] characterStickerCounts;
    public int[] CharacterStickerCounts
    {
        get { return characterStickerCounts; }
        set { characterStickerCounts = value; }
    }
    [SerializeField] private int[] stickerCounts;
    public int[] StickerCounts
    {
        get { return stickerCounts; }
        set { stickerCounts = value; }
    }
    [SerializeField] private int[] animationStickerCounts;
    public int[] AnimationStickerCounts
    {
        get { return animationStickerCounts; }
        set { animationStickerCounts = value; }
    }

    [SerializeField] private Transform editParent;

    private List<int> selectFingerCount;
    public List<int> SelectFingerCount
    {
        get { return selectFingerCount; }
    }

    private List<bool> isClicks;

    private bool isFirst;
    public bool IsFirtst
    {
        get { return isFirst; }
        set { isFirst = value; }
    }
    private bool isPaint;
    public bool IsPaint
    {
        get { return isPaint; }
        set { isPaint = value; }
    }

    private ManicureInfoData manicureInfoData;
    public ManicureInfoData ManicureInfoData
    {
        get { return manicureInfoData; }
        set { manicureInfoData = value; }
    }

    GameObject IFreeable.patternSlider()
    {
        return patternSlider;
    }

    /// <summary>
    /// 초기값 설정 
    /// </summary>
    public void Init()
    {
        selectFingerCount = new List<int>();

        isClicks = new List<bool>();
        for (int i = 0; i < 5; i++)
        {
            isClicks.Add(false);
            fingerNailButtons[i].interactable = false;
        }
    }

    /// <summary>
    /// 게임씬에서 메인씬으로 이동
    /// </summary>
    /// <param name="isInit"></param>
    public void MoveToMainScene(bool isInit)
    {
        AllReset();
        isFirst = false;
        PaintSceneManager.Instance.UnregisterFreeable(this);
        MainSceneController.Instance.MainSceneOn(isInit);
        PaintSceneManager.Instance.Hide();
    }

    /// <summary>
    /// 전체 초기화 
    /// </summary>
    public void AllReset()
    {
        //메인타입 이미지 초기화
        patternSlider.gameObject.SetActive(false);

        //UI애니메이션 초기화
        freeAnimController.SceneName = "pattern";

        //마스크 초기화 
        for (int i = 0; i < fingerPaintEngines.Length; i++)
        {
            RawImage rawImage = fingerPaintEngines[i].GetComponent<RawImage>();
            rawImage.texture = null;
            fingerPaintEngines[i].tex = null;
            fingerPaintEngines[i].IsInit = false;
            fingerPaintEngines[i].gameObject.SetActive(false);
        }

        //메니큐어 리스트 초기화 
        manicureItemScrollView.Resets();

        //패턴 초기화
        for (int i = 0; i < 5; i++)
        {
            if (patternItems[i].patternConverter != null)
            {
                Destroy(patternItems[i].patternConverter.gameObject);
                patternItems[i].patternConverter = null;
            }

            patternSizes[i] = 0.5f;
        }
        patternClone = null;

        //스티커 초기화
        for (int i = 0; i < characterStickerItems.Count; i++)
        {
            for (int j = 0; j < characterStickerItems[i].stickerConverters.Count; j++)
            {
                Destroy(characterStickerItems[i].stickerConverters[j].gameObject);
            }
            characterStickerItems[i].stickerConverters = new List<StickerConverter>();
        }

        for (int i = 0; i < stickerItems.Count; i++)
        {
            for (int j = 0; j < stickerItems[i].stickerConverters.Count; j++)
            {
                Destroy(stickerItems[i].stickerConverters[j].gameObject);
            }
            stickerItems[i].stickerConverters = new List<StickerConverter>();
        }

        for (int i = 0; i < animationStickerItems.Count; i++)
        {
            for (int j = 0; j < animationStickerItems[i].stickerConverters.Count; j++)
            {
                Destroy(animationStickerItems[i].stickerConverters[j].gameObject);
            }
            animationStickerItems[i].stickerConverters = new List<StickerConverter>();
        }

        for (int i = 0; i < stickerCounts.Length; i++)
        {
            stickerCounts[i] = 0;
            characterStickerCounts[i] = 0;
            animationStickerCounts[i] = 0;
        }

        //페인트 단계 초기화 
        for (int i = 0; i < paintSteps.Length; i++)
        {
            paintSteps[i] = 0;
        }

        fingerToesParticlePositioner.DestroyParticle();

        SetDrawBrushOnOff(false);
        ParticleManager.Instance.DestroyParticle();
    }

    /// <summary>
    /// 매니큐어 선택시 매니큐어의 정보를 paintEngine스크립트로 넘겨줌
    /// </summary>
    /// <param name="manicureName"></param>
    /// <param name="drawMode"></param>
    /// <param name="brushSize"></param>
    /// <param name="brushColor"></param>
    /// <param name="pattern"></param>
    /// <param name="isAditiveBrush"></param>
    /// <param name="brushCanDrawOnBlack"></param>
    /// <param name="usesLockMasks"></param>
    /// <param name="drawEnable"></param>
    public void SetVectorBrush(string manicureName, DrawMode drawMode, int brushSize, Color brushColor, Texture2D pattern, bool isAditiveBrush, bool brushCanDrawOnBlack, bool usesLockMasks, bool drawEnable)
    {
        selectPaintEngine.SetVectorBrush(manicureName, drawMode, brushSize, brushColor, pattern, isAditiveBrush, brushCanDrawOnBlack, usesLockMasks, drawEnable);
    }

    /// <summary>
    /// 드로우 모드 셋팅
    /// </summary>
    public void SetDrawMode()
    {
        if (selectPaintEngine != null)
        {
            paintBrushCreater.SetDrawMode(selectPaintEngine.drawMode);
        }
    }

    /// <summary>
    /// 브러쉬 활성화 여부
    /// </summary>
    /// <param name="isOn"></param>
    public void SetDrawBrushOnOff(bool isOn)
    {
        paintBrushCreater.IsInit = isOn;
        paintBrushCreater.Freeable = this;
    }

    /// <summary>
    /// 패턴 디자인 선택시 패턴 디자인 설정
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="scale"></param>
    /// <param name="manicureInfoData"></param>
    public void PatternItem(Sprite sprite, Vector3 scale, ManicureInfoData manicureInfoData)
    {
        selectPaintEngine.drawEnabled = false;
        patternColorScroll.InitBaseColor(manicureInfoData);

        Color color = new Color(patternColor.r, patternColor.g, patternColor.b, patternAlphaNumbers[selectFingerIndex] * 0.25f);
        if (patternItems[selectFingerIndex].patternConverter == null)
        {
            if (manicureInfoData.option == "DESTROY")
            {
                return;
            }
            beautyItemSpawner.CreatePattern(patternItems[selectFingerIndex].patternItemParent, sprite, color, scale, null, this);
        }
        else
        {
            PatternConverter patternConverter = patternItems[selectFingerIndex].patternConverter;
            if (manicureInfoData.option == "DESTROY")
            {
                if (patternSlider.activeSelf == true)
                {
                    patternSlider.SetActive(false);
                }
                Destroy(patternConverter.gameObject);
                return;
            }
            patternConverter.Init(sprite, color, scale);
            patternClone = patternConverter.gameObject;
        }
        patternSlider.SetActive(true);
    }

    /// <summary>
    /// 패턴 슬라이더 활성화 여부
    /// </summary>
    public void OnOffPatternSlider()
    {
        if (patternItems[selectFingerIndex].patternConverter != null)
        {
            patternSlider.SetActive(true);
            patternClone = patternItems[selectFingerIndex].patternConverter.gameObject;
        }
        else
        {
            patternSlider.SetActive(false);
        }
    }

    /// <summary>
    /// 스티커 선택시 손톱에 스티커 생성
    /// </summary>
    /// <param name="manicureInfoData"></param>
    public void CreateStickerItem(ManicureInfoData manicureInfoData)
    {
        selectPaintEngine.drawEnabled = false;

        if (StickerConverter.currentStickerConverter != null)
        {
            StickerConverter.currentStickerConverter.DeselectItem();
        }

        string sceneName = GetSceneName();
        if (sceneName == "sticker")
        {
            if (characterStickerCounts[selectFingerIndex] >= 5)
            {
                return;
            }

            beautyItemSpawner.CreateSticker(characterStickerItems[selectFingerIndex].characterStickerItemParent, selectPaintEngine, manicureInfoData, null, this);
            characterStickerCounts[selectFingerIndex]++;
        }
        else if (sceneName == "animationSticker")
        {
            if (stickerCounts[selectFingerIndex] >= 5)
            {
                return;
            }

            beautyItemSpawner.CreateSticker(stickerItems[selectFingerIndex].stickerItemParent, selectPaintEngine, manicureInfoData, null, this);
            stickerCounts[selectFingerIndex]++;
        }
        else if (sceneName == "selectFinger")
        {
            if (animationStickerCounts[selectFingerIndex] >= 5)
            {
                return;
            }

            beautyItemSpawner.CreateSticker(animationStickerItems[selectFingerIndex].AnimationStickerItemParent, selectPaintEngine, manicureInfoData, null, this);
            animationStickerCounts[selectFingerIndex]++;
        }
    }

    /// <summary>
    /// 스티커 아이템 미선택시 실행
    /// </summary>
    /// <param name="stickerObject"></param>
    public void StickerItemDeselect(StickerConverter stickerObject)
    {
        if (stickerObject.stickerType == StickerType.CharacterSticker)
        {
            //RectTransform rectTransform = stickerObject.GetComponent<RectTransform>();
            //rectTransform.localScale = Vector3.one;
        }
        else if (stickerObject.stickerType == StickerType.Sticker)
        {

        }
        else
        {
            RectTransform rectTransform = stickerObject.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// 편집모드 들어갈시 UI 오더갑 설정을 위해 부모를 변경
    /// </summary>
    public void ChangeEidtParent(StickerConverter stickerObject)
    {
        stickerObject.transform.SetParent(editParent);
        //Vector3 originPos = stickerObject.GetComponent<RectTransform>().anchoredPosition;
        //stickerObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(originPos.x, originPos.y, 0.0f);
    }


    /// <summary>
    /// 패턴 사이즈 변경
    /// </summary>
    /// <param name="value"></param>
    public void ChageSizePatternObject(float value)
    {
        if (patternClone == null)
        {
            return;
        }
        patternClone.transform.localScale = new Vector3(value, value, value);
    }

    /// <summary>
    /// 패턴 투명도 변경
    /// </summary>
    /// <param name="alphaNumber"></param>
    public void ChageAlphaPatternObject(int alphaNumber)
    {
        if (patternClone == null)
        {
            return;
        }
        Image image = patternClone.transform.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, alphaNumber * 0.25f);

        patternAlphaNumbers[selectFingerIndex] = alphaNumber;
    }

    /// <summary>
    /// 패턴 색상 변경
    /// </summary>
    /// <param name="id"></param>
    /// <param name="color"></param>
    public void ChageColorPatternObject(int id, Color color)
    {
        patternColor = color;
        if (patternClone == null)
        {

        }
        else
        {
            Image image = patternClone.transform.GetComponent<Image>();
            image.color = new Color(patternColor.r, patternColor.g, patternColor.b, patternAlphaNumbers[selectFingerIndex] * 0.25f);
        }
    }

    /// <summary>
    /// 매니큐어 아이템 선택
    /// </summary>
    public void SelectBeautyItem()
    {
        ManicureConverter manicureConverter = null;
        ManicureConverter baseManicureConverter = null;
        List<ManicureConverter> manicureConverters;

        manicureConverters = manicureItemScrollView.ManicureConverters;
        for (int i = 0; i < manicureConverters.Count; i++)
        {
            ManicureConverter converter = manicureConverters[i];

            if (converter.ManicureInfoData.id == 1)
            {
                baseManicureConverter = converter;
            }
            if (converter.ManicureInfoData.id == Statics.selectManicureId)
            {
                if (converter.ManicureInfoData.type == "Manicure")
                {
                    manicureConverter = converter;
                }
            }
        }

        if (manicureConverter == null)
        {
            baseManicureConverter.SetUpPaint();
        }
        else
        {
            manicureConverter.SetUpPaint();
        }
    }

    /// <summary>
    /// 애니메이션 작동 완료후 paintEngine의 그리기모드 true로 변경후 선택한 손가락의 paintEngine으로 정보변경
    /// </summary>
    /// <param name="fingerNum"></param>
    public void SelectFinger(int fingerNum)
    {
        isClicks[fingerNum] = true;
        if (!selectFingerCount.Contains(fingerNum))
        {
            selectFingerCount.Add(fingerNum);
        }

        fingerToesParticlePositioner.ActiveOnOffParticle(0, true, false);
        fingerNailButtons[fingerNum].interactable = false;

        patternClone = null;

        PaintSelectFinger(fingerNum);
    }

    /// <summary>
    /// paintEngine 빈값으로 변경후 그리기모드 false로 변경
    /// </summary>
    public void CancleFinger()
    {
        isPaint = false;

        if (selectPaintEngine != null)
        {
            if (paintBrushCreater.BrushImage != null)
            {
                Destroy(paintBrushCreater.BrushImage);
            }

            selectPaintEngine.movable = false;
            selectPaintEngine.drawEnabled = false;
        }
        freeAnimController.SceneName = "pattern";
        selectPaintEngine = null;

        patternClone = null;

        StickerItemRayCastOnOff(null);
        //파티클 생성
        ParticleOn();
        CreateClickedParticle();
    }

    /// <summary>
    /// 버튼 활성화 여부
    /// </summary>
    /// <param name="isOn"></param>
    public void OnOffAllInterectable(bool isOn)
    {
        for (int i = 0; i < 5; i++)
        {
            fingerNailButtons[i].interactable = isOn;
        }
    }

    /// <summary>
    /// 파티클 생성
    /// </summary>
    private void ParticleOn()
    {
        StopCoroutine(IParticleOn());
        StartCoroutine(IParticleOn());
    }

    IEnumerator IParticleOn()
    {
        yield return new WaitForSeconds(1.65f);
        for (int i = 0; i < 5; i++)
        {
            if (isClicks[i] == true)
            {
                fingerToesParticlePositioner.ActiveOnOffParticle(i, false, false);
            }
            else if (isClicks[i] == false)
            {
                fingerToesParticlePositioner.ActiveOnOffParticle(i, false, true);
            }
        }
    }

    /// <summary>
    /// 클릭한 손가락에 파티클 생성
    /// </summary>
    private void CreateClickedParticle()
    {
        StopCoroutine(ICreateClickedParticle());
        StartCoroutine(ICreateClickedParticle());
    }

    IEnumerator ICreateClickedParticle()
    {
        yield return new WaitForSeconds(1.65f);
        fingerToesParticlePositioner.CreateCelearParticle(selectFingerIndex);
    }

    public void NailMaskSetUp()
    {
        StopCoroutine(INailMaskSetUp());
        StartCoroutine(INailMaskSetUp());
    }

    IEnumerator INailMaskSetUp()
    {
        yield return new WaitForSeconds(0.3f);
        NailSetting();
    }

    /// <summary>
    /// 브러쉬 사이즈 조절
    /// </summary>
    /// <param name="size"></param>
    public void ChangeBrushSize(int size)
    {
        selectPaintEngine.ChangeBrushSize(size);

    }

    /// <summary>
    /// 지우개 셋팅
    /// </summary>
    public void SetUpEraser()
    {
        selectPaintEngine.SetupEraser();
    }

    /// <summary>
    /// 매니큐어 아이템리스트 변경
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeManicureItemList(string sceneName)
    {
        switch (sceneName)
        {
            case "ManicureScene":
                PatternSlideSetActive();
                manicureItemScrollView.ResetContent("Manicure");
                manicureItemScrollView.PlayMoveContentBox();
                break;
            case "PatternScene":
                selectPaintEngine.drawEnabled = false;
                manicureItemScrollView.ResetContent("Pattern");
                manicureItemScrollView.PlayMoveContentBox();
                break;
            case "characterStickerScene":
                selectPaintEngine.drawEnabled = false;
                PatternSlideSetActive();
                manicureItemScrollView.ResetContent("CharacterSticker");
                manicureItemScrollView.PlayMoveContentBox();
                break;
            case "stickerScene":
                selectPaintEngine.drawEnabled = false;
                manicureItemScrollView.ResetContent("Sticker");
                manicureItemScrollView.PlayMoveContentBox();
                break;
            case "animationStickerScene":
                selectPaintEngine.drawEnabled = false;
                manicureItemScrollView.ResetContent("AnimationSticker");
                manicureItemScrollView.PlayMoveContentBox();
                break;
            default:
                break;
        }
        StickerItemRayCastOnOff(sceneName);
    }

    /// <summary>
    /// 스티커 아이템 움직일수 있는지 여부
    /// </summary>
    /// <param name="value"></param>
    public void ChangeStickerObjectStatus(bool value)
    {
        selectPaintEngine.movable = value;
    }

    /// <summary>
    /// 다음, 이전 버튼 누를때마다 해당손가락에 단계를 저장
    /// </summary>
    /// <param name="selectFinger"></param>
    /// <param name="fingerStep"></param>
    public void FingerStepSetUp(int selectFinger, int fingerStep)
    {
        paintSteps[selectFinger] = fingerStep;
    }

    /// <summary>
    /// 다음 버튼 및 손가락 선택 버튼 누를시 패턴 슬라이더가 기본값으로 초기화
    /// </summary>
    public void PatternSlideSetActive()
    {
        if (patternSlider.gameObject.activeSelf == true)
        {
            patternSlider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 손가락 선택
    /// </summary>
    /// <param name="fingerNum"></param>
    public void PaintSelectFinger(int fingerNum)
    {
        //if (PaintSceneManager.Instance.Freeables == null)
        //{
        //    return;
        //}
        selectFingerIndex = fingerNum;
        selectPaintEngine = fingerPaintEngines[selectFingerIndex];
        freeAnimController.ClickFreeUIAnim();
    }

    /// <summary>
    /// 손가락 클릭시 해당 손가락에 해당하는 페인팅 스크립트 ON
    /// </summary>
    public void paintEngineObjectSetActive()
    {
        if (selectPaintEngine != null)
        {
            selectPaintEngine.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 배경버튼 선택시 클릭
    /// </summary>
    public void NailSetting()
    {
        PolygonCollider2D handPolygonCollider2D = PaintSceneManager.Instance.HandBg.gameObject.GetComponent<PolygonCollider2D>();
        handPolygonCollider2D.enabled = false;
        PolygonCollider2D footPolygonCollider2D = PaintSceneManager.Instance.FootBg.gameObject.GetComponent<PolygonCollider2D>();
        footPolygonCollider2D.enabled = false;

        FirstParticleOn();
    }

    private void FirstParticleOn()
    {
        StopCoroutine(IFirstParticleOn());
        StartCoroutine(IFirstParticleOn());
    }

    IEnumerator IFirstParticleOn()
    {
        yield return new WaitForSeconds(0.75f);
        fingerToesParticlePositioner.Init();
        for (int i = 0; i < fingerNailButtons.Length; i++)
        {
            fingerNailButtons[i].interactable = true;
        }
    }

    /// <summary>
    /// 스티커 아이템 레이캐스트 충돌 가능한지 여부
    /// </summary>
    /// <param name="sceneName"></param>
    public void StickerItemRayCastOnOff(string sceneName)
    {
        switch (sceneName)
        {
            case "ManicureScene":
                OnOffStickerCollider(false);
                StickerItemRatCastOnOffSetting(false, false, false);
                StickerItemExitEnter(false, false, false);
                break;
            case "PatternScene":
                OnOffStickerCollider(false);
                StickerItemRatCastOnOffSetting(false, false, false);
                StickerItemExitEnter(false, false, false);
                break;
            case "characterStickerScene":
                OnOffStickerCollider(true);
                StickerItemRatCastOnOffSetting(true, false, false);
                StickerItemExitEnter(true, false, false);
                break;
            case "stickerScene":
                OnOffStickerCollider(true);
                StickerItemRatCastOnOffSetting(false, true, false);
                StickerItemExitEnter(false, true, false);
                break;
            case "animationStickerScene":
                OnOffStickerCollider(true);
                StickerItemRatCastOnOffSetting(false, false, true);
                StickerItemExitEnter(false, false, true);
                break;
            default:
                OnOffStickerCollider(false);
                StickerItemRatCastOnOffSetting(false, false, false);
                StickerItemExitEnter(false, false, false);
                break;
        }
    }

    /// <summary>
    /// 콜라이더 온오프 
    /// </summary>
    /// <param name="isOn"></param>
    private void OnOffStickerCollider(bool isOn)
    {
        for (int i = 0; i < characterStickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            StickerConverter stickerConverter = characterStickerItems[selectFingerIndex].stickerConverters[i];
            stickerConverter.OnOffCollider(isOn);
        }
        for (int i = 0; i < stickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            StickerConverter stickerConverter = stickerItems[selectFingerIndex].stickerConverters[i];
            stickerConverter.OnOffCollider(isOn);
        }
        for (int i = 0; i < animationStickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            StickerConverter stickerConverter = animationStickerItems[selectFingerIndex].stickerConverters[i];
            stickerConverter.OnOffCollider(isOn);
        }
    }

    /// <summary>
    /// 레이캐스트타켓 온오프
    /// </summary>
    /// <param name="isCharacterSticker"></param>
    /// <param name="isSticker"></param>
    /// <param name="isAnimationSticker"></param>
    private void StickerItemRatCastOnOffSetting(bool isCharacterSticker, bool isSticker, bool isAnimationSticker)
    {
        for (int i = 0; i < characterStickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            GameObject obj = characterStickerItems[selectFingerIndex].stickerConverters[i].gameObject;
            obj.GetComponent<Image>().raycastTarget = isCharacterSticker;
        }
        for (int i = 0; i < stickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            GameObject obj = stickerItems[selectFingerIndex].stickerConverters[i].gameObject;
            obj.GetComponent<Image>().raycastTarget = isSticker;
        }
        for (int i = 0; i < animationStickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            GameObject obj = animationStickerItems[selectFingerIndex].stickerConverters[i].gameObject;
            obj.GetComponent<Image>().raycastTarget = isAnimationSticker;
        }
    }

    /// <summary>
    /// 스티커 아이템 선택 가능한지 여부
    /// </summary>
    /// <param name="isCharacterSticker"></param>
    /// <param name="isSticker"></param>
    /// <param name="isAnimationSticker"></param>
    private void StickerItemExitEnter(bool isCharacterSticker, bool isSticker, bool isAnimationSticker)
    {
        for (int i = 0; i < characterStickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            GameObject obj = characterStickerItems[selectFingerIndex].stickerConverters[i].gameObject;
            StickerConverter stickerConverter = obj.GetComponent<StickerConverter>();
            if (isCharacterSticker == true)
            {
                stickerConverter.EnterSticker();
            }
            else if (isCharacterSticker == false)
            {
                stickerConverter.ExitSticker();
            }
        }
        for (int i = 0; i < stickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            GameObject obj = stickerItems[selectFingerIndex].stickerConverters[i].gameObject;
            StickerConverter stickerConverter = obj.GetComponent<StickerConverter>();
            if (isSticker == true)
            {
                stickerConverter.EnterSticker();
            }
            else if (isSticker == false)
            {
                stickerConverter.ExitSticker();
            }
        }
        for (int i = 0; i < animationStickerItems[selectFingerIndex].stickerConverters.Count; i++)
        {
            GameObject obj = animationStickerItems[selectFingerIndex].stickerConverters[i].gameObject;
            StickerConverter stickerConverter = obj.GetComponent<StickerConverter>();
            if (isAnimationSticker == true)
            {
                stickerConverter.EnterSticker();
            }
            else if (isAnimationSticker == false)
            {
                stickerConverter.ExitSticker();
            }
        }
    }

    public void PatternGuideAnim()
    {
        patternSlider.GetComponent<PatternSizeBar>().GuideHandInit();
    }

    public string GetSceneName()
    {
        string sceneName = freeAnimController.SceneName;
        return sceneName;
    }
}