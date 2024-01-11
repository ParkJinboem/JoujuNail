using UnityEngine;
using OnDot.System;
using OnDot.Util;
using UnityEngine.AddressableAssets;
using System;

public partial class AddressableManager : PersistentSingleton<AddressableManager>
{
    private void HandlerAssetLoadStart()
    {
        ScreenFaderManager.DirectFadeOut(ScreenFaderManager.FadeType.Loading);
    }

    public void HandlerAssetLoadCompleted(DataManager.DataInitedHandler assetLoadCompletedEvent)
    {
        assetLoadCompletedEvent();
    }

    public void FadeInSelectScene()
    {
        ScreenFaderManager.FadeIn();
    }

    internal void LoadAssets(AssetLabelReference assetLabelReferences)
    {
        throw new NotImplementedException();
    }
}
