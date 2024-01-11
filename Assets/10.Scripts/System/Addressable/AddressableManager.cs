using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using OnDot.Util;
using UnityEngine.ResourceManagement.ResourceLocations;

/// <summary>
/// 어드레서블 관리
/// </summary>
public partial class AddressableManager : PersistentSingleton<AddressableManager>
{
    public delegate void AssetLoadStartHandler();
    public static event AssetLoadStartHandler AssetLoadStartEvent;

    public delegate void AssetLoadProgressHandler(float progress);
    public static event AssetLoadProgressHandler AssetLoadProgressEvent;

    public delegate void AssetLoadCompletedHandler();
    public static event AssetLoadCompletedHandler AssetLoadCompletedEvent;

    public List<AssetLabelReference> assetLabelReferences;
    public List<AssetLabelReference> assetLabelBrushReferences;

    public List<Object> loadAssets;
    public AsyncOperationHandle<IList<Object>> assetLabelReferenceHandle;
    public AsyncOperationHandle<IList<Object>> assetLabelReferenceHandleBrush;

    //private void OnDestroy()
    //{
    //    Addressables.Release(assetLabelReferenceHandle);
    //}

    /// <summary>
    /// 키로 에셋 반환
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public TObject GetAssetByKey<TObject>(string key) where TObject : Object
    {
        return (TObject)loadAssets.Find(x => x.name == key);
    }

    /// <summary>
    /// 에셋 로드
    /// </summary>
    public void LoadAssets()
    {
        loadAssets = new List<Object>();

        StartCoroutine(ILoadAssets());
        //게임 시작시 로딩시간이 길어 브러쉬는 3초뒤에 넣어줌
        StartCoroutine(ILoadAssetsBrush());
    }

    private IEnumerator ILoadAssets()
    {
        AssetLoadStartEvent?.Invoke();

        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(assetLabelReferences, Addressables.MergeMode.Union, typeof(Object));
        yield return locationsHandle;

        AssetLoadProgressEvent?.Invoke(0f);
        int allCount = locationsHandle.Result.Count;
        int loadedCount = 0;
        assetLabelReferenceHandle = Addressables.LoadAssetsAsync<Object>(locationsHandle.Result,
            addressable =>
            {
                loadedCount += 1;
                AssetLoadProgressEvent?.Invoke((float)loadedCount / allCount);
            });
        yield return assetLabelReferenceHandle;

        AssetLoadProgressEvent?.Invoke(1f);

        Addressables.Release(locationsHandle);

        loadAssets.AddRange(assetLabelReferenceHandle.Result);

        AssetLoadCompletedEvent?.Invoke();
    }

    private IEnumerator ILoadAssetsBrush()
    {
        yield return new WaitForSeconds(3.0f);
        AsyncOperationHandle <IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(assetLabelBrushReferences, Addressables.MergeMode.Union, typeof(Object));
        yield return locationsHandle;
        assetLabelReferenceHandleBrush = Addressables.LoadAssetsAsync<Object>(locationsHandle.Result,
            addressable =>
            {
            });
        yield return assetLabelReferenceHandleBrush;
        loadAssets.AddRange(assetLabelReferenceHandleBrush.Result);
    }
}
