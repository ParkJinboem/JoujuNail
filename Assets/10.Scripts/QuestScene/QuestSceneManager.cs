using OnDot.Util;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestSceneManager : PersistentSingleton<QuestSceneManager>
{
    public delegate void TabButtonInitOnHandler();
    public static event TabButtonInitOnHandler OnTabButtonInited;
    public delegate void TabButtonInitOffHandler();
    public static event TabButtonInitOffHandler OffTabButtonInited;

    public delegate void TabButtonChangedHandler(TabButtonType tabButtonType);
    public static event TabButtonChangedHandler OnTabButtonChanged;

    public delegate void BrushIconChangedHandler(ManicureInfoData manicureInfoData);
    public static event BrushIconChangedHandler OnBrushIconChanged;

    [Header("Setting")]
    [SerializeField] private QuestHandPaintSceneController questHandPaintSceneController;
    [SerializeField] private QuestFootPaintSceneController questFootPaintSceneController;
    [SerializeField] private QuestSceneSaveBehaviour questSceneSaveBehaviour;
    [SerializeField] private GuidePaintBehaviour guidePaintBehaviour;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private QuestPaintSceneItem questPaintSceneItem;
    [SerializeField] private QuestPreview questPreview;
    [SerializeField] private QuestAnimController questAnimController;
    [SerializeField] private ManicureItemScrollView manicureItemScrollView;
    [SerializeField] private AnswerChecker answerChecker;
    [SerializeField] private QuestGuideHandAnim guideHandAnim;
    [SerializeField] private GameObject handRoot;
    [SerializeField] private GameObject footRoot;

    [Header("HandFootImage")]
    [SerializeField] private Image[] fingerImage;
    [SerializeField] private Image[] toesImage;

    public Image handImage;
    public Image footImage;

    private int handfootSpriteIndex;

    [Header("Nail")]
    [SerializeField] private Image[] fingerNailMasks;
    [SerializeField] private Image[] toesNailMasks;

    [Header("Cover")]
    [SerializeField] private Image[] fingerNailCovers;
    [SerializeField] private Image[] toesNailCovers;

    private QuestHandData questHandData;

    [Header("Tutorial")]
    public Transform tutorial2Parent;
    public GameObject tutorial2Target;

    public ManicureConverter answerManicureConverter;
    public ManicureConverter AnswerManicureConverter
    {
        get { return answerManicureConverter; }
        set { answerManicureConverter = value; }
    }

    private bool isFirstPlay = false;
    public bool IsFirstPlay
    {
        set { isFirstPlay = value; }
    }

    private List<IQuestable> questables = new List<IQuestable>();
    public List<IQuestable> Questables
    {
        get { return questables; }
    }

    public void RegisterQuestable(IQuestable questable)
    {
        Instance.questables.Add(questable);
    }

    public void UnregisterQuestable(IQuestable questable)
    {
        Instance.questables.Remove(questable);
    }

    /// <summary>
    /// 초기값 설정
    /// </summary>
    /// <param name="level"></param>
    /// <param name="bgObject"></param>
    public void Init(int level, GameObject bgObject)
    {
        //튜토리얼1을 클리어하지않았으면 클리어 한것으로 체크
        if(!PlayerDataManager.Instance.IsCompleteTutorial(1))
        {
            TutorialManager.Instance.ClearTutorial(1);
            TutorialManager.Instance.TutorialHide();
        }
        //튜토리얼2를 클리어하지 않았으면 튜토리얼 2 실행
        if(tutorial2Parent.childCount == 0)
        {
            TutorialManager.Instance.CheckClearTutorial(2, tutorial2Parent, tutorial2Target);
        }
        
        questHandData = DataManager.Instance.GetQuestHandDataWithLevel(level);
        Statics.selectType = questHandData.selectType;
        Set(level, bgObject);

        if (questPreview.gameObject.activeSelf == false)
        {
            questPreview.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 로딩중 타입의 맞는 페인트엔진 활성화
    /// </summary>
    /// <param name="count"></param>
    public void InitPaintEngine(int count)
    {
        if (questHandData.selectType == SelectType.Hand)
        {
            questHandPaintSceneController.fingerPaintEngines[count].gameObject.SetActive(true);
        }
        else if (questHandData.selectType == SelectType.Foot)
        {
            questFootPaintSceneController.toesPaintEngines[count].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 초기값 설정 
    /// </summary>
    /// <param name="level"></param>
    /// <param name="bgObject"></param>
    public void Set(int level, GameObject bgObject)
    {
        rectTransform.anchoredPosition3D = Vector3.zero;
        rectTransform.sizeDelta = Vector2.zero;

        SpriteRenderer bgSpriteRenderer = bgObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        bgSpriteRenderer.sprite = DataManager.Instance.GetBgSprite(questHandData.bgSpriteString);

        SetHandFootImage(questHandData.selectType);

        manicureItemScrollView.Init();

        //첫 실행일경우
        if (isFirstPlay == false)
        {
            questPreview.Init(questHandData);
            guidePaintBehaviour.Init(questHandData);
        }

        questPreview.SetHandFootImage();

        if (questHandData.selectType == SelectType.Hand)
        {
            questHandPaintSceneController.RegisterQuestable();
            for (int i = 0; i < fingerNailMasks.Length; i++)
            {
                QuestFingerData questFingerData = questHandData.questFingerDatas[i];
                fingerNailMasks[i].sprite = DataManager.Instance.GetFingerNailMaskSprite(questFingerData.nailMaskString);
                fingerNailCovers[i].sprite = DataManager.Instance.GetFingerNailCoverSprite(questFingerData.nailCoverString);
            }
        }
        else if (questHandData.selectType == SelectType.Foot)
        {
            questFootPaintSceneController.RegisterQuestable();
            for (int i = 0; i < toesNailMasks.Length; i++)
            {
                QuestFingerData questFingerData = questHandData.questFingerDatas[i];
                toesNailMasks[i].sprite = DataManager.Instance.GetToesNailMaskSprite(questFingerData.nailMaskString);
                toesNailCovers[i].sprite = DataManager.Instance.GetToesNailCoverSprite(questFingerData.nailCoverString);
            }
        }

        questAnimController.Init(questables[0]);
        answerChecker.Init(questables[0]);

        if (questHandData.selectType == SelectType.Hand)
        {
            questables[0].Init(level, questHandData, handImage, fingerNailMasks);
        }
        else if (questHandData.selectType == SelectType.Foot)
        {
            questables[0].Init(level, questHandData, handImage, toesNailMasks);
        }

        isFirstPlay = true;
    }

    /// <summary>
    /// 손, 발, 손가락, 발가락 색상 설정
    /// </summary>
    /// <param name="selectType"></param>
    public void SetHandFootImage(SelectType selectType)
    {
        Info info = PlayerDataManager.Instance.GetInfo();
        handfootSpriteIndex = info.handfootIndex;

        if (selectType == SelectType.Hand)
        {
            if (handRoot.activeSelf == false || footRoot.activeSelf == true)
            {
                handRoot.SetActive(true);
                footRoot.SetActive(false);
            }

            handImage.sprite = DataManager.Instance.GetHandSprite(info.handImageName);

            for (int i = 0; i < 5; i++)
            {
                fingerImage[i].sprite = DataManager.Instance.GetFingerSprite(info.fingerImagenames[i]);
            }
        }
        else if (selectType == SelectType.Foot)
        {
            if (handRoot.activeSelf == true || footRoot.activeSelf == false)
            {
                handRoot.SetActive(false);
                footRoot.SetActive(true);
            }

            footImage.sprite = DataManager.Instance.GetFootSprite(info.footImageName);

            for (int i = 0; i < 5; i++)
            {
                toesImage[i].sprite = DataManager.Instance.GetToesSprite(info.toesImagenames[i]);
            }
        }
    }

    /// <summary>
    /// 메인씬으로 이동
    /// </summary>
    public void MoveToMainScene()
    {
        questables[0].MoveToMainScene();
    }

    /// <summary>
    /// 손, 발 이미지 변경
    /// </summary>
    public void ChangeHand()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        handfootSpriteIndex += 1;
        if (handfootSpriteIndex > Statics.handfootSpriteCount)
        {
            handfootSpriteIndex = 1;
        }

        PlayerDataManager.Instance.SaveInfo(handfootSpriteIndex);

        SetHandFootImage(questHandData.selectType);
        questPreview.SetHandFootImage();
    }

    /// <summary>
    /// 탭버튼 셋팅
    /// </summary>
    /// <param name="isInit"></param>
    public void SetTabButton(bool isInit)
    {
        if (isInit == true)
        {
            OnTabButtonInited?.Invoke();
        }
        else
        {
            OffTabButtonInited?.Invoke();
        }
    }

    /// <summary>
    /// 지우개 셋팅
    /// </summary>
    public void SetEraser()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        questables[0].SetUpEraser();
        OnTabButtonChanged?.Invoke(TabButtonType.Eraser);
    }

    /// <summary>
    /// 브러쉬 사이즈 변경
    /// </summary>
    /// <param name="size"></param>
    public void ChangeBrushSize(int size)
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        questables[0].ChangeBrushSize(size);
        if (size == 30)
        {
            OnTabButtonChanged?.Invoke(TabButtonType.BigBrush);
        }
        else
        {
            OnTabButtonChanged?.Invoke(TabButtonType.SamllBrush);
        }
    }

    /// <summary>
    /// 체크이미지 활성화 여부
    /// </summary>
    /// <param name="isOn"></param>
    /// <param name="isActive"></param>
    public void ActiveCheck(bool isOn, bool isActive)
    {
        if (isOn == true)
        {
            answerChecker.IsAcive = isActive;
        }
        answerChecker.IsOnObject(isActive);
    }

    /// <summary>
    /// 정답 매니큐어 찾기
    /// </summary>
    public void SetAnswerManicureConvers()
    {
        QuestFingerData questFingerData = questHandData.questFingerDatas[questables[0].SelectFingerIndex];
        for (int i = 0; i < manicureItemScrollView.ManicureConverters.Count; i++)
        {
            ManicureConverter manicureConverter = manicureItemScrollView.ManicureConverters[i];
            //매니큐어일 경우 정답 찾기
            if (questables[0].PaintSteps[questables[0].SelectFingerIndex] == 0)
            {
                if (questFingerData.manicureName == manicureConverter.ManicureInfoData.spriteName)
                {
                    answerManicureConverter = manicureConverter;
                }
            }
            //패턴일 경우 정답 찾기
            else if (questables[0].PaintSteps[questables[0].SelectFingerIndex] == 1)
            {
                if (questFingerData.questPatternData.patternSpriteString == manicureConverter.ManicureInfoData.spriteName)
                {
                    answerManicureConverter = manicureConverter;
                }
            }
            //캐릭터 스티커일 경우 정답 찾기
            else if (questables[0].PaintSteps[questables[0].SelectFingerIndex] == 2)
            {
                if (questFingerData.questStickerDatas[0].stickerSpriteString == manicureConverter.ManicureInfoData.spriteName)
                {
                    answerManicureConverter = manicureConverter;
                }
            }
            //스티커일 경우 정답 찾기
            else if (questables[0].PaintSteps[questables[0].SelectFingerIndex] == 3)
            {
                if (questFingerData.questStickerDatas[1].stickerSpriteString == manicureConverter.ManicureInfoData.spriteName)
                {
                    answerManicureConverter = manicureConverter;
                }
            }
            //애니메이션 캐릭터 스티커일 경우 정답 찾기
            else if (questables[0].PaintSteps[questables[0].SelectFingerIndex] == 4)
            {
                if (questFingerData.questStickerDatas[2].stickerSpriteString == manicureConverter.ManicureInfoData.spriteName)
                {
                    answerManicureConverter = manicureConverter;
                }
            }
        }

        //찾기와 동시에 정답 유도 애니메이션 실행여부 결정
        if (answerManicureConverter.IsSelect == true)
        {
            answerChecker.IsUrge = false;
            answerChecker.SelectItemToUrgeAnim();
        }
        else
        {
            answerChecker.IsUrge = true;
            answerChecker.SelectItemToUrgeAnim();
        }
    }

    /// <summary>
    /// 패턴 컬러 체크
    /// </summary>
    public void CheckPatternColor(bool soundCheck)
    {
        answerChecker.CheckPatternColor(soundCheck);
    }

    /// <summary>
    /// 브러쉬 아이콘 변경
    /// </summary>
    /// <param name="manicureInfoData"></param>
    public void ChangeBrushIcon(ManicureInfoData manicureInfoData)
    {
        OnBrushIconChanged?.Invoke(manicureInfoData);
    }

    /// <summary>
    /// 미리보기 이미지 변경 
    /// </summary>
    public void SetImageOff()
    {
        questPreview.SetImageOff();
    }

    /// <summary>
    /// 가이드 페인트 셋팅
    /// </summary>
    /// <param name="fingerNum"></param>
    /// <param name="sceneName"></param>
    public void SetupGuidePaint(int fingerNum, string sceneName)
    {
        guidePaintBehaviour.SetupGuidePaint(fingerNum, sceneName);
    }

    /// <summary>
    /// 가이드 페인트 초기화
    /// </summary>
    public void ResetGuidePaint()
    {
        guidePaintBehaviour.ResetGuidePaint(questables[0].SelectFingerIndex);
    }

    /// <summary>
    /// 가이트 페인트 숨기기
    /// </summary>
    public void HideGuidePaint()
    {
        guidePaintBehaviour.HideGuidePaint(questables[0].SelectFingerIndex);
    }

    /// <summary>
    /// 퀘스트 완료 저장
    /// </summary>
    public void Save()
    {
        if (Statics.selectType == SelectType.Hand)
        {
            questSceneSaveBehaviour.Save(questHandData, handImage.sprite.name, questables[0]);
        }
        else
        {
            questSceneSaveBehaviour.Save(questHandData, footImage.sprite.name, questables[0]);
        }
    }

    public void Hide()
    {
        questPaintSceneItem.Hide();
    }

    public void Tutorial2Action()
    {
        questPreview.ClearAnim();
    }


    #region GuideHandAnim
    public void QuestGuideHandSetUp(int actionNum)
    {
        if(manicureItemScrollView.itemParent.childCount < 8 && actionNum == 1 )
        {
            return;
        }
        //1: 스크롤액션실행
        //2: 핑거 액션 실행
        //3: 스크롤 액션 둘다 멈춤
        guideHandAnim.SetUp(actionNum);
    }
    #endregion
}