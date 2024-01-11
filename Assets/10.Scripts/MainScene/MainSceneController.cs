using OnDot.System;
using OnDot.Util;
using UnityEngine;

public class MainSceneController : PersistentSingleton<MainSceneController>
{
    [SerializeField] private MainSceneIntroController mainSceneIntroController;
    [SerializeField] private SceneSpawner sceneSpawner;

    [Header("Scene")]
    [SerializeField] private GameObject mainSceneUI;    //메인씬UI 루트
    [SerializeField] private Transform transformUI;     //퀘스트씬 및 프리씬 및 앨범씬의 부모위치

    [Header("InGame")]
    private int level;
    public int Level
    {
        get { return level; }
        set { level = value;}
    }

    [Header("BG")]
    [SerializeField] private GameObject freePaintSceneBg;
    [SerializeField] private GameObject questPaintSceneBg;

    //isClear True일시 
    public void Init(bool isClear)
    {
        mainSceneIntroController.Init(isClear);
    }

    /// <summary>
    /// 퀘스트 레벨 셋팅
    /// </summary>
    public void Set()
    {
        bool isHave = PlayerDataManager.Instance.IsHaveQuest();
        if (isHave == true)
        {
            Quest quest = PlayerDataManager.Instance.GetQuest();
            Statics.level = quest.level;
            level = Statics.level;
        }
        else
        {
            Statics.level = 1;
            level = Statics.level;
        }
        Statics.selectManicureId = PlayerDataManager.Instance.GetInfo().curSelectedManicureId;
    }

    /// <summary>
    /// 퀘스트 씬 생성(메인씬의 퀘스트스타일 버튼 클릭 )
    /// </summary>
    public void MoveToQuestScene()
    {
        //로딩씬이 완료되기전에는 리턴
        if(0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }
        SoundManager.Instance.PlayEffectSound("UIButton");
        StopAllCoroutines();
        Statics.gameMode = GameMode.Quest;
        mainSceneUI.SetActive(false);
        questPaintSceneBg.SetActive(true);
        
        sceneSpawner.CreateQuestPaintScene(level, questPaintSceneBg, transformUI);
        SoundManager.Instance.PlayBGM("BGM_06");
    }

    /// <summary>
    /// 프리 씬 생성 
    /// </summary>
    public void MoveToPaintScene()
    {
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }
        SoundManager.Instance.PlayEffectSound("UIButton");
        StopAllCoroutines();
        Statics.gameMode = GameMode.Free;
        freePaintSceneBg.SetActive(true);
        mainSceneUI.SetActive(false);
        sceneSpawner.CreatePaintScene(freePaintSceneBg, transformUI);
        SoundManager.Instance.PlayBGM("BGM_06");
    }

    /// <summary>
    /// 앨범 씬 생성 
    /// </summary>
    public void MoveToAlbumScene()
    {
        if (0 < ScreenFaderManager.Instance.canvasAlphaNumber())
        {
            return;
        }
        SoundManager.Instance.PlayEffectSound("UIButton");
        StopAllCoroutines();
        mainSceneUI.SetActive(false);
        sceneSpawner.CreateAlbumScene(transformUI);
    }

    /// <summary>
    /// 씬별로 홈버튼 누를시 실행되는 함수
    /// </summary>
    /// <param name="isClear"></param>
    public void MainSceneOn(bool isClear)
    {
        if (freePaintSceneBg.activeSelf == true)
        {
            freePaintSceneBg.SetActive(false);
        }
        if (questPaintSceneBg.activeSelf == true)
        {
            questPaintSceneBg.SetActive(false);
        }
        mainSceneUI.SetActive(true);

        Init(isClear);
        SoundManager.Instance.PlayBGM("BGM_05");
    }

    /// <summary>
    /// 설정 팝업
    /// </summary>
    public void ShowSetting()
    {
        PopupManager.Instance.ShowSetting();
    }
}