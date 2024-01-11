using System.Collections;
using OnDot.System;
using UnityEngine;

public class AlbumSceneIntroController : MonoBehaviour
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

    public void Init()
    {
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

    IEnumerator ILoadStart()
    {
        AssetLoadStartEvent?.Invoke();
        AssetLoadProgressEvent?.Invoke(0f);

        yield return null;

        float time = 0;
        while (time <= 1)
        {
            time += Time.deltaTime;
            AssetLoadProgressEvent?.Invoke((float)time);
            yield return null;
        }

        AlbumSceneManager.Instance.Init();
        AssetLoadProgressEvent?.Invoke(1f);
        AssetLoadCompletedEvent?.Invoke();
        ScreenFaderManager.FadeIn();
    }
}