using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestFootPaintSceneController : MonoBehaviour, IQuestable
{
    [Header("Behaviour")]
    [SerializeField] private QuestSceneManager questScene;
    [SerializeField] private QuestPreview questPreview;
    [SerializeField] private QuestConfirmBehaviour questConfirmBehaviour;
    [SerializeField] private ManicureItemScrollView manicureItemScrollView;
    [SerializeField] private QuestAnimController questAnimController;
    [SerializeField] private BeautyItemSpawner beautyItemSpawner;
    [SerializeField] private PatternColorScroll patternColorScroll;
    [SerializeField] private PaintBrushCreater paintBrushCreater;
    [SerializeField] private FingerToesParticlePositioner fingerToesParticlePositioner;

    private Image handfootImage;
    public Image HandfootImage
    {
        get { return handfootImage; }
    }

    [Header("PaintEngine")]
    public AdvancedMobilePaint[] toesPaintEngines;
    public AdvancedMobilePaint[] PaintEngines
    {
        get { return toesPaintEngines; }
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
        set { selectFingerIndex = value; }
    }

    [Header("Nail")]
    [SerializeField] private Button[] toesNailButtons;
    private Image[] toesNailMasks;
    public int[] paintSteps;
    public int[] PaintSteps
    {
        get { return paintSteps; }
    }
    private List<int> clearFingers;
    public List<int> ClearFingers
    {
        get { return clearFingers; }
    }

    [Header("Pattern")]
    public List<PatternItems> patternItems;
    public List<PatternItems> PatternItems
    {
        get { return patternItems; }
        set { patternItems = value; }
    }

    private GameObject patternClone;
    public GameObject PatternClone
    {
        set { patternClone = value; }
    }
    private Color patternColor = Color.white;
    public Color PatternColor
    {
        get { return patternColor; }
        set { patternColor = value; }
    }

    [Header("Sticker")]
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
    private List<GameObject> guideStickerObjects;
    public List<GameObject> GuideStickerObjects
    {
        get { return guideStickerObjects; }
    }
    [SerializeField] private Transform editParent;


    [Header("Quest")]
    private int level;
    public int Level
    {
        get { return level; }
    }
    private bool isClear;
    public bool IsClear
    {
        get { return isClear; }
        set { isClear = value; }
    }

    private string sceneName;
    public string SceneName
    {
        get { return sceneName; }
    }

    private List<int> showClearMark = new List<int>();

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
    private bool isPatternScroll;
    public bool IsPatternScroll
    {
        get { return isPatternScroll; }
        set { isPatternScroll = value; }
    }
    private bool isChecking;
    public bool IsChecking
    {
        get { return isChecking; }
        set { isChecking = value; }
    }

    
    public void RegisterQuestable()
    {
        QuestSceneManager.Instance.RegisterQuestable(this);
    }

    public void UnregisterQuestable()
    {
        QuestSceneManager.Instance.UnregisterQuestable(this);
    }

    /// <summary>
    /// 초기값 설정
    /// </summary>
    /// <param name="level"></param>
    /// <param name="questHandData"></param>
    /// <param name="handImage"></param>
    /// <param name="toesNailMasks"></param>
    public void Init(int level, QuestHandData questHandData, Image handImage, Image[] toesNailMasks)
    {
        this.level = level;
        handfootImage = handImage;
        this.toesNailMasks = toesNailMasks;

        clearFingers = new List<int>();
        guideStickerObjects = new List<GameObject>();

        questConfirmBehaviour.Init(questHandData, this);

        StartCoroutine(FirstParticleOn());
    }

    IEnumerator FirstParticleOn()
    {
        yield return new WaitForSeconds(0.75f);
        fingerToesParticlePositioner.Init();
    }

    /// <summary>
    /// 게임신에서 메인씬으로 이동
    /// </summary>
    public void MoveToMainScene()
    {
        AllReset();
        isFirst = false;
        if (isClear == true)
        {
            MainSceneController.Instance.MainSceneOn(true);
            isClear = false;
        }
        else
        {
            MainSceneController.Instance.MainSceneOn(false);
        }

        questScene.Hide();
    }

    /// <summary>
    /// 전체 초기화 
    /// </summary>
    public void AllReset()
    {
        //전체 버튼 활성화
        for (int i = 0; i < toesNailButtons.Length; i++)
        {
            toesNailButtons[i].interactable = true;
        }

        //마스크 초기화 
        for (int i = 0; i < toesPaintEngines.Length; i++)
        {
            RawImage rawImage = toesPaintEngines[i].GetComponent<RawImage>();
            rawImage.texture = null;
            toesPaintEngines[i].tex = null;
            toesPaintEngines[i].IsInit = false;
            toesPaintEngines[i].gameObject.SetActive(false);
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
        }
        patternClone = null;

        //스티커 초기화
        for (int i = 0; i < characterStickerItems.Count; i++)
        {
            for (int j = 0; j < characterStickerItems[i].stickerConverters.Count; j++)
            {
                if (characterStickerItems[i].stickerConverters[j] != null)
                {
                    Destroy(characterStickerItems[i].stickerConverters[j].gameObject);
                }
            }
            characterStickerItems[i].stickerConverters = new List<StickerConverter>();

            for (int j = 0; j < characterStickerItems[i].guideStickerConverters.Count; j++)
            {
                if (characterStickerItems[i].guideStickerConverters[j] != null)
                {
                    Destroy(characterStickerItems[i].guideStickerConverters[j].gameObject);
                }
            }
            characterStickerItems[i].guideStickerConverters = new List<GuideStickerConverter>();
        }

        for (int i = 0; i < stickerItems.Count; i++)
        {
            for (int j = 0; j < stickerItems[i].stickerConverters.Count; j++)
            {
                if (stickerItems[i].stickerConverters[j] != null)
                {
                    Destroy(stickerItems[i].stickerConverters[j].gameObject);
                }
            }
            stickerItems[i].stickerConverters = new List<StickerConverter>();

            for (int j = 0; j < stickerItems[i].guideStickerConverters.Count; j++)
            {
                if (stickerItems[i].guideStickerConverters[j] != null)
                {
                    Destroy(stickerItems[i].guideStickerConverters[j].gameObject);
                }
            }
            stickerItems[i].guideStickerConverters = new List<GuideStickerConverter>();
        }

        for (int i = 0; i < animationStickerItems.Count; i++)
        {
            for (int j = 0; j < animationStickerItems[i].stickerConverters.Count; j++)
            {
                if (animationStickerItems[i].stickerConverters[j] != null)
                {
                    Destroy(animationStickerItems[i].stickerConverters[j].gameObject);
                }
            }
            animationStickerItems[i].stickerConverters = new List<StickerConverter>();

            for (int j = 0; j < animationStickerItems[i].guideStickerConverters.Count; j++)
            {
                if (animationStickerItems[i].guideStickerConverters[j] != null)
                {
                    Destroy(animationStickerItems[i].guideStickerConverters[j].gameObject);
                }
            }
            animationStickerItems[i].guideStickerConverters = new List<GuideStickerConverter>();
        }

        //페인트 단계 초기화 
        for (int i = 0; i < paintSteps.Length; i++)
        {
            paintSteps[i] = 0;
        }

        fingerToesParticlePositioner.DestroyParticle();

        SetDrawBrushOnOff(false);
        ParticleManager.Instance.DestroyParticle();
        UnregisterQuestable();
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
    /// 브러쉬 비활성화
    /// </summary>
    /// <param name="isOn"></param>
    public void SetDrawBrushOnOff(bool isOn)
    {
        paintBrushCreater.IsInit = isOn;
        paintBrushCreater.Questable = this;
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

        if (patternItems[selectFingerIndex].patternConverter == null)
        {
            //패턴아이템의 옵션이 DESTROY일경우 생성을 안함
            if (manicureInfoData.option == "DESTROY")
            {
                return;
            }
            beautyItemSpawner.CreatePattern(patternItems[selectFingerIndex].patternItemParent, sprite, patternColor, scale, this, null);
        }
        else
        {
            PatternConverter patternConverter = patternItems[selectFingerIndex].patternConverter;
            //패턴아이템의 옵션이 DESTROY일경우 생성을 안함
            if (manicureInfoData.option == "DESTROY")
            {
                Destroy(patternConverter.gameObject);
                return;
            }
            patternConverter.Init(sprite, patternColor, scale);
            patternClone = patternConverter.gameObject;
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

        if (sceneName == "characterStickerScene")
        {
            for (int i = 0; i < characterStickerItems[selectFingerIndex].stickerConverters.Count; i++)
            {
                StickerConverter stickerConverter = characterStickerItems[selectFingerIndex].stickerConverters[i];
                if (stickerConverter != null)
                {
                    if (stickerConverter.stickerType == StickerType.CharacterSticker)
                    {
                        stickerConverter.Hide();
                    }
                    characterStickerItems[selectFingerIndex].stickerConverters.RemoveAt(i);
                    characterStickerItems[selectFingerIndex].stickerConverters = new List<StickerConverter>();
                }
            }
            beautyItemSpawner.CreateSticker(characterStickerItems[selectFingerIndex].characterStickerItemParent, selectPaintEngine, manicureInfoData, this, null);
        }
        else if (sceneName == "stickerScene")
        {
            for (int i = 0; i < stickerItems[selectFingerIndex].stickerConverters.Count; i++)
            {
                StickerConverter stickerConverter = stickerItems[selectFingerIndex].stickerConverters[i];
                if (stickerConverter != null)
                {
                    if (stickerConverter.stickerType == StickerType.Sticker)
                    {
                        stickerConverter.Hide();
                    }
                    stickerItems[selectFingerIndex].stickerConverters.RemoveAt(i);
                    stickerItems[selectFingerIndex].stickerConverters = new List<StickerConverter>();
                }
            }
            beautyItemSpawner.CreateSticker(stickerItems[selectFingerIndex].stickerItemParent, selectPaintEngine, manicureInfoData, this, null);
        }
        else if (sceneName == "animationStickerScene")
        {
            for (int i = 0; i < animationStickerItems[selectFingerIndex].stickerConverters.Count; i++)
            {
                StickerConverter stickerConverter = animationStickerItems[selectFingerIndex].stickerConverters[i];
                if (stickerConverter != null)
                {
                    if (stickerConverter.stickerType == StickerType.AnimationSticker)
                    {
                        stickerConverter.Hide();
                    }
                    animationStickerItems[selectFingerIndex].stickerConverters.RemoveAt(i);
                    animationStickerItems[selectFingerIndex].stickerConverters = new List<StickerConverter>();
                }
            }
            beautyItemSpawner.CreateSticker(animationStickerItems[selectFingerIndex].AnimationStickerItemParent, selectPaintEngine, manicureInfoData, this, null);
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
        else if (stickerObject.stickerType == StickerType.AnimationSticker)
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
    /// 패턴 색상 변경
    /// </summary>
    /// <param name="color"></param>
    public void ChageColorPatternObject(Color color)
    {
        patternColor = color;
        if (patternClone == null)
        {

        }
        else
        {
            Image image = patternClone.transform.GetComponent<Image>();
            image.color = patternColor;
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

            //컨버터의 매니큐어아이디가 1일경우 베이스로 저장
            if (converter.ManicureInfoData.id == 1)
            {
                baseManicureConverter = converter;
            }
            //현재 선택된 매니큐어의 아이디와 같은 아이디를 가진 매니큐어 아이템이 있을경우
            if (converter.ManicureInfoData.id == Statics.selectManicureId)
            {
                //그 아이템의 타입이 매니큐어일경우 일반컨버터 저장
                if (converter.ManicureInfoData.type == "Manicure")
                {
                    manicureConverter = converter;
                }
            }
        }

        //일반컨버터가 null일경우
        if (manicureConverter == null)
        {
            //베이스가 null이 아니면 베이스로 셋팅
            if (baseManicureConverter != null)
            {
                baseManicureConverter.SetUpPaint();
            }
        }
        //일반컨버터가 null일 아니면 일반컨버터로 셋팅
        else
        {
            manicureConverter.SetUpPaint();
        }
    }

    /// <summary>
    /// 애니메이션 작동 완료후 paintEngine의 그리기모드 true로 변경후 선택한 손가락의 paintEngine으로 정보변경
    /// </summary>
    /// <param name="index"></param>
    public void SelectFinger(int index)
    {
        for (int i = 0; i < 5; i++)
        {
            toesNailButtons[index].interactable = false;
        }

        fingerToesParticlePositioner.ActiveOnOffParticle(0, true, false);

        RectTransform rectTransform = questPreview.GetComponent<RectTransform>();
        Vector2 preViewScale = rectTransform.localScale;
        if (preViewScale.x != 1)
        {
            rectTransform.localScale = new Vector2(1, 1);
        }

        QuestSelectFinger(index);
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
        selectPaintEngine = null;
        patternClone = null;

        QuestCancleFinger();
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
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 5; i++)
        {
            if (clearFingers.Contains(i))
            {
                fingerToesParticlePositioner.ActiveOnOffParticle(i, false, false);
            }
            else
            {
                fingerToesParticlePositioner.ActiveOnOffParticle(i, false, true);
            }
        }
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
                manicureItemScrollView.ResetContent("Manicure");
                manicureItemScrollView.PlayMoveContentBox();
                QuestSceneManager.Instance.SetAnswerManicureConvers();
                break;
            case "PatternScene":
                selectPaintEngine.drawEnabled = false;
                manicureItemScrollView.ResetContent("Pattern");
                manicureItemScrollView.PlayMoveContentBox();
                QuestSceneManager.Instance.SetAnswerManicureConvers();
                break;
            case "characterStickerScene":
                selectPaintEngine.drawEnabled = false;
                manicureItemScrollView.ResetContent("CharacterSticker");
                manicureItemScrollView.PlayMoveContentBox();
                CreateGuideSticker("CharacterSticker");
                QuestSceneManager.Instance.SetAnswerManicureConvers();
                break;
            case "stickerScene":
                selectPaintEngine.drawEnabled = false;
                manicureItemScrollView.ResetContent("Sticker");
                manicureItemScrollView.PlayMoveContentBox();
                CreateGuideSticker("Sticker");
                QuestSceneManager.Instance.SetAnswerManicureConvers();
                break;
            case "animationStickerScene":
                selectPaintEngine.drawEnabled = false;
                manicureItemScrollView.ResetContent("AnimationSticker");
                manicureItemScrollView.PlayMoveContentBox();
                CreateGuideSticker("AnimationSticker");
                QuestSceneManager.Instance.SetAnswerManicureConvers();
                break;
            default:
                break;
        }

        this.sceneName = sceneName;
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
    /// 각 미션마다 조건 충족시 퀘스트를 검사함
    /// </summary>
    public void QuestConfirm()
    {
        if (questConfirmBehaviour.IsClear == true)
        {
            return;
        }
        questConfirmBehaviour.QuestConfirm(paintSteps, selectFingerIndex, selectPaintEngine, patternItems, guideStickerObjects, characterStickerItems, stickerItems, animationStickerItems, clearFingers);
    }

    /// <summary>
    /// 미션을 모두 완료할시 실행
    /// </summary>
    /// <param name="finger"></param>
    /// <param name="handSpriteName"></param>
    public void ClearQuest(int finger, string handSpriteName)
    {
        Button button = toesNailMasks[finger].transform.parent.parent.GetComponent<Button>();
        button.interactable = false;

        questScene.IsFirstPlay = false;
        QuestSceneManager.Instance.Save();
    }

    /// <summary>
    /// 손가락 클릭시 작동
    /// </summary>
    /// <param name="fingerNum"></param>
    public void QuestSelectFinger(int fingerNum)
    {
        selectFingerIndex = fingerNum;
        selectPaintEngine = toesPaintEngines[selectFingerIndex];
        questPreview.SetImageOn(fingerNum, paintSteps[selectFingerIndex]);
        //각 손가락에 해당하는 미션 단계에 따라 애니메이션 실행
        questAnimController.ClickQuestUIAnim(fingerNum, paintSteps[selectFingerIndex], questConfirmBehaviour.BeforeStep);
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
    /// paintEngine 빈값으로 변경후 그리기모드 false로 변경
    /// </summary>
    public void QuestCancleFinger()
    {
        QuestSceneManager.Instance.ResetGuidePaint();
        for (int i = 0; i < guideStickerObjects.Count; i++)
        {
            GuideStickerConverter guideStickerConverter = guideStickerObjects[i].GetComponent<GuideStickerConverter>();
            if(guideStickerConverter.anim)
            {
                Destroy(guideStickerConverter.gameObject);
            }
            else
            {
                guideStickerConverter.Hide();

            }
        }
        guideStickerObjects.Clear();

        if (characterStickerItems[selectFingerIndex].stickerConverters.Count != 0 && paintSteps[selectFingerIndex] == 2)
        {
            for (int i = 0; i < characterStickerItems[selectFingerIndex].stickerConverters.Count; i++)
            {
                Destroy(characterStickerItems[selectFingerIndex].stickerConverters[i].gameObject);
            }
            characterStickerItems[selectFingerIndex].stickerConverters = new List<StickerConverter>();
        }
        if (stickerItems[selectFingerIndex].stickerConverters.Count != 0 && paintSteps[selectFingerIndex] == 3)
        {
            for (int i = 0; i < stickerItems[selectFingerIndex].stickerConverters.Count; i++)
            {
                Destroy(stickerItems[selectFingerIndex].stickerConverters[i].gameObject);
            }
            stickerItems[selectFingerIndex].stickerConverters = new List<StickerConverter>();
        }
        if (animationStickerItems[selectFingerIndex].stickerConverters.Count != 0 && paintSteps[selectFingerIndex] == 3)
        {
            for (int i = 0; i < animationStickerItems[selectFingerIndex].stickerConverters.Count; i++)
            {
                Destroy(animationStickerItems[selectFingerIndex].stickerConverters[i].gameObject);
            }
            animationStickerItems[selectFingerIndex].stickerConverters = new List<StickerConverter>();
        }

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
        for (int i = 0; i < toesNailButtons.Length; i++)
        {
            if (clearFingers.Contains(i) == false)
            {
                toesNailButtons[i].interactable = isOn;
            }
            else
            {
                toesNailButtons[i].interactable = false;
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
        if (clearFingers.Contains(selectFingerIndex))
        {
            yield return new WaitForSeconds(1.65f);
            fingerToesParticlePositioner.CreateCelearParticle(selectFingerIndex);
        }
    }

    /// <summary>
    /// 반투명 스티커 이미지 생성
    /// </summary>
    /// <param name="stickerTypeName"></param>
    public void CreateGuideSticker(string stickerTypeName)
    {
        if (guideStickerObjects.Count == 0)
        {
            guideStickerObjects = new List<GameObject>();
            StickerType stickerType = Enum.Parse<StickerType>(stickerTypeName);
            List<QuestStickerData> questStickerDatas = DataManager.Instance.GetQuestStickerDataWithType(level, selectFingerIndex, stickerType);

            beautyItemSpawner.CreateGuideSticker(toesNailMasks[selectFingerIndex].transform, questStickerDatas, this);
        }
    }

    /// <summary>
    /// 각 스텝에 맞는 UI애니메이션 실행
    /// </summary>
    public void ClicekdQuestButtonWithUIAnim()
    {
        questAnimController.ClickQuestUIAnim(selectFingerIndex, paintSteps[selectFingerIndex], questConfirmBehaviour.BeforeStep);
    }

    /// <summary>
    /// 퀘스트 손가락 꾸미기에서 뒤로가기 버튼 클릭시 애니메이션 실행
    /// </summary>
    /// <param name="isClear"></param>
    public void ClickedQuestSceneBackButton(bool isClear)
    {
        questAnimController.ClickedQuestSceneBackButton(isClear);
    }

    /// <summary>
    /// 스텝에 맞는 미리보기 스텝 실행
    /// </summary>
    public void PreviewStep()
    {
        questPreview.ShowPreviewStep(paintSteps[selectFingerIndex], true);
        QuestSceneManager.Instance.ResetGuidePaint();
    }

    /// <summary>
    /// 미션완료시 해당 손가락의 프리뷰의 파티클 출현
    /// </summary>
    /// <param name="clearfinger"></param>
    /// <param name="isClear"></param>
    public void ShowClearMark(int clearfinger, bool isClear)
    {
        showClearMark.Add(clearfinger);
        questPreview.ClearAnim();
        questPreview.ShowClearMark(clearfinger);
    }

    /// <summary>
    /// 현재 스텝 확인
    /// </summary>
    /// <returns></returns>
    public int GetSceneStep()
    {
        int sceneStep = paintSteps[selectFingerIndex];
        return sceneStep;
    }

    /// <summary>
    /// 스텝 완료여부
    /// </summary>
    /// <returns></returns>
    public bool IsClearing()
    {
        bool isClear = questConfirmBehaviour.IsClear;
        return isClear;
    }

    public void SetClearing(bool isClear)
    {
        questConfirmBehaviour.IsClear = isClear;
    }

    public void Hide()
    {
        questScene.Hide();
    }
}