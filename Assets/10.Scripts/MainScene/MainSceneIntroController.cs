using System.Collections;
using UnityEngine;
using OnDot.System;

public class MainSceneIntroController : MonoBehaviour
{
    public delegate void AssetLoadStartHandler();
    public static event AssetLoadStartHandler AssetLoadStartEvent;

    public delegate void AssetLoadProgressHandler(float progress);
    public static event AssetLoadProgressHandler AssetLoadProgressEvent;

    public delegate void AssetLoadCompletedHandler();
    public static event AssetLoadCompletedHandler AssetLoadCompletedEvent;

    private bool isLoading; // 로딩중

    [SerializeField] private UIButtonAnimationController uiButtonAnimationController;

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
        isLoading = false;
    }

    private void OnEnable()
    {
        AssetLoadStartEvent += HandlerAssetLoadStart;
        AssetLoadProgressEvent += HandlerAssetLoadProgress;
        AssetLoadCompletedEvent += HandlerAssetLoadCompleted;
    }

    private void OnDestroy()
    {
        AssetLoadStartEvent -= HandlerAssetLoadStart;
        AssetLoadProgressEvent -= HandlerAssetLoadProgress;
        AssetLoadCompletedEvent -= HandlerAssetLoadCompleted;
    }

    public void Init(bool isClear)
    {
        uiButtonAnimationController.Init();
        LoadStarter(isClear);
    }

    public void LoadStarter(bool isClear)
    {
        if (isLoading)
        {
            return;
        }

        isLoading = true;

        ScreenFaderManager.DirectFadeOut(ScreenFaderManager.FadeType.Loading);

        LoadStart(isClear);
    }

    private void LoadStart(bool isClear)
    {
        StartCoroutine(ILoadStart(isClear));
    }

    IEnumerator ILoadStart(bool isClear)
    {
        AssetLoadStartEvent?.Invoke();
        AssetLoadProgressEvent?.Invoke(0f);
        yield return null;
        float count = 0;
        while (count <= 0.9f)
        {
            count += Time.deltaTime;
            AssetLoadProgressEvent?.Invoke((float)count);
            yield return null;
        }

        if (isClear == true)
        {
            HandFootTextureMaker.Instance.Init();
            CollectionManager.Instance.GetCollectionIndexs();
            isClear = false;
        }

        //실행되는것이 없음_231221 박진범
        //PlayerDataManager.Instance.SaveData();

        //다음 퀘스트 데이터를 받아옴
        MainSceneController.Instance.Set();

        AssetLoadProgressEvent?.Invoke(1f);
        AssetLoadCompletedEvent?.Invoke();
        ScreenFaderManager.FadeIn();
    }
}
