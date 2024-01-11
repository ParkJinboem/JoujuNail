using System.Collections;
using OnDot.System;
using UnityEngine;

public class QuestSceneIntroController : MonoBehaviour
{
    public delegate void AssetLoadStartHandler();
    public static event AssetLoadStartHandler AssetLoadStartEvent;

    public delegate void AssetLoadProgressHandler(float progress);
    public static event AssetLoadProgressHandler AssetLoadProgressEvent;

    public delegate void AssetLoadCompletedHandler();
    public static event AssetLoadCompletedHandler AssetLoadCompletedEvent;

    private bool isLoading; // 로딩중

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

    public void Init(int level, GameObject bgObject)
    {
        QuestSceneManager.Instance.Init(level, bgObject);

        LoadStarter();
    }

    public void LoadStarter()
    {
        if (isLoading)
        {
            return;
        }

        isLoading = true;

        ScreenFaderManager.DirectFadeOut(ScreenFaderManager.FadeType.Loading);

        LoadStart();
    }

    private void LoadStart()
    {
        StartCoroutine(ILoadStart());
    }

    //퀘스트씬 시작시 어드밴5개 켜주는데 부하가 심해서 로딩신으로 대체한것임
    IEnumerator ILoadStart()
    {
        AssetLoadStartEvent?.Invoke();
        AssetLoadProgressEvent?.Invoke(0f);
        yield return null;

        int count = 0; //페인트엔진 숫자
        int allCount = 5;
        while (count < allCount)
        {
            QuestSceneManager.Instance.InitPaintEngine(count);
            count++;
            AssetLoadProgressEvent?.Invoke((float)count / allCount);
            yield return null;
        }

        AssetLoadProgressEvent?.Invoke(1f);
        AssetLoadCompletedEvent?.Invoke();

        ScreenFaderManager.FadeIn();
    }
}