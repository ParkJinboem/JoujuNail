using System.Collections;
using OnDot.System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI btnText;
    private bool isLoading; // 로딩중


    public void Start()
    {
        PlayerDataManager.Instance.Init();
        SoundManager.Instance.IntroInit();
        TextSetUp();
    }

    private void HandlerAssetLoadStart()
    {
        
    }

    private void HandlerAssetLoadProgress(float progress)
    {
        ScreenFaderManager.Progress($"{(int)(progress * 100)}%");
        ScreenFaderManager.Progressbar(progress);
    }

    private void HandlerAssetLoadCompleted()
    {
        MoveMain();
    }

    private void Awake()
    {
        AddressableManager.AssetLoadStartEvent += HandlerAssetLoadStart;
        AddressableManager.AssetLoadProgressEvent += HandlerAssetLoadProgress;
        AddressableManager.AssetLoadCompletedEvent += HandlerAssetLoadCompleted;
    }

    private void OnDestroy()
    {
        AddressableManager.AssetLoadStartEvent -= HandlerAssetLoadStart;
        AddressableManager.AssetLoadProgressEvent -= HandlerAssetLoadProgress;
        AddressableManager.AssetLoadCompletedEvent -= HandlerAssetLoadCompleted;
    }

    public void Loading()
    {
        SoundManager.Instance.PlayEffectSound("UIButton");
        LoadStart();
    }

    //게임 시작 버튼 클릭시 실행
    public void LoadStart()
    {
        if (isLoading)
        {
            return;
        }

        isLoading = true;

        ScreenFaderManager.DirectFadeOut(ScreenFaderManager.FadeType.Loading);

        AddressableManager.Instance.LoadAssets();
        
    }

    private void MoveMain()
    {
        StartCoroutine(ILoadMainScene());
    }

    IEnumerator ILoadMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void TextSetUp()
    {
        string language = PlayerDataManager.Instance.GetOptionData().language;
        if(language == "Korean")
        {
            btnText.text = "플레이";
        }
        else
        {
            btnText.text = "Play";
        }
    }
}
