using System.Collections.Generic;
using OnDot.Util;
using UnityEngine;
using UnityEngine.UI;

public class PaintSceneManager: PersistentSingleton<PaintSceneManager>
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
    [SerializeField] private HandPaintSceneController handPaintSceneController;
    [SerializeField] private FootPaintSceneController footPaintSceneController;
    [SerializeField] private PaintSceneSaveBehaviour paintSceneSaveBehaviour;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private PaintSceneItem paintSceneItem;
    [SerializeField] private FreeAnimController freeAnimController;
    [SerializeField] private ManicureItemScrollView manicureItemScrollView;
    [SerializeField] private HandFootPositioner handFootPositioner;
    [SerializeField] private FreeGuideHandAnim freeGuideHandAnim;

    [Header("BackGround")]
    private List<HandFootPartsData> bgPartsDatas;
    private SpriteRenderer handBg;
    public SpriteRenderer HandBg
    {
        get { return handBg; }
    }
    private SpriteRenderer footBg;
    public SpriteRenderer FootBg
    {
        get { return footBg; }
    }
    private int bgSpriteIndex = 1;

    [Header("HandFootImage")]
    private List<HandFootPartsData> handFootPartsDatas;
    [SerializeField] private Image[] fingerImages;
    [SerializeField] private Image[] toesImages;
    public Image handImage;
    public Image footImage;
    private int handfootSpriteIndex;

    [Header("Nail")]
    private List<HandFootPartsData> nailPartsDatas;
    [SerializeField] private Image[] fingerNailMasks;
    [SerializeField] private Image[] toesNailMasks;

    [Header("Cover")]
    private List<HandFootPartsData> nailCoverPartsDatas;
    [SerializeField] private Image[] fingerNailCovers;
    [SerializeField] private Image[] toesNailCovers;

    [Header("Save")]
    [SerializeField] private GameObject saveButton;
    [SerializeField] private Animator saveButtonAnimator;

    private List<IFreeable> freeables = new List<IFreeable>();
    public List<IFreeable> Freeables
    {
        get { return freeables; }
    }

    public void RegisterFreeable(IFreeable freeable)
    {
        if(freeables.Count < 1)
        {
            Instance.freeables.Add(freeable);
        }
    }

    public void UnregisterFreeable(IFreeable freeable)
    {
        Instance.freeables.Remove(freeable);
    }

    private void OnEnable()
    {
        rectTransform.anchoredPosition3D = Vector3.zero;
        rectTransform.sizeDelta = Vector2.zero;

        freeAnimController.Init(handFootPositioner);

        SetHandFootImage();
    }

    public void Init(GameObject bgObject)
    {
        manicureItemScrollView.Init();
        SetEverything(bgObject);
    }

    public void InitPaintEngine(int count)
    {
        handPaintSceneController.fingerPaintEngines[count].gameObject.SetActive(true);
        footPaintSceneController.toesPaintEngines[count].gameObject.SetActive(true);
    }

    private void SetEverything(GameObject bgOabject)
    {
        handBg = bgOabject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        footBg = bgOabject.transform.GetChild(2).GetComponent<SpriteRenderer>();

        BgClicked handBgClicked = handBg.gameObject.GetComponent<BgClicked>();
        handBgClicked.Init();
        handBgClicked.FreeAnimController = freeAnimController;
        BgClicked footBgClicked = footBg.gameObject.GetComponent<BgClicked>();
        footBgClicked.Init();
        footBgClicked.FreeAnimController = freeAnimController;

        PolygonCollider2D handPolygonCollider2D = handBg.gameObject.GetComponent<PolygonCollider2D>();
        handPolygonCollider2D.enabled = true;
        PolygonCollider2D footPolygonCollider2D = footBg.gameObject.GetComponent<PolygonCollider2D>();
        footPolygonCollider2D.enabled = true;

        NailMaskInit();

        handFootPositioner.Init();
    }

    /// <summary>
    /// 손 색상 셋팅 
    /// </summary>
    private void SetHandFootImage()
    {
        Info info = PlayerDataManager.Instance.GetInfo();
        handfootSpriteIndex = info.handfootIndex;

        handImage.sprite = DataManager.Instance.GetHandSprite(info.handImageName);
        footImage.sprite = DataManager.Instance.GetFootSprite(info.footImageName);

        for (int i = 0; i < 5; i++)
        {
            fingerImages[i].sprite = DataManager.Instance.GetFingerSprite(info.fingerImagenames[i]);
            toesImages[i].sprite = DataManager.Instance.GetToesSprite(info.toesImagenames[i]);
        }
    }

    /// <summary>
    /// 기본 손톤 설정 
    /// </summary>
    private void NailMaskInit()
    {
        HandFootPartsData fingerNailPartsData = DataManager.Instance.GetHandFootPartsDataByTypeByid(Statics.fingerNailType, 1);
        HandFootPartsData toesNailPartsData = DataManager.Instance.GetHandFootPartsDataByTypeByid(Statics.toesNailType, 1);
        HandFootPartsData fingerNailCoverPartsData = DataManager.Instance.GetHandFootPartsDataByTypeByid(Statics.fingerNailCoverType, 1);
        HandFootPartsData toesNailCoverPartsData = DataManager.Instance.GetHandFootPartsDataByTypeByid(Statics.toesNailCoverType, 1);
        for (int i = 0; i < 5; i++)
        {
            fingerNailMasks[i].sprite = DataManager.Instance.GetFingerNailMaskSprite(fingerNailPartsData.name + "_" + (i + 1).ToString("D3"));
            toesNailMasks[i].sprite = DataManager.Instance.GetToesNailMaskSprite(toesNailPartsData.name + "_" + (i + 1).ToString("D3"));
            fingerNailCovers[i].sprite = DataManager.Instance.GetFingerNailCoverSprite(fingerNailCoverPartsData.name + "_" + (i + 1).ToString("D3"));
            toesNailCovers[i].sprite = DataManager.Instance.GetToesNailCoverSprite(toesNailCoverPartsData.name + "_" + (i + 1).ToString("D3"));
        }
    }

    /// <summary>
    /// 메인씬으로 이동
    /// </summary>
    public void MoveToMainScene()
    {
        if (freeables.Count != 0)
        {
            freeables[0].MoveToMainScene(false);
        }
        else
        {
            MainSceneController.Instance.MainSceneOn(false);
            Hide();
        }
    }

    /// <summary>
    /// 인터페이스 적용후 필요데이터 셋팅
    /// </summary>
    public void SetDataWithInterface()
    {
       
        if (Statics.selectType == SelectType.Hand)
        {
            RegisterFreeable(handPaintSceneController);
            nailPartsDatas = DataManager.Instance.GetHandFootPartsDataWithType(Statics.fingerNailType);
            handFootPartsDatas = DataManager.Instance.GetHandFootPartsDataWithType(Statics.handType);
            nailCoverPartsDatas = DataManager.Instance.GetHandFootPartsDataWithType(Statics.fingerNailCoverType);
            handPaintSceneController.Init();
        }
        else
        {
            RegisterFreeable(footPaintSceneController);
            nailPartsDatas = DataManager.Instance.GetHandFootPartsDataWithType(Statics.toesNailType);
            handFootPartsDatas = DataManager.Instance.GetHandFootPartsDataWithType(Statics.footType);
            nailCoverPartsDatas = DataManager.Instance.GetHandFootPartsDataWithType(Statics.toesNailCoverType);
            footPaintSceneController.Init();
        }
        bgPartsDatas = DataManager.Instance.GetHandFootPartsDataWithType(Statics.bgType);

        freeAnimController.Freeable = freeables[0];
    }

    /// <summary>
    /// 손가락, 발가락 마스크 이미지 변경 
    /// </summary>
    public void ChangeNailMask()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        handfootSpriteIndex += 1;
        if (handfootSpriteIndex > nailPartsDatas.Count)
        {
            handfootSpriteIndex = 1;
        }

        HandFootPartsData nailPartsData = nailPartsDatas.Find(x => x.id == handfootSpriteIndex);
        HandFootPartsData nailCoverPartsData = nailCoverPartsDatas.Find(x => x.id == handfootSpriteIndex);
        if (Statics.selectType == SelectType.Hand)
        {
            for (int i = 0; i < fingerNailMasks.Length; i++)
            {
                fingerNailMasks[i].sprite = DataManager.Instance.GetFingerNailMaskSprite(nailPartsData.name + "_" + (i + 1).ToString("D3"));
                fingerNailCovers[i].sprite = DataManager.Instance.GetFingerNailCoverSprite(nailCoverPartsData.name + "_" + (i + 1).ToString("D3"));
            }
        }
        else
        {
            for (int i = 0; i < toesNailMasks.Length; i++)
            {
                toesNailMasks[i].sprite = DataManager.Instance.GetToesNailMaskSprite(nailPartsData.name + "_" + (i + 1).ToString("D3"));
                toesNailCovers[i].sprite = DataManager.Instance.GetToesNailCoverSprite(nailCoverPartsData.name + "_" + (i + 1).ToString("D3"));
            }
        }
    }

    /// <summary>
    /// 손, 발 이미지 변경
    /// </summary>
    public void ChangeHand()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        handfootSpriteIndex += 1;
        if (handfootSpriteIndex > handFootPartsDatas.Count)
        {
            handfootSpriteIndex = 1;
        }
        PlayerDataManager.Instance.SaveInfo(handfootSpriteIndex);

        Info info = PlayerDataManager.Instance.GetInfo();
        if (Statics.selectType == SelectType.Hand)
        {
            handImage.sprite = DataManager.Instance.GetHandSprite(info.handImageName);
            for (int i = 0; i < 5; i++)
            {
                fingerImages[i].sprite = DataManager.Instance.GetFingerSprite(info.fingerImagenames[i]);
            }
        }
        else
        {
            footImage.sprite = DataManager.Instance.GetFootSprite(info.footImageName);
            for (int i = 0; i < 5; i++)
            {
                toesImages[i].sprite = DataManager.Instance.GetToesSprite(info.toesImagenames[i]);
            }
        }
    }

    /// <summary>
    /// 배경 이미지 변경 
    /// </summary>
    public void ChangeBg()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        bgSpriteIndex += 1;
        if (bgSpriteIndex > bgPartsDatas.Count)
        {
            bgSpriteIndex = 1;
        }

        HandFootPartsData bgPartsData = bgPartsDatas.Find(x => x.id == bgSpriteIndex);
        HandBg.sprite = DataManager.Instance.GetBgSprite(bgPartsData.name);
        FootBg.sprite = DataManager.Instance.GetBgSprite(bgPartsData.name);
    }

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
        freeables[0].SetUpEraser();
        OnTabButtonChanged?.Invoke(TabButtonType.Eraser);
    }

    /// <summary>
    /// 브러쉬 사이즈 변경
    /// </summary>
    /// <param name="size"></param>
    public void ChangeBrushSize(int size)
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        freeables[0].ChangeBrushSize(size);
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
    /// 브러쉬 아이콘 변경
    /// </summary>
    /// <param name="manicureInfoData"></param>
    public void ChangeBrushIcon(ManicureInfoData manicureInfoData)
    {
        OnBrushIconChanged?.Invoke(manicureInfoData);
    }

    /// <summary>
    /// 데이터저장 버튼 활성화
    /// </summary>
    /// <param name="isOn"></param>
    public void SaveButtonSetActive(bool isOn)
    {
        //저장이 가능할때
        if (isOn == true)
        {
            if (freeables[0].SelectFingerCount.Count == 5)
            {
                saveButton.SetActive(true);
                saveButtonAnimator.SetTrigger("Show");
            }
        }
        //저장이 불가능할떄
        else if (isOn == false)
        {
            if (freeables.Count == 0)
            {
                return;
            }
            if (freeables[0].SelectFingerCount.Count == 5)
            {
                saveButton.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    public void Save()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        freeAnimController.uiAnimator.SetTrigger("isFreeEndAnim");
        saveButtonAnimator.SetTrigger("Hide");
        if (Statics.selectType == SelectType.Hand)
        {
            paintSceneSaveBehaviour.Save(handBg, handImage.sprite.name, freeables[0]);
        }
        else
        {
            paintSceneSaveBehaviour.Save(footBg, footImage.sprite.name, freeables[0]);
        }
    }

    public void Hide()
    {
        paintSceneItem.Hide();
    }



    #region GuideHandAnim
    public void FreeGuideHandSetUp(int actionNum)
    {
        if (manicureItemScrollView.itemParent.childCount < 8 && actionNum == 1)
        {
            
            return;
        }

        //1: 스크롤액션실행
        //2: 핑거 액션 실행
        //3: 스크롤 액션 둘다 멈춤
        freeGuideHandAnim.SetUp(actionNum);
    }

    public void PatternHandAnim()
    {
        freeables[0].PatternGuideAnim();
    }

    #endregion
}